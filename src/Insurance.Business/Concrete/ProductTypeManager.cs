using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.DataAccess.Concrete;
using Insurance.Entities.Concrete;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Concrete
{
    public class ProductTypeManager : IProductTypeService
    {
        private readonly IProductTypeApi _productTypeApi;
        private readonly ILogBuilder _logBuilder;

        public ProductTypeManager(IProductTypeApi productTypeApi, ILogBuilder logBuilder)
        {
            _productTypeApi = productTypeApi;
            _logBuilder = logBuilder;
        }

        public async Task<List<ProductType>> GetAllProductTypesAsync()
        {
            return await _productTypeApi.GetListAsync("/product_types");
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await _productTypeApi.GetAsync($"/product_types/{id}");
        }
    }
}
