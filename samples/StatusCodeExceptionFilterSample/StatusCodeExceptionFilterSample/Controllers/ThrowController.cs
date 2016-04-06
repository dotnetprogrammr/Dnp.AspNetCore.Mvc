using Microsoft.AspNet.Mvc;

namespace StatusCodeExceptionFilterSample
{
    public class ThrowController : Controller
    {
        [HttpGet]
        [Route("/throw-bad-request")]
        public IActionResult GetThrowBadRequestAsync()
        {
            throw new BadRequestException();
        }

        [HttpGet]
        [Route("/throw-not-found")]
        public IActionResult GetThrowNotFoundAsync()
        {
            throw new NotFoundException();
        }
    }
}
