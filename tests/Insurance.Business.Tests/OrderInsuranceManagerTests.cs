using Insurance.Business.Abstract;
using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.Entities.ComplexTypes;
using Insurance.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Tests
{
    public class OrderInsuranceManagerTests
    {
        private readonly Mock<IInsuranceService> _insuranceServiceMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly OrderInsuranceManager _orderInsuranceManager;

        public OrderInsuranceManagerTests()
        {
            _insuranceServiceMock = new Mock<IInsuranceService>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _orderInsuranceManager = new OrderInsuranceManager(_insuranceServiceMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task PopulateOrderInsuranceAsync_Given2ProductsWithSalesPriceLessThan500EurosWithQuantity1_ShouldAddZeroToTotalInsuranceCost()
        {
            float expected = 0;

            List<OrderProduct> orderProducts = new List<OrderProduct>() 
            { 
                new OrderProduct {ProductId = 1, Quantity = 1 },
                new OrderProduct {ProductId = 2, Quantity = 1 }
            };

            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmountAsync(It.IsAny<InsuranceModel>()))
                .Returns(Task.FromResult(new InsuranceModel() { InsuranceValue = 0}));

            var result = await _orderInsuranceManager.PopulateOrderInsuranceAsync(orderProducts);
            Assert.Equal(expected, result.TotalInsuranceAmount);
        }

        [Fact]
        public async Task PopulateOrderInsuranceAsync_Given3ProductsWithSalesPriceLessThan500EurosLaptopsWithQuantity1_ShouldAdd1500EurosToTotalInsuranceCost()
        {
            float expected = 1500;

            List<OrderProduct> orderProducts = new List<OrderProduct>()
            {
                new OrderProduct {ProductId = 1, Quantity = 1 },
                new OrderProduct {ProductId = 2, Quantity = 1 },
                new OrderProduct {ProductId = 3, Quantity = 1 }
            };

            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmountAsync(It.IsAny<InsuranceModel>()))
                .Returns(Task.FromResult(new InsuranceModel() { InsuranceValue = 500 }));

            var result = await _orderInsuranceManager.PopulateOrderInsuranceAsync(orderProducts);
            Assert.Equal(expected, result.TotalInsuranceAmount);
        }

        [Fact]
        public async Task PopulateOrderInsuranceAsync_GivenInvalidInput_ShouldThrowError()
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>()
            {
                new OrderProduct {ProductId = 1, Quantity = 1 }
            };

            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductIdAsync(It.IsAny<int>()))
                .Throws(new Exception());

            Task result() => _orderInsuranceManager.PopulateOrderInsuranceAsync(orderProducts);
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task PopulateOrderInsuranceAsync_Given3ProductsWithDigitalCamerasWithQuantity1_ShouldAdd2000EurosToTotalInsuranceCost()
        {
            float expected = 2000;

            List<OrderProduct> orderProducts = new List<OrderProduct>()
            {
                new OrderProduct {ProductId = 1, Quantity = 1 },
                new OrderProduct {ProductId = 2, Quantity = 1 },
                new OrderProduct {ProductId = 3, Quantity = 1 }
            };

            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmountAsync(It.IsAny<InsuranceModel>()))
                .Returns(Task.FromResult(new InsuranceModel() { InsuranceValue = 500, ProductTypeId = 33 }));

            var result = await _orderInsuranceManager.PopulateOrderInsuranceAsync(orderProducts);
            Assert.Equal(expected, result.TotalInsuranceAmount);
        }

        [Fact]
        public async Task PopulateOrderInsuranceAsync_Given2ProductsSalesPriceBetween500And2000WithQuantity2_ShouldAdd4000EurosToTotalInsuranceCost()
        {
            float expected = 4000;

            List<OrderProduct> orderProducts = new List<OrderProduct>()
            {
                new OrderProduct {ProductId = 1, Quantity = 2 },
                new OrderProduct {ProductId = 2, Quantity = 2 }
            };

            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmountAsync(It.IsAny<InsuranceModel>()))
                .Returns(Task.FromResult(new InsuranceModel() { InsuranceValue = 1000 }));

            var result = await _orderInsuranceManager.PopulateOrderInsuranceAsync(orderProducts);
            Assert.Equal(expected, result.TotalInsuranceAmount);
        }

        [Fact]
        public async Task PopulateOrderInsuranceAsync_Given3ProductsWithSalesPriceLessThan500EurosLaptopsWithQuantity2_ShouldAdd3000EurosToTotalInsuranceCost()
        {
            float expected = 3000;

            List<OrderProduct> orderProducts = new List<OrderProduct>()
            {
                new OrderProduct {ProductId = 1, Quantity = 2 },
                new OrderProduct {ProductId = 2, Quantity = 2 },
                new OrderProduct {ProductId = 3, Quantity = 2 }
            };

            _insuranceServiceMock.Setup(x => x.PopulateInsuranceByProductIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock.Setup(x => x.CalculateInsuranceAmountAsync(It.IsAny<InsuranceModel>()))
                .Returns(Task.FromResult(new InsuranceModel() { InsuranceValue = 500 }));

            var result = await _orderInsuranceManager.PopulateOrderInsuranceAsync(orderProducts);
            Assert.Equal(expected, result.TotalInsuranceAmount);
        }
    }
}
