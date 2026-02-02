namespace Domain.Entities
{
    // потрібно огланути неохідність цього класу для палетного збкрігання
    public class Item
    {
        public int id;
        public string name;

        public DateTime? expirationDate;
        public int boxPerCell;
        public int? unitPerBox;

        public int Id { get { return id; } }

    }
}
