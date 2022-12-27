using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models
{
    public class ApplicationUserBook
    {
        [ForeignKey(nameof(ApplicationUser))]
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }


        [ForeignKey(nameof(Book))]
        [Required]
        public int BookId  { get; set; }
        public Book Book { get; set; }
    }
}
