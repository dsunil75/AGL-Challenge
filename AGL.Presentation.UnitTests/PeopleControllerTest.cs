using AGL.BusinessLogic;
using AGL.Dto;
using AGL.Dto.ViewDto;
using AGL.Library;
using AGL.Presentation.Controllers;
using AGL.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AGL.Presentation.UnitTests
{
    public class PeopleControllerTest
    {
        private PeopleController _sut { get; set; }
        private Mock<IPeopleBusinessLogic> _mockPeopleBusinessLogic { get; set; } = new Mock<IPeopleBusinessLogic>();

        /// <summary>
        /// Gets no information from the lower layer, but does not error.
        /// </summary>
        [Fact]
        public async Task Test_GetPeople_NoData_ExpectSuccess()
        {
            //arrange
            _mockPeopleBusinessLogic.Setup(p => p.GetPeople())
                                    .Returns(Task.Run(() => new Response<List<GenderViewDto>>()));

            _sut = new PeopleController(_mockPeopleBusinessLogic.Object);

            //act
            var actionResult = await _sut.Index();

            //assert
            Assert.NotNull(actionResult);
            Assert.True(actionResult is ViewResult);

            var viewResult = actionResult as ViewResult;
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);
            Assert.True(viewResult.Model is List<Gender>);

            var genders = viewResult.Model as List<Gender>;
            Assert.NotNull(genders);
        }

        /// <summary>
        /// Gets the people in the correct view structure.
        /// </summary>
        [Fact]
        public async Task Test_GetPeople_Data_ExpectSuccess()
        {
            //arrange
            var expectedData = new List<GenderViewDto>
            {
                new GenderViewDto
                {
                    Gender = GenderTypeEnum.Female,
                    Cats = new List<CatViewDto>
                    {
                        new CatViewDto
                        {
                            Name = "Waffles"
                        }
                    },
                },
                new GenderViewDto
                {
                    Gender = GenderTypeEnum.Male,
                    Cats = new List<CatViewDto>
                    {
                        new CatViewDto
                        {
                            Name = "Albert",
                        },
                        new CatViewDto
                        {
                            Name = "Helen",
                        }
                    },
                },
            };

            _mockPeopleBusinessLogic.Setup(p => p.GetPeople())
                                    .Returns(Task.Run(() => new Response<List<GenderViewDto>> { Data = expectedData }));

            _sut = new PeopleController(_mockPeopleBusinessLogic.Object);

            //act
            var actionResult = await _sut.Index();

            //assert
            Assert.NotNull(actionResult);
            Assert.True(actionResult is ViewResult);

            var viewResult = actionResult as ViewResult;
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);
            Assert.True(viewResult.Model is List<Gender>);

            var genders = viewResult.Model as List<Gender>;
            Assert.NotNull(genders);
            Assert.True(genders.Count == 2);

            var maleOwners = genders.FirstOrDefault(p => p.GenderType == GenderTypeEnum.Male);
            Assert.NotNull(maleOwners);
            Assert.NotNull(maleOwners.Cats);
            Assert.True(maleOwners.Cats.Count == 2);
            Assert.True(maleOwners.Cats.FirstOrDefault()?.Name == "Albert"); 
            Assert.True(maleOwners.Cats[1]?.Name == "Helen"); 

            var femaleOwners = genders.FirstOrDefault(p => p.GenderType == GenderTypeEnum.Female);
            Assert.NotNull(femaleOwners);
            Assert.NotNull(femaleOwners.Cats);
            Assert.True(femaleOwners.Cats.Count == 1);
            Assert.True(femaleOwners.Cats.FirstOrDefault()?.Name == "Waffles"); 
        }
    }
}
