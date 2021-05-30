using KanbanBoard.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KanbanBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var services = host.Services.CreateScope())
            {
                var dbContext = services.ServiceProvider.GetRequiredService<AppDbContext>();

                var userMgr = services.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                dbContext.Database.Migrate();

                var adminClaim = new Claim("Role", "Admin");
                var managerClaim = new Claim("Role", "Manager");
                var userClaim = new Claim("Role", "User");

                if(!dbContext.Users.Any(u=> u.UserName=="admin"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "mvionut99@gmail.com"
                    };

                    var resul = userMgr.CreateAsync(adminUser, "admin").GetAwaiter().GetResult();

                    userMgr.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                    userMgr.AddClaimAsync(adminUser, managerClaim).GetAwaiter().GetResult();
                    userMgr.AddClaimAsync(adminUser, userClaim).GetAwaiter().GetResult();

                }
                else
                {
                    var adminUser = userMgr.FindByNameAsync("admin").GetAwaiter().GetResult();
                    
                    if(!userMgr.GetClaimsAsync(adminUser).GetAwaiter().GetResult().Any())
                    {
                        userMgr.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                        userMgr.AddClaimAsync(adminUser, managerClaim).GetAwaiter().GetResult();
                        userMgr.AddClaimAsync(adminUser, userClaim).GetAwaiter().GetResult();
                    }
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
