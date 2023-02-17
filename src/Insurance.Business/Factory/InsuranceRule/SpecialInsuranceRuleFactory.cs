using Insurance.Business.Factory.InsuranceRule.Abstract;
using Insurance.Business.Factory.Insurance.SpecialRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.Insurance
{
    public class SpecialInsuranceRuleFactory
    {
        public static ISpecialInsuranceRule CreateRule(int productTypeId)
        {
            switch (productTypeId)
            {
                case 32:
                    return new SmartphoneInsuranceRule();
                case 21:
                    return new LaptopInsuranceRule();
                default:
                    return null;
            }
        }
    }
}
