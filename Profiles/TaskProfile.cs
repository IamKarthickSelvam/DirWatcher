using AutoMapper;
using DirWatcher.DTOs;
using DirWatcher.Models;

namespace DirWatcher.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile() 
        {
            // Source --> Target
            CreateMap<BgConfigDto, BgConfig>();
            CreateMap<TaskDetailDto, TaskDetail>();
        }
    }
}
