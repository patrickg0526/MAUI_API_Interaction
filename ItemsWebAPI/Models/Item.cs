using SQLite;

namespace ItemsWebAPI.Models
{
    [Table("Items")]
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, NotNull, MaxLength(20)]
        public string ItemId { get; set; } = string.Empty;

        [NotNull, MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ItemDescription { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; }
    }
}
