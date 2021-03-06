﻿using AutoMapper;
using Kbalan.TouchType.Data.Models;
using Kbalan.TouchType.Logic.Dto;

namespace Kbalan.TouchType.Logic.Profiles
{
    public class StatisticProfile : Profile
    {
        public StatisticProfile()
        {
            CreateMap<StatisticDb, StatisticDto>().ReverseMap();
        }
    }

}
