using GameHub.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GameHubDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("GameHub")));

            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<GameHubDbContext>();


        }
    }
}
