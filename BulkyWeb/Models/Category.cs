using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category//Class Model
    {
        [Key]//Data Annotation That Creates Primary Key
        public int Id { get; set; }//Getter Setter Fields

        [Required(ErrorMessage = "Category name is required.")]//Data Anotation For required Data and Error Message for Asp-Display Errors

        public string Name { get; set; }

        [DisplayName("Display Order")]//It Modifies the Title to be Displayed in the Frontend

        [Range(1,100,ErrorMessage ="The Value must be Between 1 to 100")]//Range ServerValidation 
        public int CategoryOrder { get; set; }  
    }
}
