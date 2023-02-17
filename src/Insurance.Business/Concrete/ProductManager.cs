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
            return await _productApi.GetList("/products");
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productApi.Get($"/products/{id}");
        }
    }
}
