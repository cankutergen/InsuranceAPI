using Insurance.Business.Abstract;
using Insurance.Business.Builder;
using Insurance.Business.Factory.Insurance;
using Insurance.Core.Logging;
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
    public class InsuranceManager : IInsuranceService
    {
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly ILogBuilder _logBuilder;

        public InsuranceManager(IProductService productService, IProductTypeService productTypeService, ILogBuilder logBuilder)
        {
            _productService = productService;
            _productTypeService = productTypeService;
            _logBuilder = logBuilder;
        }

        public InsuranceModel CalculateInsuranceAmount(InsuranceModel insuranceModel)
        {
            try
            {
                if (!insuranceModel.ProductTypeHasInsurance)
                {
                    Log.Information($"Product has no insurance {JsonConvert.SerializeObject(insuranceModel)}");
                    insuranceModel.InsuranceValue = 0;
                    return insuranceModel;
                }

                var regularRule = RegularInsuranceRuleFactory.CreateRule(insuranceModel.SalesPrice);
                if (regularRule == null)
                {
                    Log.Warning($"Regular insurance rule is not found for object {JsonConvert.SerializeObject(insuranceModel)}");
                }
                else
                {
                    insuranceModel.InsuranceValue += regularRule.GetInsuranceAmount();
                }

                var specialRule = SpecialInsuranceRuleFactory.CreateRule(insuranceModel.ProductTypeId);
                if (specialRule == null)
                {
                    Log.Information($"Special insurance rule is not found for object {JsonConvert.SerializeObject(insuranceModel)}");
                }
                else
                {
                    insuranceModel.InsuranceValue += specialRule.GetInsuranceAmount();
                }

                return insuranceModel;
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), insuranceModel));
                throw new Exception(ex.Message);
            }
        }

        public async Task<InsuranceModel> PopulateInsuranceByProductIdAsync(int productId)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    throw new Exception($"Product with id: {productId} is not found");
                }

                var productType = await _productTypeService.GetProductTypeByIdAsync(product.ProductTypeId);
                if (productType == null)
                {
                    throw new Exception($"Product type with id: {product.ProductTypeId} of {product.Name} with is not found");
                }

                InsuranceBuilder insuranceBuilder = new InsuranceBuilder();

                return insuranceBuilder.BuildWithProductInformation(product, productType);
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), productId));
                throw new Exception(ex.Message);
            }
        }
    }
}
