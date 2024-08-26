using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using NuGet.Versioning;
using ShoppingAPI.Models;

namespace ShoppingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreFrontsController : ControllerBase
    {
        private readonly DBContext _context;
        private bool allAvailableForPurchase;

        public StoreFrontsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/StoreFronts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreFront>>> GetStoreFront()
        {
            return await _context.StoreFront.ToListAsync();
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<StoreFront>> GetStoreFrontByName(string name)
        {
            StoreFront? storeFront = await _context.StoreFront.FirstOrDefaultAsync<StoreFront>(storeFront => storeFront.Name == name);

            if (storeFront != null)

                return storeFront;
            else
                return NoContent();
        }

        [HttpPost]
        [Route("{id}/addItems")]
        public async Task<ActionResult> AddStoreItems([FromBody]InventoryItem[] storeInventory)
        {
            InventoryItem[] itemsToBeAdded = new InventoryItem[1];
     
            InventoryItem? itemToChange;
            foreach (InventoryItem item in storeInventory)

            {
                itemToChange = await _context.InventoryItem.FirstOrDefaultAsync(inventoryItem => inventoryItem.Name == item.Name);
                if (itemToChange != null)
                {
                    itemToChange.UnitsAvailable += item.UnitsAvailable;
                
                    _context.Entry(itemToChange).State= EntityState.Modified;
                }
                else
                {
                    try
                    {
                        _ = await _context.InventoryItem.AddAsync(item) ?? throw new Exception();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                  
                }
                await _context.SaveChangesAsync();
            }
               

            return Created();

        }
        [HttpPost]
        [Route("items")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetStoreItems([FromBody]StoreFront storeFront)
        {
            List<InventoryItem> storeItems = new List<InventoryItem>();
            if (storeFront != null)
                await _context.InventoryItem.ForEachAsync(inventoryItem =>
                {
                    
                    if (inventoryItem != null && inventoryItem.StoreFrontID == storeFront.Id)
                        storeItems.Add(inventoryItem);
                });


            return storeItems;

        }
        // GET: api/StoreFronts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreFront>> GetStoreFront(int id)
        {
            var storeFront = await _context.StoreFront.FindAsync(id);

            if (storeFront == null)
            {
                return NotFound();
            }

            return storeFront;
        }

        // PUT: api/StoreFronts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoreFront(int id, StoreFront storeFront)
        {
            if (id != storeFront.Id)
            {
                return BadRequest();
            }

            _context.Entry(storeFront).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreFrontExists(id))
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

        // POST: api/StoreFronts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StoreFront>> PostStoreFront(StoreFront storeFront)
        {
            _context.StoreFront.Add(storeFront);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStoreFront", new { id = storeFront.Id }, storeFront);
        }

        // DELETE: api/StoreFronts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreFront(int id)
        {
            var storeFront = await _context.StoreFront.FindAsync(id);
            if (storeFront == null)
            {
                return NotFound();
            }

            _context.StoreFront.Remove(storeFront);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private  async Task IsInInventory(int storeId, CartItem cartItem)
        {
            InventoryItem? item = null;
            if (cartItem != null)
            {
                item = await _context.InventoryItem.FirstOrDefaultAsync(inventoryItem => cartItem.Name == inventoryItem.Name && inventoryItem.StoreFrontID == storeId);

                if (item != null)
                    if (item.UnitsAvailable < cartItem.UnitsInCart)
                        allAvailableForPurchase = false;

            }
           
        }

        [HttpPost]
        [Route("store/{storeId}/purchase")]
        public async Task<IActionResult> Purchase([FromRoute]int storeId,[FromBody] CartItem[] cartItems)
        {

            allAvailableForPurchase = true;


            CartItem[] shoppingCartItems = new CartItem[1];

            

            if (cartItems != null)
            {
                foreach (CartItem item in cartItems)
                {
                    await IsInInventory(storeId, item);
                }

                if (allAvailableForPurchase)
                {
                    foreach(CartItem cartItem in cartItems)
                    {
                        InventoryItem item = await _context.InventoryItem.FirstAsync(inventoryItem => (inventoryItem.Name == cartItem.Name && inventoryItem.StoreFrontID == storeId));
                        item.UnitsAvailable -= cartItem.UnitsInCart;


                        _context.Entry(item).State = EntityState.Modified;


                        await _context.SaveChangesAsync();
                    }
                   
                }
                else
                    return Ok("Not all items available for purchase. Purchase blocked.");


            }
            

            return Ok("Items purchased");
        }
            
        private bool StoreFrontExists(int id)
        {
            return _context.StoreFront.Any(e => e.Id == id);
        }
    }
}
