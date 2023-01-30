using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Api.Models;

namespace Sample.Api.Test.Mock.Entities
{
    public partial class BookLibraryContextMock : BookLibraryContext
    {
        private string _conn = "";
        public BookLibraryContextMock(string conn) : base(conn)
        {
            _conn = conn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_conn);
            }
        }
       
    }
}