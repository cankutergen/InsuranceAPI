using AutoMapper;
using Insurance.Api.Controllers;
using Insurance.Api.Models.Insurance;
using Insurance.Api.Models.SurchargeRate;
using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Api.Tests
{
    public class SurchargeRateControllerTests
    {
        private readonly Mock<ISurchargeRateService> _surchargeRateServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly SurchargeRateController _controller;

        public SurchargeRateControllerTests()
        {
            _surchargeRateServiceMock = new Mock<ISurchargeRateService>(); 
            _mapperMock = new Mock<IMapper>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _controller = new SurchargeRateController(_surchargeRateServiceMock.Object, _mapperMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task GetById_GivenValidInput_ShouldReturnOkResponse()
        {
            _surchargeRateServiceMock.Setup(x => x.GetSurchargeRateByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            _mapperMock
             .Setup(x => x.Map<SurchargeResponseModel>(It.IsAny<SurchargeRate>()))
             .Returns(new SurchargeResponseModel());

            var result = await _controller.GetById(It.IsAny<int>()) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_GivenNonExistingSurchargeId_ShouldReturnNotFoundResponse()
        {
            _surchargeRateServiceMock.Setup(x => x.GetSurchargeRateByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            _mapperMock
             .Setup(x => x.Map<SurchargeResponseModel>(It.IsAny<SurchargeRate>()))
             .Returns(new SurchargeResponseModel());

            var result = await _controller.GetById(It.IsAny<int>()) as NotFoundResult;
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_GivenInvalidInput_ShouldReturnBadRequest()
        {
            _surchargeRateServiceMock.Setup(x => x.GetSurchargeRateByIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException());

            var result = await _controller.GetById(It.IsAny<int>()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetByProductTypeId_GivenValidInput_ShouldReturnOkResponse()
        {
            _surchargeRateServiceMock.Setup(x => x.GetSurchargeRateByProductTypeIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            _mapperMock
             .Setup(x => x.Map<SurchargeResponseModel>(It.IsAny<SurchargeRate>()))
             .Returns(new SurchargeResponseModel());

            var result = await _controller.GetByProductTypeId(It.IsAny<int>()) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByProductTypeId_GivenNonExistingSurchargeId_ShouldReturnNotFoundResponse()
        {
            _surchargeRateServiceMock.Setup(x => x.GetSurchargeRateByProductTypeIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            _mapperMock
             .Setup(x => x.Map<SurchargeResponseModel>(It.IsAny<SurchargeRate>()))
             .Returns(new SurchargeResponseModel());

            var result = await _controller.GetByProductTypeId(It.IsAny<int>()) as NotFoundResult;
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByProductTypeId_GivenInvalidInput_ShouldReturnBadRequest()
        {
            _surchargeRateServiceMock.Setup(x => x.GetSurchargeRateByProductTypeIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException());

            var result = await _controller.GetByProductTypeId(It.IsAny<int>()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_GivenValidInput_ShouldReturnOkResponse()
        {
            _surchargeRateServiceMock.Setup(x => x.CreateSurchargeRateAsync(It.IsAny<SurchargeRate>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            _mapperMock
             .Setup(x => x.Map<SurchargeRate>(It.IsAny<SurchargePostRequestModel>()))
             .Returns(new SurchargeRate());

            _mapperMock
             .Setup(x => x.Map<SurchargeResponseModel>(It.IsAny<SurchargeRate>()))
             .Returns(new SurchargeResponseModel());

            var result = await _controller.Post(It.IsAny<SurchargePostRequestModel>()) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_GivenInvalidInput_ShouldReturnBadRequest()
        {
            _surchargeRateServiceMock.Setup(x => x.CreateSurchargeRateAsync(It.IsAny<SurchargeRate>()))
                .Throws(new NullReferenceException());

            _mapperMock
             .Setup(x => x.Map<SurchargeRate>(It.IsAny<SurchargePostRequestModel>()))
             .Returns(new SurchargeRate());

            var result = await _controller.Post(It.IsAny<SurchargePostRequestModel>()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_GivenValidInput_ShouldReturnOkResponse()
        {
            _surchargeRateServiceMock.Setup(x => x.UpdateSurchargeRateAsync(It.IsAny<SurchargeRate>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            _mapperMock
             .Setup(x => x.Map<SurchargeRate>(It.IsAny<SurchargePutRequestModel>()))
             .Returns(new SurchargeRate());

            _mapperMock
             .Setup(x => x.Map<SurchargeResponseModel>(It.IsAny<SurchargeRate>()))
             .Returns(new SurchargeResponseModel());

            var result = await _controller.Put(It.IsAny<SurchargePutRequestModel>()) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Put_GivenInvalidInput_ShouldReturnBadRequest()
        {
            _surchargeRateServiceMock.Setup(x => x.UpdateSurchargeRateAsync(It.IsAny<SurchargeRate>()))
                .Throws(new NullReferenceException());

            _mapperMock
             .Setup(x => x.Map<SurchargeRate>(It.IsAny<SurchargePutRequestModel>()))
             .Returns(new SurchargeRate());

            var result = await _controller.Put(It.IsAny<SurchargePutRequestModel>()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_GivenValidInput_ShouldReturnOkResponse()
        {
            var result = await _controller.Delete(It.IsAny<int>()) as OkResult;
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_GivenInvalidInput_ShouldReturnBadRequest()
        {
            _surchargeRateServiceMock.Setup(x => x.DeleteSurchargeRateByIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException());

            var result = await _controller.Delete(It.IsAny<int>()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
