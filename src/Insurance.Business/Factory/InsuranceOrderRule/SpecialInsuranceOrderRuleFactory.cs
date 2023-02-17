using Insurance.Business.Factory.InsuranceOrderRule.SpecialRules;
using Insurance.Business.Factory.InsuranceRule.Abstract;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.InsuranceOrderRule
{
    public class SpecialInsuranceOrderRuleFactory
    {
        public static ISpecialInsuranceRule CreateRule(InsuranceOrder insuranceOrder)
        {
            bool containsDigitalCameras = insuranceOrder.InsuranceList.Any(x => x.ProductTypeName == "Digital cameras");
            if (containsDigitalCameras)
            {
                return new DigitalCamerasInsuranceRule();
            }

            return null;
        }
    }
}
