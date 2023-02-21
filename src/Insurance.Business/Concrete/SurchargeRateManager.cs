
using FluentValidation;
using Insurance.Business.Abstract;
using Insurance.Core.CrossCuttingConcerns.FluentValidation;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Concrete
{
    public class SurchargeRateManager : ISurchargeRateService
    {
        private readonly ISurchargeRateDal _surchargeRateDal;
        private readonly IValidator<SurchargeRate> _validator;
        private readonly ILogBuilder _logBuilder;

        public SurchargeRateManager(ISurchargeRateDal surchargeRateDal, IValidator<SurchargeRate> validator, ILogBuilder logBuilder)
        {
            _surchargeRateDal = surchargeRateDal;
            _validator = validator;
            _logBuilder = logBuilder;
        }

        public async Task<SurchargeRate> CreateSurchargeRateAsync(SurchargeRate surchargeRate)
        {
            ValidatorTool.FluentValidate(_validator, surchargeRate);

            var entity = await GetSurchargeRateByProductTypeIdAsync(surchargeRate.ProductTypeId);
            if(entity != null)
            {
                string errorMessage = $"Surcharge rate is already existing for product type with id: {surchargeRate.ProductTypeId}";

                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), errorMessage, surchargeRate));
                throw new Exception(errorMessage);
            }

            return await _surchargeRateDal.AddAsync(surchargeRate);
        }

        public async Task DeleteSurchargeRateByIdAsync(int id)
        {
            var entity = await GetSurchargeRateByIdAsync(id);
            if(entity == null)
            {
                string errorMessage = $"Surcharge rate with id:{id} does not found";

                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), errorMessage, id));
                throw new Exception(errorMessage);
            }

            await _surchargeRateDal.DeleteAsync(entity);
        }

        public async Task<SurchargeRate> GetSurchargeRateByIdAsync(int id)
        {
            return await _surchargeRateDal.GetAsync(x => x.Id == id);
        }

        public async Task<SurchargeRate> GetSurchargeRateByProductTypeIdAsync(int productTypeId)
        {
            return await _surchargeRateDal.GetAsync(x => x.ProductTypeId == productTypeId);
        }

        public async Task<SurchargeRate> UpdateSurchargeRateAsync(SurchargeRate surchargeRate)
        {
            var entity = await GetSurchargeRateByIdAsync(surchargeRate.Id);
            if (entity == null)
            {
                string errorMessage = $"Surcharge rate with id:{surchargeRate.Id} does not found";

                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), errorMessage, surchargeRate));
                throw new Exception(errorMessage);
            }

            ValidatorTool.FluentValidate(_validator, surchargeRate);
            return await _surchargeRateDal.UpdateAsync(surchargeRate);
        }
    }
}
