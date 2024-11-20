using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category//Class Model
    {
        [Key]//Data Annotation That Creates Primary Key
        public int Id { get; set; }//Getter Setter Fields

        [Required]//Data Anotation For required Data
        public string Name { get; set; }

        public int CategoryOrder { get; set; }  
    }
}
