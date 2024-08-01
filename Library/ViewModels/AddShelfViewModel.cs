using Library.Models;

namespace Library.ViewModels
{
    public class AddShelfViewModel
    {
        public ShelfModel Shelf { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
