using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Builder.Insurance
{
    public interface IInsuranceBuilder
    {
        InsuranceModel BuildWithProductInformation(Product product, ProductType productType);
    }
}
