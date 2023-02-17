using Insurance.Business.Factory.Insurance.Abstract;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Factory.Insurance.SpecialRules
{
    public class SmartphoneInsuranceRule : ISpecialInsuranceRule
    {
        public float GetInsuranceAmount()
        {
            return 500;
        }
    }
}
