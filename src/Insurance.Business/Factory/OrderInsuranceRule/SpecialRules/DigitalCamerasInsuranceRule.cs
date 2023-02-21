using Insurance.Business.Factory.OrderInsuranceRule.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.OrderInsuranceRule.SpecialRules
{
    public class DigitalCamerasInsuranceRule : ISpecialOrderInsuranceRule
    {
        public float GetInsuranceAmount()
        {
            return 500;
        }
    }
}
