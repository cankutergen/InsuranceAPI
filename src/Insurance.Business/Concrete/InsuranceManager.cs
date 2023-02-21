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
        private readonly ISurchargeRateService _surchargeRateService;

        public InsuranceManager(IProductService productService, IProductTypeService productTypeService, ILogBuilder logBuilder, ISurchargeRateService surchargeRateService)
        {
            _productService = productService;
            _productTypeService = productTypeService;
            _logBuilder = logBuilder;
            _surchargeRateService = surchargeRateService;
        }

        public async Task<InsuranceModel> CalculateInsuranceAmountAsync(InsuranceModel insuranceModel)
        {
            try
            {
                if (!insuranceModel.ProductTypeHasInsurance)
                {
                    Log.Information($"Product has no insurance {JsonConvert.SerializeObject(insuranceModel)}");
                    insuranceModel.InsuranceValue = 0;
                    return insuranceModel;
                }

                AddRegularRuleAmount(ref insuranceModel);
                AddSpecialRuleAmount(ref insuranceModel);

                var surchargeRate = await _surchargeRateService.GetSurchargeRateByProductTypeIdAsync(insuranceModel.ProductTypeId);
                AddSurchargeAmount(surchargeRate, ref insuranceModel);

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

        private void AddRegularRuleAmount(ref InsuranceModel insuranceModel)
        {
            var regularRule = RegularInsuranceRuleFactory.CreateRule(insuranceModel.SalesPrice);
            if (regularRule == null)
            {
                Log.Warning($"Regular insurance rule is not found for object {JsonConvert.SerializeObject(insuranceModel)}");
            }
            else
            {
                insuranceModel.InsuranceValue += regularRule.GetInsuranceAmount();
            }
        }

        private void AddSpecialRuleAmount(ref InsuranceModel insuranceModel)
        {
            var specialRule = SpecialInsuranceRuleFactory.CreateRule(insuranceModel.ProductTypeId);
            if (specialRule == null)
            {
                Log.Information($"Special insurance rule is not found for object {JsonConvert.SerializeObject(insuranceModel)}");
            }
            else
            {
                insuranceModel.InsuranceValue += specialRule.GetInsuranceAmount();
            }
        }

        private void AddSurchargeAmount(SurchargeRate surchargeRate, ref InsuranceModel insuranceModel)
        {
            if (surchargeRate == null)
            {
                Log.Information($"Surcharge rate is not found for object {JsonConvert.SerializeObject(insuranceModel)}");
            }
            else
            {
                var surchargeAmount = (float)(surchargeRate.Rate / 100.0) * insuranceModel.InsuranceValue;
                insuranceModel.InsuranceValue += surchargeAmount;
            }
        }
    }
}
