namespace Lagerverwaltung.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string ArticleNumber { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<StockMovement> StockMovements { get; set; }
            = new List<StockMovement>();
    }
}