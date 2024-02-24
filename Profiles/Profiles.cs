using AutoMapper;
using DirWatcher.DTOs;
using DirWatcher.Models;

namespace DirWatcher.Profiles
{
    public class Profiles : Profile
    {
        public Profiles() 
        {
            // Source --> Target
            CreateMap<BgConfigDto, BgConfig>();
            CreateMap<TaskDetailDto, TaskDetail>();
        }
    }
}
