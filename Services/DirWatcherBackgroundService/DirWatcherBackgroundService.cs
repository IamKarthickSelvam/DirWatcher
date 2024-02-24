using DirWatcher.Data;
using DirWatcher.Models;

namespace DirWatcher.Services.DirWatcherBackgroundService
{
    public class DirWatcherBackgroundService : BackgroundService, IDirWatcherBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Timer _timer;
        private Task<BgConfig> _directoryConfig;
        private string[] _previousFiles;
        private DateTime _lastCheckTime;
        private int _count;
        private int _newTaskId;

        public DirWatcherBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RepositoryScope();

            _previousFiles = Directory.GetFiles(_directoryConfig.Result.Directory);
            _timer = new Timer(ScheduledDirectoryCheck, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            _lastCheckTime = DateTime.MinValue;
            _count = 0;
            _newTaskId = 0;
        }

        public void RepositoryScope()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IDirWatcherRepository>();

                _directoryConfig = repository.GetBgConfigAsync();
                _newTaskId = repository.GetAsync().Result.Count + 1;

                TaskDetail taskDetail = new()
                {
                    TaskNo = _newTaskId,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.MinValue,
                    Status = "Inprogress",
                };
                repository.CreateAsync(taskDetail);
            }
        }

        public void RepositoryScope(TaskDetail details)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IDirWatcherRepository>();
                repository.UpdateAsync(_newTaskId, details);
            }
        }

        public void RepositoryScope(TaskDetail details, bool shutdown)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IDirWatcherRepository>();
                if (shutdown)
                {
                    details.Status = "Success";
                    repository.UpdateAsync(_newTaskId, details);
                }
            }
        }

        private void ScheduledDirectoryCheck(object state)
        {
            if (DateTime.Now - _lastCheckTime >= TimeSpan.FromSeconds(30))
            {
                DirectoryCheck();
            }
        }

        public async Task<TaskDetail> GetTaskDetailsAsync()
        {
            var (addedFiles, deletedFiles, currentFiles) = DirectoryCheck();

            // Return task details
            TaskDetail details = new TaskDetail
            {
                Status = "InProgress",
                CurrentFiles = currentFiles,
                AddedFiles = addedFiles,
                DeletedFiles = deletedFiles
            };

            return details;
        }

        private (List<string>, List<string>, List<string>) DirectoryCheck()
        {
            List<string> addedFiles = [];
            List<string> deletedFiles = [];

            string[] currentFilesArray = Directory.GetFiles(_directoryConfig.Result.Directory, "*.txt");
            List<string> currentFiles = new(currentFilesArray);

            addedFiles = currentFiles.Except(_previousFiles).ToList();

            deletedFiles = _previousFiles.Except(currentFiles).ToList();

            _lastCheckTime = DateTime.Now;

            _previousFiles = currentFilesArray;

            foreach (string file in currentFiles)
            {
                string content = File.ReadAllText(file);
                var index = 0;
                while ((index = content.IndexOf(_directoryConfig.Result.MagicString, index)) != -1)
                {
                    // Increment count and move index forward to search for next occurrence
                    _count++;
                    index += _directoryConfig.Result.MagicString.Length;
                }
            }

            TaskDetail details = new TaskDetail
            {
                Status = "InProgress",
                CurrentFiles = currentFiles,
                AddedFiles = addedFiles,
                DeletedFiles = deletedFiles
            };

            RepositoryScope(details);

            return (addedFiles, deletedFiles, currentFiles);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Console.WriteLine("--> Background service is running");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
