using Insurance.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Core.DataAccess
{
    public interface IApiRepository<TResponse, TError> 
        where TResponse : class, IEntity, new()
        where TError : class, IError, new()
    {
        Task<TResponse> GetAsync(string query);

        Task<List<TResponse>> GetListAsync(string query);
    }
}
