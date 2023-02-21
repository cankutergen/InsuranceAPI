using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Insurance.Api.Models;
using Insurance.Api.Models.Insurance;
using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Insurance")]
    public class InsuranceController: Controller
    {
        private readonly IInsuranceService _insuranceService;
        private readonly IOrderInsuranceService _orderInsuranceService;
        private readonly IMapper _mapper;
        private readonly ILogBuilder _logBuilder;
       
        public InsuranceController(IInsuranceService insuranceService, IOrderInsuranceService orderInsuranceService, IMapper mapper, ILogBuilder logBuilder)
        {
            _insuranceService = insuranceService;
            _orderInsuranceService = orderInsuranceService;
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

                InsuranceModel insuranceModel = await _insuranceService.PopulateInsuranceByProductIdAsync(toInsure.ProductId);
                insuranceModel = await _insuranceService.CalculateInsuranceAmountAsync(insuranceModel);

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
        public async Task<IActionResult> CalculateOrderInsurance([FromBody] OrderInsuranceRequestModel orderInsuranceRequestModel)
        {
            try
            {
                if(orderInsuranceRequestModel == null || orderInsuranceRequestModel.OrderProducts == null)
                {
                    throw new Exception("Invalid input");
                }

                OrderInsurance insuranceOrder = await _orderInsuranceService.PopulateOrderInsuranceAsync(orderInsuranceRequestModel.OrderProducts);
                OrderInsuranceResponseModel responseModel = _mapper.Map<OrderInsuranceResponseModel>(insuranceOrder);

                return await Task.FromResult(Ok(responseModel));
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), orderInsuranceRequestModel));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }
    }
}