using Microsoft.AspNetCore.Mvc;

namespace Library.Models
{
    public class BookViewModel
    {
        [BindProperty]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Rating { get; set; }
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
