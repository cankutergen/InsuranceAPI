using Insurance.Core.Logging;
using Insurance.Entities.Abstract;
using Insurance.Entities.Concrete;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Core.DataAccess.RestSharp
{
    public class ApiRepositoryBase<TResponse, TError> : IApiRepository<TResponse, TError> 
        where TResponse : class, IEntity, new()
        where TError : class, IError, new()
    {
        private readonly RestClient _restClient;
        private readonly ILogBuilder _logBuilder;

        public ApiRepositoryBase(RestClient restClient, ILogBuilder logBuilder)
        {
            _restClient = restClient;
            _logBuilder = logBuilder;
        }

        public async Task<TResponse> Get(string query) 
        {
            try
            {
                var request = new RestRequest(query, Method.Get);

                // may return TResponse or TError
                var result = await _restClient.GetAsync(request);

                var errorModel = JsonConvert.DeserializeObject<TError>(result.Content);
                if (errorModel.Status != 0)
                {
                    string errorMessage = JsonConvert.SerializeObject(errorModel);
                    Log.Information(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), errorMessage, query));
                }

                return JsonConvert.DeserializeObject<TResponse>(result.Content);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), query));
                return null;
            }
        }

        public async Task<List<TResponse>> GetList(string query) 
        {
            try
            {
                var request = new RestRequest(query, Method.Get);

                var result = await _restClient.GetAsync(request);

                var errorModel = JsonConvert.DeserializeObject<TError>(result.Content);
                if (errorModel.Status != 0)
                {
                    string errorMessage = JsonConvert.SerializeObject(errorModel);
                    Log.Information(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), errorMessage, query));
                }

                return JsonConvert.DeserializeObject<List<TResponse>>(result.Content);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), query));
                return new List<TResponse>();
            }
        }
    }
}
