using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Abstract
{
    public interface IOrderInsuranceService
    {
        Task<OrderInsurance> PopulateOrderInsuranceAsync(List<OrderProduct> orderProducts);
    }
}
