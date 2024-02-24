using DirWatcher.Models;

namespace DirWatcher.Services
{
    public interface ITaskManagementService
    {
        Task<TaskDetail> GetRunningTaskDetailsAsync();
    }
}
