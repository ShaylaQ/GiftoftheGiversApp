using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiversApp.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }


    }
}
