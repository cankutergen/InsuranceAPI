using AutoMapper;
using Insurance.Api.Models;
using Insurance.Api.Models.Insurance;
using Insurance.Api.Models.SurchargeRate;
using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Surcharge")]
    public class SurchargeRateController : ControllerBase
    {
        private readonly ISurchargeRateService _surchargeRateService;
        private readonly IMapper _mapper;
        private readonly ILogBuilder _logBuilder;

        public SurchargeRateController(ISurchargeRateService surchargeRateService, IMapper mapper, ILogBuilder logBuilder)
        {
            _surchargeRateService = surchargeRateService;
            _mapper = mapper;
            _logBuilder = logBuilder;
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _surchargeRateService.GetSurchargeRateByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }

                var responseModel = _mapper.Map<SurchargeResponseModel>(result);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), id));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("GetByProductTypeId")]
        public async Task<IActionResult> GetByProductTypeId(int productTypeId)
        {
            try
            {
                var result = await _surchargeRateService.GetSurchargeRateByProductTypeIdAsync(productTypeId);
                if (result == null)
                {
                    return NotFound();
                }

                var responseModel = _mapper.Map<SurchargeResponseModel>(result);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), productTypeId));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SurchargePostRequestModel requestModel)
        {
            try
            {
                var model = _mapper.Map<SurchargeRate>(requestModel);
                var result = await _surchargeRateService.CreateSurchargeRateAsync(model);

                var responseModel = _mapper.Map<SurchargeResponseModel>(result);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), requestModel));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SurchargePutRequestModel requestModel)
        {
            try
            {
                var model = _mapper.Map<SurchargeRate>(requestModel);
                var result = await _surchargeRateService.UpdateSurchargeRateAsync(model);

                var responseModel = _mapper.Map<SurchargeResponseModel>(result);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), requestModel));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                await _surchargeRateService.DeleteSurchargeRateByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), id));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
        }
    }
}
