using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Insurance.Api.Models;
using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Insurance.Api.Controllers
{
    public class InsuranceController: Controller
    {
        private readonly IInsuranceService _insuranceService;
        private readonly IInsuranceOrderService _insuranceOrderService;
        private readonly IMapper _mapper;
        private readonly ILogBuilder _logBuilder;

        
        public InsuranceController(IInsuranceService insuranceService, IInsuranceOrderService insuranceOrderService, IMapper mapper, ILogBuilder logBuilder)
        {
            _insuranceService = insuranceService;
            _insuranceOrderService = insuranceOrderService;
            _mapper = mapper;
            _logBuilder = logBuilder;
        }

        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<IActionResult> CalculateInsurance([FromBody] InsuranceRequestModel toInsure)
        {
            try
            {
                if(toInsure == null)
                {
                    throw new Exception("Invalid input");
                }

                InsuranceModel insuranceModel = await _insuranceService.PopulateInsuranceByProductId(toInsure.ProductId);
                insuranceModel = _insuranceService.CalculateInsuranceAmount(insuranceModel);

                InsuranceResponseModel response = _mapper.Map<InsuranceResponseModel>(insuranceModel);
                return await Task.FromResult(Ok(response));
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), toInsure));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500});
            }
        }

        [HttpPost]
        [Route("api/insurance/order")]
        public async Task<IActionResult> CalculateOrderInsurance([FromBody] InsuranceOrderRequestModel orderApiModel)
        {
            try
            {
                if(orderApiModel == null || orderApiModel.productIdList == null)
                {
                    throw new Exception("Invalid input");
                }

                InsuranceOrder insuranceOrder = await _insuranceOrderService.PopulateInsuranceOrderByProductIdList(orderApiModel.productIdList);
                InsuranceOrderResponseModel responseModel = _mapper.Map<InsuranceOrderResponseModel>(insuranceOrder);

                return await Task.FromResult(Ok(responseModel));
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), orderApiModel));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }
    }
}