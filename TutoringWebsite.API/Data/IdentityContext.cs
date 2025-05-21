using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutoringWebsite.API.Models;


namespace TutoringWebsite.API.Data
{
    public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<User>(options)
    {

    }
}