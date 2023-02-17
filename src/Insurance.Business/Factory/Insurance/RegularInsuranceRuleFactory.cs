using Insurance.Business.Factory.Insurance.Abstract;
using Insurance.Business.Factory.Insurance.RegularRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.Insurance
{
    public class RegularInsuranceRuleFactory
    {
        public static IRegularInsuranceRule CreateRule(float salesPrice)
        {
            if(salesPrice < 500)
            {
                return new NoInsuranceRule();
            }

            if(salesPrice >= 500 && salesPrice < 2000)
            {
                return new MidInsuranceRule();
            }

            if(salesPrice >= 2000)
            {
                return new HighInsuranceRule();
            }

            return null;
        }
    }
}
