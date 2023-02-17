using Insurance.Business.Abstract;
using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Tests
{
    public class InsuranceOrderManagerTests
    {
        private readonly Mock<IInsuranceService> _insuranceServiceMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly InsuranceOrderManager _insuranceOrderManager;

        public InsuranceOrderManagerTests()
        {
            _insuranceServiceMock = new Mock<IInsuranceService>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _insuranceOrderManager = new InsuranceOrderManager(_insuranceServiceMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task PopulateInsuranceOrderByProductIdList_GivenProductListSalesPriceLessThan500Euros_ShouldAddZeroToTotalInsuranceCost()
        {
            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmount(It.IsAny<InsuranceModel>()))
                .Returns(new InsuranceModel() { InsuranceValue = 0});

            var result = await _insuranceOrderManager.PopulateInsuranceOrderByProductIdList(new List<int> { 1, 2, 3});
            Assert.Equal(0, result.TotalInsuranceAmount);
        }

        [Fact]
        public async Task PopulateInsuranceOrderByProductIdList_GivenProductListSalesPriceLessThan500EurosLaptops_ShouldAdd1500EurosToTotalInsuranceCost()
        {
            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmount(It.IsAny<InsuranceModel>()))
                .Returns(new InsuranceModel() { InsuranceValue = 500 });

            var result = await _insuranceOrderManager.PopulateInsuranceOrderByProductIdList(new List<int> { 1, 2, 3 });
            Assert.Equal(1500, result.TotalInsuranceAmount);
        }

        [Fact]
        public async Task PopulateInsuranceOrderByProductIdList_GivenInvalidInput_ShouldThrowError()
        {
            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Throws(new Exception());

            Task result() => _insuranceOrderManager.PopulateInsuranceOrderByProductIdList(new List<int> { 1, 2, 3 });
            await Assert.ThrowsAsync<Exception>(result);
        }
    }
}
