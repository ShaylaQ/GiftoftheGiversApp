
using Microsoft.AspNetCore.Identity;

namespace GiftoftheGiversApp.Models.Entities
{
    public class User : IdentityUser
    {
        
        public string Name { get; set; }
    }
}

