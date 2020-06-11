using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TweetBook.Data;

namespace Tweetbook.Data
{
    public static class MigrationManager
    {
        public static async Task<IHost> MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            await appContext.Database.MigrateAsync();

            return host;
        }

        public static async Task<IHost> CreateRoles(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!await roleManager.RoleExistsAsync("Admin")) {
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }
            
            if(!await roleManager.RoleExistsAsync("Poster")) {
                var adminRole = new IdentityRole("Poster");
                await roleManager.CreateAsync(adminRole);
            }

            return host;
        }
    }
}
