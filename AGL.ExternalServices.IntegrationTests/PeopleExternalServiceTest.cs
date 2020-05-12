using System;
using Xunit;
using AGL.ExternalServices;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AGL.Dto;
using System.Linq;
using AGL.Library;

namespace AGL.ExternalServices.IntegrationTests
{
    public class PeopleExternalServiceTest
    {
        public PeopleExternalService _sut;

        /// <summary>
        /// This integration test will work with the correct URL (supplied in appsettings.json).
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_GetPeople_ExpectSuccess()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            
            _sut = new PeopleExternalService(config);

            //Act
            var peopleResponse = await _sut.GetPeople();

            //Assert
            Assert.NotNull(peopleResponse);
            Assert.True(peopleResponse.ResponseStatus == ResponseStatusEnum.Success);
            Assert.False(peopleResponse.Errors.Any());

            var people = peopleResponse.Data;

            Assert.NotNull(people);
            Assert.True(people.Any());
            
            //additional testing not required. Data may change over time and influence test results.
        }

        /// <summary>
        /// This integration test tests the url ht:\\www.google.com (supplied in appsettings-fail-url.json)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestUrl_GetPeople_ExpectFailure()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings-fail-url.json")
                    .Build();

            _sut = new PeopleExternalService(config);

            //Act
            var peopleResponse = await _sut.GetPeople();

            //Assert
            Assert.NotNull(peopleResponse);
            Assert.True(peopleResponse.ResponseStatus == ResponseStatusEnum.Failure);
            Assert.True(peopleResponse.Errors.Any());
            Assert.True(peopleResponse.Errors.Count() == 1);
            Assert.True(peopleResponse.Errors.FirstOrDefault() == ErrorMessages.CannotConnectToServer_01);

            var people = peopleResponse.Data;

            Assert.Null(people);
        }

        /// <summary>
        /// This integration test tests deserialization of PeopleDto with www.google.com (supplied in appsettings-fail-deserialization.json)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestDeserialization_GetPeople_ExpectFailure()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings-fail-deserialization.json")
                    .Build();

            _sut = new PeopleExternalService(config);

            //Act
            var peopleResponse = await _sut.GetPeople();

            //Assert
            Assert.NotNull(peopleResponse);
            Assert.True(peopleResponse.ResponseStatus == ResponseStatusEnum.Failure);
            Assert.True(peopleResponse.Errors.Any());
            Assert.True(peopleResponse.Errors.Count() == 1);
            Assert.True(peopleResponse.Errors.FirstOrDefault() == ErrorMessages.CannotDeserializePeople_02);

            var people = peopleResponse.Data;

            Assert.Null(people);
        }
    }
}
