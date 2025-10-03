using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GiftoftheGiversApp.Models.Entities
{
    [Table("Volunteers")]
    public class Volunteer 
    {
        [Key]
        public int VolunteerId { get; set; }

        [Required]
        public string UserId {  get; set; }

        public string TaskType {  get; set; }

        public DateTime Availabilty { get; set; }


    }
}
