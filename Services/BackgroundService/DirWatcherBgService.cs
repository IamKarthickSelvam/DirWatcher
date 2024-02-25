
using DirWatcher.Data;
using DirWatcher.Models;
using DirWatcher.Services.WatcherManagementService;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DirWatcher.Services
{
    public class DirWatcherBgService : BackgroundService, IDirWatcherBgService
    {
        private Task? _executeTask;
        private CancellationTokenSource? _stoppingCts;

        private int _executionCount = 0;
        private readonly ILogger<DirWatcherBgService> _logger;
        private readonly IServiceScopeFactory _factory;
        private static Task<BgConfig> _directoryConfig;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        private readonly Stopwatch _stopwatch = new();
        private static DateTime _lastCheckTime;
        private static string[] _previousFiles;
        private List<string> _addedFiles = [];
        private List<string> _deletedFiles = [];
        private int _newTaskId;

        public bool IsEnabled { get; set; } = true;
        public bool IsRunning { get; set; }

        public DirWatcherBgService(
            ILogger<DirWatcherBgService> logger,
            IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
            _lastCheckTime = DateTime.MinValue;

            if (IsEnabled)
                _stopwatch.Start();
        }

        // Maybe use StartAsync
        //public async Task Init()
        //{
        //    if (IsEnabled)
        //        _stopwatch.Start();

        //    await CreateTaskAsync();
        //}

        //public override async Task<object> StartAsync(CancellationToken cancellationToken)
        //{
        //    if (IsEnabled)
        //        _stopwatch.Start();


        //    _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        //    // Store the task we're executing
        //    _executeTask = ExecuteAsync(_stoppingCts.Token);

        //    // If the task is completed then return it, this will bubble cancellation and failure to the caller
        //    if (_executeTask.IsCompleted)
        //    {
        //        return _executeTask;
        //    }

        //    // Otherwise it's running
        //    return Task.CompletedTask;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(_period);
            _logger.LogInformation($"Task has started");

            if (IsEnabled)
            {
                await CreateTaskAsync();
                while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken) &&
                IsEnabled)
                {
                    try
                    {
                        var (addedFiles, deletedFiles, currentFiles, magicCount) = await Watch();

                        TaskDetail taskDetails = new TaskDetail
                        {
                            MagicCount = magicCount,
                            Status = "InProgress",
                            CurrentFiles = currentFiles,
                            AddedFiles = addedFiles,
                            DeletedFiles = deletedFiles
                        };

                        // USE REPOSITORY LOGIC HERE
                        string status = "In Progress";
                        UpdateTaskAsync(taskDetails, status);

                        _executionCount++;
                        _logger.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");
                        _logger.LogInformation($"Time elapsed: {_stopwatch.Elapsed}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"Failed to execute PeriodicBackgroundService with exception message {ex.Message}");
                    }
                }
            }
            else
            {
                _logger.LogInformation($"Task has been stopped");
                _stopwatch.Stop();
            }
        }

        public override async Task<Task> StopAsync(CancellationToken cancellationToken)
        {
            await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
            DirWatcherRepository dirWatcherRepository = asyncScope.ServiceProvider.GetRequiredService<DirWatcherRepository>();

            var task = dirWatcherRepository.GetByTaskId(_newTaskId);

            string status = "Success";

            UpdateTaskAsync(task.Result, status);
            return base.StopAsync(cancellationToken);
        }

        private async Task<(List<string>, List<string>, List<string>, int)> Watch()
        {
            var magicCount = 0;
            List<string> addedFiles = [];
            List<string> deletedFiles = [];

            string[] currentFilesArray = Directory.GetFiles(_directoryConfig.Result.Directory, "*.txt").Select(file => Path.GetFileName(file)).ToArray();

            List<string> currentFiles = new(currentFilesArray);

            addedFiles = currentFilesArray.Except(_previousFiles).ToList();
            //_addedFiles.AddRange(addedFiles);

            deletedFiles = _previousFiles.Except(currentFilesArray).ToList();
            //_deletedFiles.AddRange(deletedFiles);

            _lastCheckTime = DateTime.Now;

            //_previousFiles = currentFilesArray;

            foreach (string file in currentFiles)
            {
                string content = File.ReadAllText(_directoryConfig.Result.Directory + "\\" + file);
                var index = 0;
                while ((index = content.IndexOf("awan", index)) != -1)
                {
                    // Increment count and move index forward to search for next occurrence
                    magicCount++;
                    index += "awan".Length;
                }
            }

            return (addedFiles, deletedFiles, currentFiles, magicCount);
            //_logger.LogInformation($"count: {count}");
            //await Console.Out.WriteLineAsync($"count: {count}");
            //foreach (var currentFile in currentFiles)
            //{
            //    //_logger.LogInformation($"currentFiles: {currentFile}");
            //    await Console.Out.WriteLineAsync($"currentFiles: {currentFile}");
            //}
            //foreach (var addedFile in addedFiles)
            //{
            //    //_logger.LogInformation($"addedFiles: {addedFile}");
            //    await Console.Out.WriteLineAsync($"addedFiles 1: {addedFile}");
            //}
            //foreach (var _addedFile in _addedFiles)
            //{
            //    //_logger.LogInformation($"addedFiles: {_addedFile}");
            //    await Console.Out.WriteLineAsync($"addedFiles 2: {_addedFile}");
            //}
            //foreach (var deletedFile in deletedFiles)
            //{
            //    //_logger.LogInformation($"deletedFiles: {deletedFile}");
            //    await Console.Out.WriteLineAsync($"deletedFiles 1: {deletedFile}");
            //}
            //foreach (var _deletedFile in _deletedFiles)
            //{
            //    //_logger.LogInformation($"deletedFiles: {_deletedFile}");
            //    await Console.Out.WriteLineAsync($"deletedFiles 2: {_deletedFile}");
            //}
        }

        public async Task UpdateConfig(BgConfig updatedBgConfig)
        {
            _directoryConfig.Result.Directory = updatedBgConfig.Directory;
            _directoryConfig.Result.Interval = updatedBgConfig.Interval;
            _directoryConfig.Result.MagicString = updatedBgConfig.MagicString;
        }

        public async Task<TaskDetail> GetTaskDetailsAsync()
        {
            var (addedFiles, deletedFiles, currentFiles, magicCount) = await Watch();

            TaskDetail taskDetails = new TaskDetail
            {
                MagicCount = magicCount,
                Status = "InProgress",
                CurrentFiles = currentFiles,
                AddedFiles = addedFiles,
                DeletedFiles = deletedFiles
            };

            return taskDetails;
        }

        private async Task CreateTaskAsync()
        {
            await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
            DirWatcherRepository dirWatcherRepository = asyncScope.ServiceProvider.GetRequiredService<DirWatcherRepository>();

            _directoryConfig = dirWatcherRepository.GetBgConfigAsync();
            _previousFiles = Directory.GetFiles(_directoryConfig.Result.Directory, "*.txt").Select(file => Path.GetFileName(file)).ToArray();
            _newTaskId = dirWatcherRepository.GetAsync().Result.Count + 1;

            TaskDetail taskDetail = new()
            {
                TaskNo = _newTaskId,
                StartTime = DateTime.Now,
                Status = "Inprogress",
            };

            await dirWatcherRepository.CreateAsync(taskDetail);
            await dirWatcherRepository.SaveChangesAsync();
        }

        private async Task UpdateTaskAsync(TaskDetail taskDetail, string status)
        {
            await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
            DirWatcherRepository dirWatcherRepository = asyncScope.ServiceProvider.GetRequiredService<DirWatcherRepository>();

            switch (status)
            {
                case "Success":
                    taskDetail.EndTime = DateTime.Now;
                    taskDetail.TotalRunTime = _stopwatch.Elapsed;
                    break;
                default:
                    break;
            }

            taskDetail.Status = status;

            await dirWatcherRepository.UpdateAsync(_newTaskId, taskDetail);
            await dirWatcherRepository.SaveChangesAsync();
        }
    }
}
