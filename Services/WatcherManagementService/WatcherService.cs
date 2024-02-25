using DirWatcher.Data;
using DirWatcher.Models;

namespace DirWatcher.Services.WatcherManagementService
{
    public class WatcherService : IWatcherService
    {
        private readonly ILogger<WatcherService> _logger;
        private readonly IDirWatcherBgService _dirWatcherBgService;
        private readonly IDirWatcherRepository _dirWatcherRepository;

        public WatcherService(
            ILogger<WatcherService> logger,
            IDirWatcherBgService dirWatcherBgService,
            IDirWatcherRepository dirWatcherRepository)
        {
            _logger = logger;
            _dirWatcherBgService = dirWatcherBgService;
            _dirWatcherRepository = dirWatcherRepository;
        }

        public async Task<TaskDetail> GetTaskDetailsAsync()
        {
            return await _dirWatcherBgService.GetTaskDetailsAsync();
        }

        public async Task UpdateConfig(BgConfig updatedBgConfig)
        {
            await _dirWatcherRepository.ModifyBgConfigAsync(updatedBgConfig);
            await _dirWatcherRepository.SaveChangesAsync();

            await _dirWatcherBgService.UpdateConfig(updatedBgConfig);
        }

        public async Task AddTask(TaskDetail newTask)
        {
            newTask.TaskNo = (await _dirWatcherRepository.GetAsync()).Count + 1;
            await _dirWatcherRepository.CreateAsync(newTask);
        }
    }
}
