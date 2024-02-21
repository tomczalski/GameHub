using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using GameHub.Application.ApplicationUser;
using GameHub.Application.Mappings;
using GameHub.Application.Tournament.Commands.AddParticipant;
using GameHub.Application.Tournament.Commands.CreateTournament;
using GameHub.Application.Tournament.Commands.Tournament;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateTournamentCommand));

            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                var scope = provider.CreateScope();
                var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();
                cfg.AddProfile(new TournamentMappingProfile(userContext));
            }).CreateMapper()
            );

            services.AddValidatorsFromAssemblyContaining<CreateTournamentCommandValidator>().AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<AddParticipantCommandValidator>().AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddScoped<IUserContext, UserContext>();
        }
    }
}
