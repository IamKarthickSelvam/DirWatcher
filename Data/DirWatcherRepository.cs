using DirWatcher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DirWatcher.Data
{
    public class DirWatcherRepository : IDirWatcherRepository
    {
        private readonly IMongoCollection<TaskDetail> _taskCollection;
        private readonly AppDbContext _context;

        public DirWatcherRepository(AppDbContext context, IOptions<TaskDatabaseSettings> taskDatabaseSettings)
        {
            var mongoClient = new MongoClient(taskDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(taskDatabaseSettings.Value.DatabaseName);
            _taskCollection = mongoDatabase.GetCollection<TaskDetail>(taskDatabaseSettings.Value.TaskCollectionName);
            _context = context;
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;

        public async Task<BgConfig> GetBgConfigAsync() => await _context.BgConfiguration.FirstOrDefaultAsync();

        public async Task<ActionResult> ModifyBgConfigAsync(BgConfig updatedBgConfig)
        {
            BgConfig bgConfig = await _context.BgConfiguration.FirstAsync() ?? throw new Exception("Configuration is missing in DB!");

            _context.Entry(bgConfig).CurrentValues.SetValues(bgConfig);

            return null;
        }

        public async Task<List<TaskDetail>> GetAsync() => await _taskCollection.Find(_ => true).ToListAsync();

        public async Task<TaskDetail> GetByTaskId(int taskId) => await _taskCollection.Find(x => x.TaskNo == taskId).FirstOrDefaultAsync();

        public async Task CreateAsync(TaskDetail taskDetail) => await _taskCollection.InsertOneAsync(taskDetail);

        public async Task UpdateAsync(int taskNo, TaskDetail updatedTaskDetail) => await _taskCollection.ReplaceOneAsync(x => x.TaskNo == taskNo, updatedTaskDetail);
    }
}
