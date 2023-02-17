using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductApi _productApi;
        private readonly ILogBuilder _logBuilder;

        public ProductManager(IProductApi productApi, ILogBuilder logBuilder)
        {
            _productApi = productApi;
            _logBuilder = logBuilder;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                return await _productApi.GetList("/products");
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), new { Query = "/products" }));
                return new List<Product>();
            }        
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                return await _productApi.Get($"/products/{id}");
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), new { Query = $"/products/{id}" }));
                return null;
            }
        }
    }
}
