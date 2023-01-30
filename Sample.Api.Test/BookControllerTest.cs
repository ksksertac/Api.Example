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
    public class BookControllerTest : IClassFixture<ControllerFixture>
    {
        BookController _controller;
        BookLibraryContext context;

        public BookControllerTest(ControllerFixture fixture)
        {
            _controller = fixture.bookController;
        }


        [Theory]
        [InlineData(1)]
        public void Get_WithoutParam_ThenOkObjectResult_Test(int authorId)
        {
            FopQuery request = new FopQuery();
            request.Order = "id;asc";
            request.PageNumber = 1;
            request.PageSize = 10;

            var result = _controller.GetList(authorId,request).Result as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData(1,-2)]
        public void GetBook_WithNonBook_ThenNotFoundResult_Test(int authorId,int id)
        {
            var result = _controller.Get(authorId,id) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Theory]
        [InlineData(1,1)]
        public void GetBook_WithTestData_ThenOkObjectResult_Test(int authorId,int id)
        {
            var result = _controller.Get(authorId,id) as OkObjectResult;
            
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<BookDto>(result.Value);
        }

        [Theory]
        [ClassData(typeof(BookTheoryData))]
        public void AddBookWithTestData_ThenCreatedAtActionResult_Test(BookDto dto)
        {
            var result = _controller.Post(1,new BookDto(){ Name = "Unit Test Book 1", ISBN="AA-11-19" }) as CreatedAtActionResult;
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void UpdateBookWithTestData_ThenOk_Test()
        {

            var result = _controller.Put(1,new BookDto(){ Id = 1, ISBN = "AA-11-13",Name = "Unit Test Book",AuthorId = 1 }) as OkResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData(1,-2)]
        public void Delete_WithNonBook_ThenNotFound_Test(int authorId,int id)
        {
            var result = _controller.Delete(authorId,id) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

       
       
    }
}