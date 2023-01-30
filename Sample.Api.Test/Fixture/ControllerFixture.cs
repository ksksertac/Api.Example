using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Sample.Api.Controllers;
using Sample.Api.Helpers;
using Sample.Api.Test.Mock.Entities;

namespace Sample.Api.Test.Fixture
{
    public class ControllerFixture : IDisposable
    {
        private BookLibraryContextMock dbContextMock { get; set; }
        private IMapper mapper { get; set; }
        public AuthorController authorController { get; private set; }
        public BookController bookController { get; private set; }


        public ControllerFixture()
        {   
            // mock data created 
            dbContextMock = new BookLibraryContextMock("Server=localhost;Database=BookDb;User=sa;Password=MyPass@word");


            #region Mapper settings with original profiles.

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mappingConfig.CreateMapper();

            #endregion

            // Create Controller
            authorController = new AuthorController(dbContextMock,mapper);
            bookController = new BookController(dbContextMock,mapper);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ControllerFixture()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContextMock.Dispose();
                dbContextMock = null;
                mapper = null;
                authorController = null;
                bookController = null;
            }
        }
    }
}