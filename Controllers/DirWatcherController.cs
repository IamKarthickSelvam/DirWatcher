using AutoMapper;
using DirWatcher.Data;
using DirWatcher.DTOs;
using DirWatcher.Models;
using DirWatcher.Services;
using DirWatcher.Services.WatcherManagementService;
using Microsoft.AspNetCore.Mvc;

namespace DirWatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirWatcherController : ControllerBase
    {
        private readonly ILogger<DirWatcherController> _logger;
        private readonly DirWatcherBgService _service;
        private readonly IDirWatcherRepository _dirWatcherRepository;
        private readonly IWatcherService _watcherService;
        private readonly IMapper _mapper;

        public record PeriodicBackgroundServiceState(bool IsEnabled);

        public DirWatcherController(
            ILogger<DirWatcherController> logger, 
            DirWatcherBgService service,
            IDirWatcherRepository dirWatcherRepository,
            IWatcherService watcherService,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _dirWatcherRepository = dirWatcherRepository;
            _watcherService = watcherService;
            _mapper = mapper;
        }

        [HttpGet("GetBgConfig")]
        public async Task<ActionResult<BgConfig>> GetBgConfig()
        {
            try
            {
                return await _dirWatcherRepository.GetBgConfigAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetTaskDetails")]
        public async Task<ActionResult<TaskDetail>> GetTaskDetails()
        {
            try
            {
                return await _watcherService.GetTaskDetailsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("CheckCurrentTaskState")]
        public ActionResult<PeriodicBackgroundServiceState> CurrentState()
        {
            try
            {
                return new PeriodicBackgroundServiceState(_service.IsEnabled);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("ToggleCurrentTask")]
        public IActionResult UpdateBackgroundState([FromBody] PeriodicHostedServiceState state)
        {
            try
            {
                if (state == null)
                {
                    return BadRequest("Invalid state provided");
                }

                _service.IsEnabled = state.IsEnabled;

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost(Name = "ModifyValues")]
        public async Task<ActionResult> ModifyValues([FromBody] BgConfigDto bgConfigDto)
        {
            try
            {
                var updatedBgConfig = _mapper.Map<BgConfig>(bgConfigDto);

                await _watcherService.UpdateConfig(updatedBgConfig);

                return StatusCode(200, updatedBgConfig);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error: {ex.Message} ");
                return StatusCode(500);
            }
        }

        [HttpPost("AddTask")]
        public async Task<ActionResult<TaskDetailDto>> AddTask([FromBody] TaskDetailDto taskDetailDto)
        {
            try
            {
                var taskDetail = _mapper.Map<TaskDetail>(taskDetailDto);
                await _dirWatcherRepository.CreateAsync(taskDetail);
                return Ok(200);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error: {ex.Message} ");
                return StatusCode(500);
            }
        }
    }
}
