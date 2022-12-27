using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public ApplicationUser()
        {
            ApplicationUsersBooks = new HashSet<ApplicationUserBook>();
        }
        public ICollection<ApplicationUserBook> ApplicationUsersBooks { get; set; }

    }           
}
