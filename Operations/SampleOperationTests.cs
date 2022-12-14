using Ebo.Plugins.Integration.Domain.OperationRequests;
using Moq;
using NUnit.Framework;
using Stanleybet.API.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SampleIntegration.Operations
{
    [TestFixture]
    public class SampleOperationTests
    {



        [SetUp]
        public void SetUp()
        {


        }

        private SampleOperation CreateSampleOperation()
        {
            var apiConfigurationMock = new Mock<IConfiguration>();
            apiConfigurationMock.SetupGet(x => x[Constants.PredefinedList]).Returns("Malta, Macedonia, Yugoslavia, Usa, Germany, France, Macedonia, Skopje");
            var sampleOperation = new SampleOperation();
            sampleOperation.ApiConfiguration = apiConfigurationMock.Object;
            return sampleOperation;
        }

        [Test]
        public async Task Operation_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            
            var sampleOperation = this.CreateSampleOperation();
            IOperationRequest operationRequest = new OperationRequest() {
                ApiPropertyBag = new Dictionary<string, object> { { "input", @"['Macedonia']" } }
            };

            // Act
            var result = await sampleOperation.Operation(
                operationRequest);

            // Assert
            Assert.NotNull(result);
        }
    }
}
