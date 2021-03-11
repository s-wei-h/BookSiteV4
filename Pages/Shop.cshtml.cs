using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSite.Infrastructure;
using BookSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookSite.Pages
{
    public class ShopModel : PageModel
    {
        private IBookRepository repository;

        public ShopModel (IBookRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public Cart cart { get; set;}
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }
        public IActionResult OnPost(long BookID, string returnUrl)
        {
            Book book = repository.BookShelf
                .FirstOrDefault(p => p.BookID == BookID);
            cart.AddItem(book, 1);
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long BookID, string returnUrl)
        {
            cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Book book = repository.BookShelf.FirstOrDefault(p => p.BookID == BookID);
            cart.RemoveLine(book);

            HttpContext.Session.SetJson("cart", cart);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
