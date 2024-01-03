using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Booky.Model
{
    public class Category
    {
        [Key] public int Id { get; set; }

        //name
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string? Name { get; set; }

        //Display order
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order Must Be Between 1-100")]
        public int DisplayOrder { get; set; }

    }
}

