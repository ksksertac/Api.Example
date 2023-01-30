using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Api.Models.Dto;
using Xunit;

namespace Sample.Api.Test.Theory
{
    public class BookTheoryData : TheoryData<BookDto>
    {
        public BookTheoryData()
        {
            Add(new BookDto()
            {
               Name = "Unit Test Book",
               ISBN = "AA-11-13",
               AuthorId = 1
            });
        }
    }
}