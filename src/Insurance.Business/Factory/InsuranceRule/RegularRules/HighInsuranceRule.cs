using Insurance.Business.Factory.InsuranceRule.Abstract;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.InsuranceRule.RegularRules
{
    public class HighInsuranceRule : IRegularInsuranceRule
    {
        public float GetInsuranceAmount()
        {
            return 2000;
        }
    }
}
