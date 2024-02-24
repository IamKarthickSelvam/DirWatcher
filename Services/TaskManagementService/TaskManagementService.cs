using DirWatcher.Models;
using DirWatcher.Services.DirWatcherBackgroundService;

namespace DirWatcher.Services.TaskManagementService
{
    public class TaskManagementService : ITaskManagementService
    {
        private readonly IDirWatcherBackgroundService _dirWatcherBackgroundService;

        public TaskManagementService(IDirWatcherBackgroundService dirWatcherBackgroundService)
        {
            _dirWatcherBackgroundService = dirWatcherBackgroundService;
        }

        public async Task<TaskDetail> GetRunningTaskDetailsAsync()
        {
            return await _dirWatcherBackgroundService.GetTaskDetailsAsync();
        }

        //public async Task ToggleBackgroundService()
        //{

        //}
    }
}