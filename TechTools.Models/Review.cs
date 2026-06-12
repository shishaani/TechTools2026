using System.ComponentModel.DataAnnotations;

namespace TechTools.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int GPUId { get; set; }
        public GPU? GPU { get; set; }
        public int CPUId { get; set; }
        public CPU? CPU { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Naam moet tussen 2 en 50 tekens liggen.")]
        [Display(Name = "Naam")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Beoordeling is verplicht.")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Beoordeling moet tussen 2 en 500 tekens liggen.")]
        [Display(Name = "Beoordeling")]
        public string Comment { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Score moet tussen 0 en 5 liggen.")]
        [Display(Name = "Score")]
        public int Score { get; set; }
    }
}
