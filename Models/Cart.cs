using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookSite.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem (Book newBook, int quantity)
        {
            //check if book is already in cart
            CartLine Line = Lines
                .Where(b => b.book.BookID == newBook.BookID)
                .FirstOrDefault();

            //if not create new line
            if(Line == null)
            {
                Lines.Add(new CartLine
                {
                    book = newBook,
                    quantity = quantity
                });
            }
            //add quantity to existing line's quantity
            else
            {
                Line.quantity += quantity;
            }
        }

        public virtual void RemoveLine(Book selectedBook) =>
            Lines.RemoveAll(x => x.book.BookID == selectedBook.BookID);

        public virtual void Clear() => Lines.Clear();

        public decimal ComputeTotalSum() => (decimal)Lines.Sum(e => e.book.Price * e.quantity);

        public class CartLine
        {
            public int CartLineId { get; set; }
            public Book book { get; set; }
            public int quantity { get; set; }
        }
    }
}
