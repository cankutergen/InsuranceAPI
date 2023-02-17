using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Abstract
{
    public interface IInsuranceOrderService
    {
        Task<InsuranceOrder> PopulateInsuranceOrderByProductIdList(List<int> productIdList);
    }
}
