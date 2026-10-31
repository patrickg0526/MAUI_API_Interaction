using ItemsWebAPI.DataAccess;
using ItemsWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItemsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        // GET: api/Items - all stored items
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return new ItemData().GetItems();
        }

        // POST: api/Items - add a new item
        [HttpPost]
        public IActionResult Post([FromBody] Item item)
        {
            if (string.IsNullOrWhiteSpace(item.ItemId) ||
                string.IsNullOrWhiteSpace(item.ItemName) ||
                string.IsNullOrWhiteSpace(item.ItemDescription))
            {
                return BadRequest("Item ID, Item Name, and Item Description are all required.");
            }

            item.ItemId = item.ItemId.Trim();
            item.ItemName = item.ItemName.Trim();
            item.ItemDescription = item.ItemDescription.Trim();

            var data = new ItemData();
            if (data.GetItemByItemId(item.ItemId) is not null)
            {
                return Conflict($"An item with ID \"{item.ItemId}\" already exists.");
            }

            data.AddItem(item);
            return CreatedAtAction(nameof(Get), item);
        }
    }
}
