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

        /// <summary>
        /// Retrieves an book list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /authors/3/books?pageNumber=1&pageSize=10&order=id;asc&filter=isbn_=AA
        ///
        /// </remarks>
        /// <response code="20O">Book list</response>
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

        /// <summary>
        /// Retrieves a specific book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /authors/3/books/3
        ///
        /// </remarks>
        /// <response code="200">If there is a record</response>
        /// <response code="404">If the item does not exist</response>
        [HttpGet("{id}")]
        public ActionResult<BookDto> Get(int authorId, int id)
        {
            var book = _bookLibrary.Books.FirstOrDefault(p => p.AuthorId == authorId && p.Id == id);
            if (book is null) return NotFound();

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        /// <summary>
        /// Creates a Book.
        /// </summary>
        /// <param name="BookDto"></param>
        /// <returns>A newly created Book</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /authors/3/books
        ///     {
        ///         "isbn": "AA-11-23-1",
        ///         "name": ".Net Core 3.1",
        ///         "authorId": 3
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Record was created</response>
        [HttpPost]
        public ActionResult Post(int authorId, BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _bookLibrary.Books.Add(book);
            _bookLibrary.SaveChanges();
            return CreatedAtAction("Get", new { id = book.Id, authorId = authorId }, book);
        }

        /// <summary>
        /// Updates a Book.
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <param name="BookDto">Book</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /authors/3/authors
        ///     {
        ///         "id": 3,
        ///         "isbn": "AA-11-23-1",
        ///         "name": ".Net Core 5",
        ///         "authorId": 3
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Record was updated</response>
        /// <response code="201">Returns the newly created item</response>
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

         /// <summary>
        /// Partially update a Book.
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <param name="patchDocument">Field to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /authors/3/books/3
        ///     [
        ///       {
        ///          "op": "replace",
        ///          "path": "/name",
        ///          "value": ".Net Core 3"
	    ///       }
        ///     ]
        ///
        /// </remarks>
        /// <response code="200">Record was updated</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the item does not exist</response>
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

        /// <summary>
        /// Delete a Book.
        /// </summary>
        /// <param name="authorId">Author Id</param>
        /// <param name="id">Book Id</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /authors/3/books/3
        ///
        /// </remarks>
        /// <response code="204">Record  was deleted</response>
        /// <response code="404">If the item does not exist</response>
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