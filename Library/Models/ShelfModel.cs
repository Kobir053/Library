namespace Library.Models
{
    public class ShelfModel
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public List<BookModel>? Books { get; set; }
        
    }
}