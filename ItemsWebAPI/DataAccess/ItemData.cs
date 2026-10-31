using SQLite;
using ItemsWebAPI.Models;

namespace ItemsWebAPI.DataAccess
{
    // Server-side data access for the Items table (synchronous calls inside the API)
    public class ItemData
    {
        SQLiteConnection? database;

        void Init()
        {
            if (database is not null)
                return;

            database = new SQLiteConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
            database.CreateTable<Item>();
        }

        public List<Item> GetItems()
        {
            Init();
            return database!.Table<Item>().OrderBy(i => i.ItemId).ToList();
        }

        public Item? GetItemByItemId(string itemId)
        {
            Init();
            return database!.Table<Item>().Where(i => i.ItemId == itemId).FirstOrDefault();
        }

        public int AddItem(Item item)
        {
            Init();
            item.Id = 0;
            item.DateAdded = DateTime.Now;
            return database!.Insert(item);
        }
    }
}
