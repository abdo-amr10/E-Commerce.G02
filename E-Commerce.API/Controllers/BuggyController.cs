using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presistence.Data;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BuggyController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("unauthorized")] // GET: /api/Buggy/unauthorized
        public ActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("badrequest")] // GET: /api/Buggy/badrequest
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("badrequest/{id}")] // GET: /api/Buggy/badrequest/Five
        public ActionResult GetValidationError(int id)
        {
            return Ok();
        }

        [HttpGet("servererror")] // GET: /api/Buggy/servererror
        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100);
            var ProductToReturn = product.ToString(); // Will Throw Exception [NULL Reference Exception]
            return Ok(ProductToReturn);
        }

        [HttpGet("notfound")] // GET: /api/Buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);

            if (product is null)
                return NotFound();

            return Ok(product);
        }
    }
}
