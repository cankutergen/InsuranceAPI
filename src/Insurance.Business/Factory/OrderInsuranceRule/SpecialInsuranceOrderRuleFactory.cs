using Insurance.Business.Factory.OrderInsuranceRule.Abstract;
using Insurance.Business.Factory.OrderInsuranceRule.SpecialRules;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.OrderInsuranceRule
{
    public class SpecialInsuranceOrderRuleFactory
    {
        public static ISpecialOrderInsuranceRule CreateRule(OrderInsurance orderInsurance)
        {
            bool containsDigitalCameras = orderInsurance.OrderProductDetails.Any(x => x.ProductTypeId == 33);
            if (containsDigitalCameras)
            {
                return new DigitalCamerasInsuranceRule();
            }

            return null;
        }
    }
}
