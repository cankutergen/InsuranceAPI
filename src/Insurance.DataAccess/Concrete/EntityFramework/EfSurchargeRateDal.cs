using Insurance.Core.DataAccess.EntityFramework;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.DataAccess.Concrete.EntityFramework
{
    public class EfSurchargeRateDal : EfEntityRepository<SurchargeRate, InsuranceContext>, ISurchargeRateDal
    {
    }
}
