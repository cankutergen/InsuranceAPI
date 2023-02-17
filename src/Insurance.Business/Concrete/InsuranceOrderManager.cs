using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Concrete
{
    public class InsuranceOrderManager : IInsuranceOrderService
    {
        private readonly IInsuranceService _insuranceService;
        private readonly ILogBuilder _logBuilder;

        public InsuranceOrderManager(IInsuranceService insuranceService, ILogBuilder logBuilder)
        {
            _insuranceService = insuranceService;
            _logBuilder = logBuilder;
        }

        public async Task<InsuranceOrder> PopulateInsuranceOrderByProductIdList(List<int> productIdList)
        {
            try
            {
                InsuranceOrder insuranceOrder = new InsuranceOrder();

                foreach (var id in productIdList)
                {
                    InsuranceModel insuranceModel = await _insuranceService.PopulateInsuranceByProductId(id);
                    if(insuranceModel == null)
                    {
                        throw new Exception($"Insurance with product id: {id} is not found");
                    }

                    insuranceModel = _insuranceService.CalculateInsuranceAmount(insuranceModel);
                    insuranceOrder.TotalInsuranceAmount += insuranceModel.InsuranceValue;

                    insuranceOrder.InsuranceList.Add(insuranceModel);
                }

                return insuranceOrder;
            }
            catch (Exception ex)
            {
                Log.Error(_logBuilder.BuildLog(MethodBase.GetCurrentMethod(), JsonConvert.SerializeObject(ex), productIdList));
                throw new Exception(ex.Message);
            }
        }
    }
}
