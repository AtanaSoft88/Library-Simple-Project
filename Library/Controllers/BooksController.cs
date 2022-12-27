using Library.Models;
using Library.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService service;

        public BooksController(IBookService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<BookViewModel> books = await service.GetAllBooksCollection();
            return this.View(books);

        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {    
            AddBookViewModel model = new AddBookViewModel()
            {
                Categories = await service.GetAllCategories()
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            { // if not valid return the model of the form to be filled again by the user
                return this.View(model);
            }
            try
            {
                await service.CurrentBookAdd(model);
                return RedirectToAction(nameof(All));  
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong with adding Book!");
                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int bookId)
        {
            try
            {
                var userId = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                await service.AddBookToCollection(bookId, userId);
            }
            catch (Exception)
            {
                this.ViewBag.ErrorCode = "Could not add book to collection!";
                RedirectToAction("ErrorStatCode", "Home");
            }                        
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var readBooks = await service.GetReadBooks(userId);
            return View(readBooks);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            try
            {
                var userId = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                await service.RemoveBookFromCollection(bookId, userId);
            }
            catch (Exception)
            {
                this.ViewBag.ErrorCode = "Could not remove the book from collection!";
                RedirectToAction("ErrorStatCode", "Home");
            }           
            return RedirectToAction(nameof(Mine));
        }
    }
}
