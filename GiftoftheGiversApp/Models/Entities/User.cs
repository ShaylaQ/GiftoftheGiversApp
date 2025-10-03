
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiversApp.Models.Entities
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }


    }
}
