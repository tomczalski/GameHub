using AutoMapper;
using GameHub.Application.ApplicationUser;
using GameHub.Application.Tournament;
using GameHub.Application.Tournament.Commands.EditTournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Mappings
{
    public class TournamentMappingProfile : Profile
    {
        public TournamentMappingProfile(IUserContext userContext) 
        {
            var user = userContext.GetCurrentUser();

            CreateMap<TournamentDto, Domain.Entities.Tournament>();

            CreateMap<Domain.Entities.Tournament, TournamentDto>()
                .ForMember(dto => dto.IsEditable, opt => opt.MapFrom(src => user != null && src.CreatedById == user.Id));

            CreateMap<TournamentDto, EditTournamentCommand>();
                
            
            
        }
    }
}
