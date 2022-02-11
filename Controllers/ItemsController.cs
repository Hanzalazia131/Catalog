using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        //GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                                        .Select(item => item.AsDto());
            return items;
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }
        
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new Item(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync),new {Id = item.Id}, item.AsDto());
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid Id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(Id);
            
            if (existingItem == null){
                return NotFound();            
            }   
            Item UpdateItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            await repository.UpdateItemAsync(UpdateItem);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid Id)
        {
            var existingItem = await repository.GetItemAsync(Id);
            if (existingItem == null)
            {
                return NotFound();
            }
            await repository.DeleteItemAsync(Id);
            return NoContent();
        }
        // public void CreateItem(Item item)
        // {
        //     repository.CreateItem(item);
        // }

        // [HttpGet("{num}")]
        // public int Getnumber(int num)
        // {
        //     return num;
        // }
    }
}