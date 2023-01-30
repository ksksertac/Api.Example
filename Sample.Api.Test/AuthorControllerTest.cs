using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using Sample.Api.Controllers;
using Sample.Api.Models.Dto;
using Fop;
using Sample.Api.Models;
using Sample.Api.Test.Fixture;
using Sample.Api.Test.Theory;
using Newtonsoft.Json;

namespace Sample.Api.Test
{
    public class AuthorControllerTest : IClassFixture<ControllerFixture>
    {
        AuthorController _controller;
        BookLibraryContext context;

        public AuthorControllerTest(ControllerFixture fixture)
        {
            _controller = fixture.authorController;
        }


        [Fact]
        public void Get_WithoutParam_ThenOkObjectResult_Test()
        {
            FopQuery request = new FopQuery();
            request.Order = "id;asc";
            request.PageNumber = 1;
            request.PageSize = 10;

            var result = _controller.Get(request).Result as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData(-2)]
        public void GetAuthor_WithNonAuthor_ThenNotFoundResult_Test(int id)
        {
            var result = _controller.Get(id) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void GetAuthor_WithTestData_ThenOkObjectResult_Test(int id)
        {
            var result = _controller.Get(id) as OkObjectResult;
            
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<AuthorDto>(result.Value);
        }

        [Theory]
        [ClassData(typeof(AuthorTheoryData))]
        public void AddAuthorWithTestData_ThenCreatedAtActionResult_Test(AuthorDto dto)
        {
            var result = _controller.Post(dto) as CreatedAtActionResult;
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void UpdateAuthorWithTestData_ThenOk_Test()
        {
            var result = _controller.Put(1,new AuthorDto(){ FirstName = "Victor",LastName = "Hugo" }) as OkResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData(-2)]
        public void Delete_WithNonAuthor_ThenNotFound_Test(int id)
        {
            var result = _controller.Delete(id) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void Delete_WithTestData_ThenNoContent_Test(int id)
        {
            var result = _controller.Delete(id) as NoContentResult;
            Assert.Equal(204, result.StatusCode);
        }
       
    }
}