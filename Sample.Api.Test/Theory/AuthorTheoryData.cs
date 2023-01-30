using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Api.Models.Dto;
using Xunit;

namespace Sample.Api.Test.Theory
{
    public class AuthorTheoryData : TheoryData<AuthorDto>
    {
        public AuthorTheoryData()
        {
            Add(new AuthorDto()
            {
               FirstName = "Unit",
               LastName = "Test"
            });
        }
    }
}