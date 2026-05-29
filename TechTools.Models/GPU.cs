using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechTools.Models
{
    public class GPU
    {
        // id is nodig voor databank, duidt specifieke records aan, dit is de 'Primary Key'
        public int Id { get; set; }

        // brand is nodig voor databank, duidt specifieke records aan, dit is de 'Brand'
        [Required(ErrorMessage ="De brand naam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "De brand naam moet tussen 2 en 50 tekens liggen.")]
        [Display(Name = "Brand naam.")]
        public string Brand { get; set; }

        // model is nodig voor databank, duidt specifieke records aan, dit is de 'Model'
        [Required(ErrorMessage = "De model naam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "De model naam moet tussen 2 en 50 tekens liggen.")]
        [Display(Name = "Model naam.")]
        public string Model { get; set; }

        // price is nodig voor databank, duidt specifieke records aan, dit is de 'Price'
        [Required(ErrorMessage = "De prijs is verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "De prijs moet een positief getal zijn.")]
        [Display(Name = "Prijs")]
        public decimal Price { get; set; }

        // picture is nodig voor databank, duidt specifieke records aan, dit is de 'Picture'
        [Display(Name = "Foto")]
        public string? Picture { get; set; }

        // description is nodig voor databank, duidt specifieke records aan, dit is de 'Description'
        [Required(ErrorMessage = "De beschrijving is verplicht.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "De beschrijving moet tussen 10 en 500 tekens liggen.")]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }


        public GPU()
        {
                
        }

        public GPU(int id, string brand, string model, decimal price, string? picture, string description)
        {
            Id = id;
            Brand = brand;
            Model = model;
            Price = price;
            Picture = picture;
            Description = description;
        }
    }
}
