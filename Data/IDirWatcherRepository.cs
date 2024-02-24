using DirWatcher.Models;
using Microsoft.AspNetCore.Mvc;

namespace DirWatcher.Data
{
    public interface IDirWatcherRepository
    {
        Task<bool> SaveChangesAsync();

        Task<BgConfig> GetBgConfigAsync();

        Task<ActionResult> ModifyBgConfigAsync(BgConfig updatedBgConfig);

        Task<List<TaskDetail>> GetAsync();

        Task<TaskDetail> GetByTaskId(int taskId);

        Task CreateAsync(TaskDetail taskDetail);

        Task UpdateAsync(int taskNo, TaskDetail updatedTaskDetail);
    }
}
