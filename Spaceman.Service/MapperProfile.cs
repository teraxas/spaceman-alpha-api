using AutoMapper;
using Spaceman.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Player, PlayerDTO>();
        }
    }
}
