using Insurance.Business.Factory.Insurance.Abstract;
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
        public static ISpecialInsuranceRule CreateRule(string productType)
        {
            switch (productType)
            {
                case "Smartphones":
                    return new SmartphoneInsuranceRule();
                case "Laptops":
                    return new LaptopInsuranceRule();
                default:
                    return null;
            }
        }
    }
}
