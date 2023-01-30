using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Models;
using Sample.Api.Models.Dto;
using Sample.Api.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Fop;
using Fop.FopExpression;

namespace Sample.Api.Controllers
{
    [Route("api/v1/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IMapper _mapper { get; }
        private readonly ILogger<AuthorController> _logger;
        private readonly BookLibraryContext _bookLibrary;
        public AuthorController(BookLibraryContext bookLibrary, IMapper mapper, ILogger<AuthorController> logger)
        {
            _bookLibrary = bookLibrary;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves an author list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /authors?pageNumber=1&pageSize=10&order=id;asc&filter=firstName_=V
        ///
        /// </remarks>
        /// <response code="20O">Author list</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FopQuery request)
        {
            var fopRequest = FopExpressionBuilder<Author>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (filteredData, totalCount) = _bookLibrary.Authors.ApplyFop(fopRequest);
            
            _logger.LogInformation(fopRequest.OrderBy);

            var data = _mapper.Map<List<AuthorDto>>(filteredData);
            return Ok(new
            {
                 totalCount,
                 items = data
            });
        }

        /// <summary>
        /// Retrieves a specific author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /authors/3
        ///
        /// </remarks>
        /// <response code="200">If there is a record</response>
        /// <response code="404">If the item does not exist</response>
        [HttpGet("{id}")]
        public ActionResult<AuthorDto> Get(int id)
        {
            var author = _bookLibrary.Authors.FirstOrDefault(p => p.Id == id);
            if (author is null) return NotFound();
            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        /// <summary>
        /// Creates an Author.
        /// </summary>
        /// <param name="authorDto"></param>
        /// <returns>A newly created Author</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /authors
        ///     {
        ///        "firstName": "First Name #1",
        ///        "lastName": "Last Name #1"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Record was created</response>
        [HttpPost]
        public ActionResult Post(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _bookLibrary.Authors.Add(author);
            _bookLibrary.SaveChanges();
            return CreatedAtAction("Get", new { id = author.Id }, authorDto);
        }

        /// <summary>
        /// Updates an Author.
        /// </summary>
        /// <param name="id">Author Id</param>
        /// <param name="authorDto">Author</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /authors
        ///     {
        ///         "id": 3,
        ///         "firstName": "Jessica",
        ///         "lastName": "YILDIRIM"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Record was updated</response>
        /// <response code="201">Returns the newly created item</response>
        [HttpPut("{id}")]
        public ActionResult Put(int Id, AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            if (!_bookLibrary.Authors.Any(x => x.Id == authorDto.Id))
            {
                _bookLibrary.Authors.Add(author);
                _bookLibrary.SaveChanges();
                return CreatedAtAction("Get", new { id = author.Id }, authorDto);
            }
            _bookLibrary.Authors.Update(author);
            _bookLibrary.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Partially update an Author.
        /// </summary>
        /// <param name="id">Author Id</param>
        /// <param name="patchDocument">Field to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /authors
        ///     [
        ///       {
        ///         "op": "replace",
        ///         "path": "/firstname",
        ///         "value": "Benjamin"
	    ///       }
        ///     ]
        ///
        /// </remarks>
        /// <response code="200">Record was updated</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the item does not exist</response>
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdate(int id, [FromBody] JsonPatchDocument<AuthorDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var existingEntry = _mapper.Map<AuthorDto>(_bookLibrary.Authors.FirstOrDefault(x => x.Id == id));
            if (existingEntry is null) return NotFound();

            patchDocument.ApplyTo(existingEntry);
            var author = _mapper.Map<Author>(existingEntry);
            _bookLibrary.ChangeTracker.Clear();
            _bookLibrary.Authors.Update(author);
            _bookLibrary.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Delete an Author.
        /// </summary>
        /// <param name="id">Author Id</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /authors/3
        ///
        /// </remarks>
        /// <response code="204">Record  was deleted</response>
        /// <response code="404">If the item does not exist</response>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var author = _bookLibrary.Authors.FirstOrDefault(p => p.Id == id);
            if (author is null) return NotFound();

            _bookLibrary.Remove(author);
            _bookLibrary.SaveChanges();
            return NoContent();
        }
    }
}