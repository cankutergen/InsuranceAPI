using Insurance.Core.DataAccess;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.DataAccess.Abstract
{
    public interface IProductApi : IApiRepository<Product, ErrorModel>
    {
    }
}
