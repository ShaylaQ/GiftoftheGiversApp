using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiversApp.ViewModels
{
    public class IncidentViewModel 
    {
        public int IncidentId { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Incident is required")]
        public string Incident { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

    }
}
