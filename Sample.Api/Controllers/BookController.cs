using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Models;
using Sample.Api.Models.Dto;
using Sample.Api.Models.Entities;
using Fop;
using Microsoft.AspNetCore.JsonPatch;
using Fop.FopExpression;

namespace Sample.Api.Controllers
{
    [Route("api/v1/authors/{authorId}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IMapper _mapper { get; }
        private readonly BookLibraryContext _bookLibrary;
        public BookController(BookLibraryContext bookLibrary, IMapper mapper)
        {
            _bookLibrary = bookLibrary;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int authorId, [FromQuery] FopQuery request)
        {
            var fopRequest = FopExpressionBuilder<Book>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (filteredData, totalCount) = _bookLibrary.Books.Where(p => p.AuthorId == authorId).ApplyFop(fopRequest);

            var data = _mapper.Map<List<BookDto>>(filteredData);
            return Ok(new
            {
                totalCount,
                items = data
            });
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> Get(int authorId, int id)
        {
            var book = _bookLibrary.Books.FirstOrDefault(p => p.AuthorId == authorId && p.Id == id);
            if (book is null) return NotFound();

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        [HttpPost]
        public ActionResult Post(int authorId, BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _bookLibrary.Books.Add(book);
            _bookLibrary.SaveChanges();
            return CreatedAtAction("Get", new { id = book.Id, authorId = authorId }, book);
        }


        [HttpPut]
        public ActionResult Put(int authorId, BookDto bookDto)
        {
            var author = _bookLibrary.Authors.FirstOrDefault(p => p.Id == authorId);
            if (author is null) return NotFound();

            var book = _mapper.Map<Book>(bookDto);
            if (!_bookLibrary.Books.Any(x => x.Id == book.Id))
            {
                _bookLibrary.Books.Add(book);
                _bookLibrary.SaveChanges();
                return CreatedAtAction("Get", new { id = book.Id, authorId = authorId }, bookDto);
            }
            _bookLibrary.Books.Update(book);
            _bookLibrary.SaveChanges();
            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdate(int authorId, int id, [FromBody] JsonPatchDocument<BookDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var existingEntry = _mapper.Map<BookDto>(_bookLibrary.Books.FirstOrDefault(x => x.Id == id));
            if (existingEntry is null) return NotFound();

            patchDocument.ApplyTo(existingEntry);
            var book = _mapper.Map<Book>(existingEntry);
            _bookLibrary.ChangeTracker.Clear();
            _bookLibrary.Books.Update(book);
            _bookLibrary.SaveChanges();

            return Ok();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int authorId, int id)
        {
            if (!_bookLibrary.Authors.Any(x => x.Id == authorId)) return NotFound();

            var book = _bookLibrary.Books.FirstOrDefault(p => p.Id == id && p.AuthorId == authorId);
            if (book is null) return NotFound();

            _bookLibrary.Remove(book);
            _bookLibrary.SaveChanges();
            return NoContent();
        }
    }
}