namespace Library.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int? ShelfId { get; set; }
        public ShelfModel? Shelf { get; set; }

    }
}