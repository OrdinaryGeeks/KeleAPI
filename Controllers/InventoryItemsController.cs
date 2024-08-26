using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Models;

namespace ShoppingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase
    {
        private readonly DBContext _context;

        public InventoryItemsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/InventoryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItem()
        {
            return await _context.InventoryItem.ToListAsync();
        }

        // GET: api/InventoryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
        {
            var InventoryItem = await _context.InventoryItem.FindAsync(id);

            if (InventoryItem == null)
            {
                return NotFound();
            }

            return InventoryItem;
        }

        // PUT: api/InventoryItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryItem(int id, InventoryItem InventoryItem)
        {
            if (id != InventoryItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(InventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/InventoryItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> PostInventoryItem(InventoryItem InventoryItem)
        {
            _context.InventoryItem.Add(InventoryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryItem", new { id = InventoryItem.ID }, InventoryItem);
        }

        // DELETE: api/InventoryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var InventoryItem = await _context.InventoryItem.FindAsync(id);
            if (InventoryItem == null)
            {
                return NotFound();
            }

            _context.InventoryItem.Remove(InventoryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItem.Any(e => e.ID == id);
        }
    }
}
