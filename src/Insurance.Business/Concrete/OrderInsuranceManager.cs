using Insurance.Business.Abstract;
using Insurance.Business.Builder;
using Insurance.Business.Factory.OrderInsuranceRule;
using Insurance.Core.Logging;
using Insurance.Entities.ComplexTypes;
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
    public class OrderInsuranceManager : IOrderInsuranceService
    {
        private readonly IInsuranceService _insuranceService;
        private readonly ILogBuilder _logBuilder;

        public OrderInsuranceManager(IInsuranceService insuranceService, ILogBuilder logBuilder)
        {
            _insuranceService = insuranceService;
            _logBuilder = logBuilder;
        }

        public async Task<OrderInsurance> PopulateOrderInsuranceAsync(List<OrderProduct> orderProducts)
        {
            try
            {
                OrderInsurance orderInsurance = new OrderInsurance();
                
                foreach (var orderProduct in orderProducts)
                {
                    InsuranceModel insuranceModel = await _insuranceService.PopulateInsuranceByProductIdAsync(orderProduct.ProductId);
                    if(insuranceModel == null)
                    {
                        throw new Exception($"Insurance with product id: {orderProduct.ProductId} is not found");
                    }

                    insuranceModel = await _insuranceService.CalculateInsuranceAmountAsync(insuranceModel);

                    OrderProductDetailBuilder builder = new OrderProductDetailBuilder();
                    var orderProductDetail = builder.Build(insuranceModel, orderProduct.Quantity);
                    orderProductDetail.TotalInsurance = insuranceModel.InsuranceValue * orderProduct.Quantity;

                    orderInsurance.TotalInsuranceAmount += orderProductDetail.TotalInsurance;
                    orderInsurance.OrderProductDetails.Add(orderProductDetail);
                }

                var specialRule = SpecialInsuranceOrderRuleFactory.CreateRule(orderInsurance);
                if(specialRule == null)
                {
                    Log.Information($"Special insurance rule is not found for object {JsonConvert.SerializeObject(orderInsurance)}");
                }
                else
                {
                    orderInsurance.TotalInsuranceAmount += specialRule.GetInsuranceAmount();
                }

                return orderInsurance;
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), orderProducts));
                throw new Exception(ex.Message);
            }
        }
    }
}
