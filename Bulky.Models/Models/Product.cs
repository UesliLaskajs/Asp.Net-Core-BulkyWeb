using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name ="List Price")]
        [Range(1,1000)]
        [ValidateNever]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = " Price For 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = " Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = " Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }


        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]

        [ValidateNever]
        public Category category { get; set; }


        [StringLength(255, ErrorMessage = "Image URL cannot be longer than 255 characters.")]
        [ValidateNever]  // Keeps validation from occurring here as well
        public string? image { get; set; } = "Deafult";

    }
}

