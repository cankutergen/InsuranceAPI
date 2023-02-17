using Insurance.Business.Factory.InsuranceRule.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.Insurance.SpecialRules
{
    public class LaptopInsuranceRule : ISpecialInsuranceRule
    {
        public float GetInsuranceAmount()
        {
            return 500;
        }
    }
}
