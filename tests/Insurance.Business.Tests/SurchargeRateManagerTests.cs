using FluentValidation;
using Insurance.Business.Concrete;
using Insurance.Business.ValidationRules.FluentValidation;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Business.Tests
{
    public class SurchargeRateManagerTests
    {
        private readonly Mock<ISurchargeRateDal> _surchargeRateDalMock;
        private readonly IValidator<SurchargeRate> _validator;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly SurchargeRateManager _surchargeRateManager;

        public SurchargeRateManagerTests()
        {
            _surchargeRateDalMock = new Mock<ISurchargeRateDal>();
            _validator = new SurchargeRateValidator();
            _logBuilderMock = new Mock<ILogBuilder>();

            _surchargeRateManager = new SurchargeRateManager(_surchargeRateDalMock.Object, _validator, _logBuilderMock.Object);
        }

        [Fact]
        public async Task CreateSurchargeRateAsync_Given0ProductTypeId_ShouldThrowValidationException()
        {
            SurchargeRate surchargeRate = new SurchargeRate { ProductTypeId = 0, Rate = 1 };

            Task result() => _surchargeRateManager.CreateSurchargeRateAsync(surchargeRate);
            await Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public async Task CreateSurchargeRateAsync_GivenNegativeRate_ShouldThrowValidationException()
        {
            SurchargeRate surchargeRate = new SurchargeRate { ProductTypeId = 1, Rate = -1 };

            Task result() => _surchargeRateManager.CreateSurchargeRateAsync(surchargeRate);
            await Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public async Task CreateSurchargeRateAsync_GivenExistingProductTypeId_ShouldThrowException()
        {
            SurchargeRate surchargeRate = new SurchargeRate { ProductTypeId = 1, Rate = 10 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.ProductTypeId == 1))
                .Returns(Task.FromResult(new SurchargeRate()));

            Task result() => _surchargeRateManager.CreateSurchargeRateAsync(surchargeRate);
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task CreateSurchargeRateAsync_GivenValidObject_ShouldCreateAndReturnNewObject()
        {
            SurchargeRate surchargeRate = new SurchargeRate { ProductTypeId = 1, Rate = 10 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.ProductTypeId == 1))
                .Returns(Task.FromResult((SurchargeRate)null));

            _surchargeRateDalMock.Setup(x => x.AddAsync(surchargeRate))
                .Returns(Task.FromResult(surchargeRate));

            var result = await _surchargeRateManager.CreateSurchargeRateAsync(surchargeRate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteSurchargeRateByIdAsync_GivenNonExistingSurchargeRateId_ShouldThrowException()
        {
            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.Id == 1))
                .Returns(Task.FromResult((SurchargeRate)null));

            Task result() => _surchargeRateManager.DeleteSurchargeRateByIdAsync(1);
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task GetSurchargeRateByIdAsync_GivenId_ShouldReturnCorrespondingSurchargeRate()
        {
            SurchargeRate surchargeRate = new SurchargeRate { Id = 1, Rate= 12, ProductTypeId = 1 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.Id == 1))
                .Returns(Task.FromResult(surchargeRate));

            var result = await _surchargeRateManager.GetSurchargeRateByIdAsync(1);
            Assert.Same(result, surchargeRate);
        }        
        
        [Fact]
        public async Task GetSurchargeRateByProductTypeIdAsync_GivenId_ShouldReturnCorrespondingSurchargeRate()
        {
            SurchargeRate surchargeRate = new SurchargeRate { Id = 1, Rate= 12, ProductTypeId = 1 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.ProductTypeId == 1))
                .Returns(Task.FromResult(surchargeRate));

            var result = await _surchargeRateManager.GetSurchargeRateByProductTypeIdAsync(1);
            Assert.Same(result, surchargeRate);
        }

        [Fact]
        public async Task UpdateSurchargeRateAsync_Given0ProductTypeId_ShouldThrowValidationException()
        {
            SurchargeRate surchargeRate = new SurchargeRate { Id = 1, ProductTypeId = 0, Rate = 1 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.Id == 1))
                .Returns(Task.FromResult(surchargeRate));

            Task result() => _surchargeRateManager.UpdateSurchargeRateAsync(surchargeRate);
            await Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public async Task UpdateSurchargeRateAsync_GivenNegativeRate_ShouldThrowValidationException()
        {
            SurchargeRate surchargeRate = new SurchargeRate { Id = 1, ProductTypeId = 1, Rate = -1 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.Id == 1))
                .Returns(Task.FromResult(surchargeRate));

            Task result() => _surchargeRateManager.UpdateSurchargeRateAsync(surchargeRate);
            await Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public async Task UpdateSurchargeRateAsync_GivenNonExistingSurchargeRateId_ShouldThrowException()
        {
            SurchargeRate surchargeRate = new SurchargeRate { Id = 1, ProductTypeId = 1, Rate = 10 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.Id == 1))
                .Returns(Task.FromResult((SurchargeRate)null));

            Task result() =>  _surchargeRateManager.UpdateSurchargeRateAsync(surchargeRate);
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task UpdateSurchargeRateAsync_GivenValidObject_ShouldUpdateAndReturnNewObject()
        {
            SurchargeRate initial = new SurchargeRate { Id = 1, ProductTypeId = 1, Rate = 10 };
            SurchargeRate updated = new SurchargeRate { Id = 1, ProductTypeId = 1, Rate = 15 };

            _surchargeRateDalMock.Setup(x => x.GetAsync(y => y.Id == 1))
                .Returns(Task.FromResult(initial));

            _surchargeRateDalMock.Setup(x => x.UpdateAsync(updated))
                .Returns(Task.FromResult(updated));

            var result = await _surchargeRateManager.UpdateSurchargeRateAsync(updated);
            Assert.Equal(initial.Id, result.Id);
            Assert.Same(updated, result);
        }

    }
}
