using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Abstract
{
    public interface ISurchargeRateService
    {
        Task<SurchargeRate> GetSurchargeRateByIdAsync(int id);

        Task<SurchargeRate> GetSurchargeRateByProductTypeIdAsync(int productTypeId);

        Task<SurchargeRate> CreateSurchargeRateAsync(SurchargeRate surchargeRate);

        Task<SurchargeRate> UpdateSurchargeRateAsync(SurchargeRate surchargeRate);

        Task DeleteSurchargeRateByIdAsync(int id);
    }
}
