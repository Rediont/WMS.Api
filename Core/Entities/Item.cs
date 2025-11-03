namespace Core.Entities
{
    public class Item
    {
        private int id;
        public readonly string name;

        public DateTime? expiration_date;
        public int box_per_cell;
        public int? unit_per_box;

        public int Id { get { return id; } }

    }
}
