namespace Lagerverwaltung.Models
{
    public class StockMovement
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public Article? Article { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public StockMovementType Type { get; set; }
    }

    public enum StockMovementType
    {
        In,
        Out
    }
}