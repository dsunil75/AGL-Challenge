using System;
using Xunit;
using AGL.BusinessLogic;
using System.Threading.Tasks;
using AGL.ExternalServices;
using Moq;
using AGL.Library;
using System.Collections.Generic;
using AGL.Dto.ContractDto;
using AGL.Dto;
using System.Linq;

namespace AGL.BusinessLogic.UnitTests
{
    public class PeopleBusinessLogicTest
    {
        private IPeopleBusinessLogic _sut { get; set; }
        private Mock<IPeopleExternalService> _mockPeopleExternalService { get; set; } = new Mock<IPeopleExternalService>();

        /// <summary>
        /// Tests that the code can successfully return nothing.
        /// </summary>
        [Fact]
        public async Task Test_GetPeople_ExpectSuccess_NoData()
        {
            //arrange
            _mockPeopleExternalService.Setup(p => p.GetPeople())
                                      .Returns(Task.Run(() => new Response<List<PersonContractDto>>()));

            _sut = new PeopleBusinessLogic(_mockPeopleExternalService.Object);

            //act
            var actualPeople = await _sut.GetPeople();

            //assert
            Assert.Null(actualPeople.Data);
            Assert.True(actualPeople.ResponseStatus == ResponseStatusEnum.Success);
        }

        /// <summary>
        /// Tests that the code can return a failure from an underlying service.
        /// </summary>
        [Fact]
        public async Task Test_GetPeople_ExpectFailure_PeopleExternalServiceFailure()
        {
            //arrange
            _mockPeopleExternalService.Setup(p => p.GetPeople())
                                      .Returns(Task.Run(() => new Response<List<PersonContractDto>>
                                      {
                                          Errors = new List<string>
                                          {
                                              ErrorMessages.CannotConnectToServer_01
                                          }
                                      }));

            _sut = new PeopleBusinessLogic(_mockPeopleExternalService.Object);

            //act
            var actualPeople = await _sut.GetPeople();

            //assert
            Assert.Null(actualPeople.Data);
            Assert.False(actualPeople.ResponseStatus == ResponseStatusEnum.Success);
        }

        /// <summary>
        /// Tests that the code can successfully return nothing because no owners have a cat.
        /// </summary>
        [Fact]
        public async Task Test_GetPeople_ExpectSuccess_NoCats()
        {
            //arrange
            var expectedPeople = new List<PersonContractDto>
            {
                new PersonContractDto
                {
                    Age = 20,
                    Gender = GenderTypeEnum.Female,
                    Name = "Elle",
                    Pets = new List<PetContractDto>
                    {
                        new PetContractDto
                        {
                            Name = "Waffles",
                            Type = PetTypeEnum.Dog,
                        }
                    }
                },
                new PersonContractDto
                {
                    Age = 24,
                    Gender = GenderTypeEnum.Male,
                    Name = "John",
                    Pets = new List<PetContractDto>
                    {
                        new PetContractDto
                        {
                            Name = "Helen",
                            Type = PetTypeEnum.Fish,
                        },
                        new PetContractDto
                        {
                            Name = "Albert",
                            Type = PetTypeEnum.Dog,
                        }
                    }
                }
            };

            _mockPeopleExternalService.Setup(p => p.GetPeople())
                                      .Returns(Task.Run(() => new Response<List<PersonContractDto>>()));

            _sut = new PeopleBusinessLogic(_mockPeopleExternalService.Object);

            //act
            var actualPeople = await _sut.GetPeople();

            //assert
            Assert.Null(actualPeople.Data);
            Assert.True(actualPeople.ResponseStatus == ResponseStatusEnum.Success);
        }

        /// <summary>
        /// Tests that the code can successfully return cats.
        /// </summary>
        [Fact]
        public async Task Test_GetPeople_ExpectSuccess_PeopleHaveCats()
        {
            //arrange
            var expectedPeople = new List<PersonContractDto>
            {
                new PersonContractDto
                {
                    Age = 20,
                    Gender = GenderTypeEnum.Female,
                    Name = "Elle",
                    Pets = new List<PetContractDto>
                    {
                        new PetContractDto
                        {
                            Name = "Waffles",
                            Type = PetTypeEnum.Cat,
                        }
                    }
                },
                new PersonContractDto
                {
                    Age = 24,
                    Gender = GenderTypeEnum.Male,
                    Name = "John",
                    Pets = new List<PetContractDto>
                    {
                        new PetContractDto
                        {
                            Name = "Helen",
                            Type = PetTypeEnum.Cat,
                        },
                        new PetContractDto
                        {
                            Name = "Albert",
                            Type = PetTypeEnum.Cat,
                        }
                    }
                }
            };

            _mockPeopleExternalService.Setup(p => p.GetPeople())
                                      .Returns(Task.Run(() => new Response<List<PersonContractDto>> { Data = expectedPeople }));

            _sut = new PeopleBusinessLogic(_mockPeopleExternalService.Object);

            //act
            var actualPeople = await _sut.GetPeople();

            //assert
            Assert.NotNull(actualPeople.Data);
            Assert.True(actualPeople.ResponseStatus == ResponseStatusEnum.Success);

            Assert.True(actualPeople.Data.Count == 2);

            var maleOwners = actualPeople.Data.FirstOrDefault(p => p.Gender == GenderTypeEnum.Male);
            Assert.NotNull(maleOwners);
            Assert.NotNull(maleOwners.Cats);
            Assert.True(maleOwners.Cats.Count == 2);
            Assert.True(maleOwners.Cats.FirstOrDefault()?.Name == "Albert"); // This is Albert and not Helen because it's ordered alphabetically.
            Assert.True(maleOwners.Cats[1]?.Name == "Helen"); 

            var femaleOwners = actualPeople.Data.FirstOrDefault(p => p.Gender == GenderTypeEnum.Female);
            Assert.NotNull(femaleOwners);
            Assert.NotNull(femaleOwners.Cats);
            Assert.True(femaleOwners.Cats.Count == 1);
            Assert.True(femaleOwners.Cats.FirstOrDefault()?.Name == "Waffles");
        }
    }
}
