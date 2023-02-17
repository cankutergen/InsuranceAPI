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
    public class HomeController: Controller
    {
        private readonly IInsuranceService _insuranceService;
        private readonly IMapper _mapper;
        private readonly ILogBuilder _logBuilder;

        public HomeController(IInsuranceService insuranceService, IMapper mapper, ILogBuilder logBuilder)
        {
            _insuranceService = insuranceService;
            _mapper = mapper;
            _logBuilder = logBuilder;
        }

        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<IActionResult> CalculateInsurance([FromBody] InsuranceApiModel toInsure)
        {
            try
            {
                InsuranceModel insuranceModel = await _insuranceService.PopulateInsuranceByProductId(toInsure.ProductId);
                insuranceModel = _insuranceService.CalculateInsuranceAmount(insuranceModel);

                InsuranceApiModel response = _mapper.Map<InsuranceApiModel>(insuranceModel);
                return await Task.FromResult(Ok(response));
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), toInsure));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500});
            }
        }
    }
}