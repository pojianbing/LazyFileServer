using Microsoft.AspNetCore.Mvc;

namespace Lazy.FileServer.Client.WebApi.Host.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private IHttpContextAccessor _httpContextAccessor;

        public FileController(ILogger<FileController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost()]
        [Route("Upload")]
        public async Task<IEnumerable<string>> UploadAsync()
        {
            var client = new FileServerClient("http://localhost:5001", "1", "123456");
            return await client.UploadAsync(_httpContextAccessor.HttpContext);
        }
    }
}