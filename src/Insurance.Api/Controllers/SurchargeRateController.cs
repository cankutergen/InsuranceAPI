using AutoMapper;
using Insurance.Api.Models;
using Insurance.Api.Models.Insurance;
using Insurance.Api.Models.SurchargeRate;
using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Reflection;
using System.Threading;
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
        private readonly IMemoryCache _cache;

        public SurchargeRateController(ISurchargeRateService surchargeRateService, IMapper mapper, ILogBuilder logBuilder, IMemoryCache cache)
        {
            _surchargeRateService = surchargeRateService;
            _mapper = mapper;
            _logBuilder = logBuilder;
            _cache = cache;
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!_cache.TryGetValue($"GetById_{id}", out SurchargeResponseModel responseModel))
                {
                    var result = await _surchargeRateService.GetSurchargeRateByIdAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }

                    responseModel = _mapper.Map<SurchargeResponseModel>(result);
                    _cache.Set($"GetById_{id}", responseModel, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddDays(1)});
                }

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
                if (!_cache.TryGetValue($"GetByProductTypeId_{productTypeId}", out SurchargeResponseModel responseModel))
                {
                    var result = await _surchargeRateService.GetSurchargeRateByProductTypeIdAsync(productTypeId);
                    if (result == null)
                    {
                        return NotFound();
                    }

                    responseModel = _mapper.Map<SurchargeResponseModel>(result);
                    _cache.Set($"GetByProductTypeId_{productTypeId}", responseModel, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddDays(1) });
                }

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
            var semaphore = new SemaphoreSlim(1, 1);

            try
            {
                await semaphore.WaitAsync();

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
            finally
            {
                semaphore.Release();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SurchargePutRequestModel requestModel)
        {
            var semaphore = new SemaphoreSlim(1, 1);

            try
            {
                await semaphore.WaitAsync();

                var model = _mapper.Map<SurchargeRate>(requestModel);
                var initialModel = await _surchargeRateService.GetSurchargeRateByIdAsync(requestModel.Id);
                var result = await _surchargeRateService.UpdateSurchargeRateAsync(model);

                _cache.Remove($"GetByProductTypeId_{initialModel.ProductTypeId}");
                _cache.Remove($"GetById_{initialModel.Id}");

                var responseModel = _mapper.Map<SurchargeResponseModel>(result);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), requestModel));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
            finally
            {
                semaphore.Release();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var semaphore = new SemaphoreSlim(1, 1);

            try
            {
                await semaphore.WaitAsync();

                var initialModel = await _surchargeRateService.GetSurchargeRateByIdAsync(id);
                await _surchargeRateService.DeleteSurchargeRateByIdAsync(id);

                _cache.Remove($"GetByProductTypeId_{initialModel.ProductTypeId}");
                _cache.Remove($"GetById_{initialModel.Id}");

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), id));
                return BadRequest(new ResponseErrorModel { Error = ex.Message, StatusCode = 500 });
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
