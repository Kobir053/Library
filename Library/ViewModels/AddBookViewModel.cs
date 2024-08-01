using Library.Models;

namespace Library.ViewModels
{
    public class AddBookViewModel
    {
        public BookModel Book { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
