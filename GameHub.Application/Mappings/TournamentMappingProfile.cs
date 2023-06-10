using AutoMapper;
using GameHub.Application.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Mappings
{
    public class TournamentMappingProfile : Profile
    {
        public TournamentMappingProfile() 
        {
            CreateMap<TournamentDto, Domain.Entities.Tournament>();
            CreateMap<Domain.Entities.Tournament, TournamentDto>();
        }
    }
}
