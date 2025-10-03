using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GiftoftheGiversApp.Models.Entities
{
    [Table("Volunteers")]
    public class Volunteer 
    {
        public int VolunteerId { get; set; }

        [Required]
        public string TaskType { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Availability { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }


    }
}
