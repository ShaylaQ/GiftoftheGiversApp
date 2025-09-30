
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiversApp.Models.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

     
    }
}
