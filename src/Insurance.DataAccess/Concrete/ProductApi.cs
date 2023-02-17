using Insurance.Core.DataAccess.RestSharp;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.DataAccess.Concrete
{
    public class ProductApi : ApiRepositoryBase<Product, ErrorModel>, IProductApi
    {
        public ProductApi(RestClient restClient, ILogBuilder logBuilder) : base(restClient, logBuilder)
        {
        }
    }
}
