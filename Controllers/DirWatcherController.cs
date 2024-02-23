using DirWatcher.Data;
using DirWatcher.Models;
using Microsoft.AspNetCore.Mvc;

namespace DirWatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirWatcherController : ControllerBase
    {
        private readonly ILogger<DirWatcherController> _logger;
        private readonly IDirWatcherRepository _dirWatcherRepository;

        public DirWatcherController(ILogger<DirWatcherController> logger, IDirWatcherRepository dirWatcherRepository)
        {
            _logger = logger;
            _dirWatcherRepository = dirWatcherRepository;
        }

        [HttpGet]
        public async ActionResult<IEnumerable<BgConfig>> GetBgConfig()
        {
            try
            {
                return await _dirWatcherRepository.GetBgConfig();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Exception occured: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
