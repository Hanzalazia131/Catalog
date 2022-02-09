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
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }
        
        [HttpPost]
        public ActionResult<ItemDto> CreateItem (CreateItemDto itemDto)
        {
            Item item = new Item(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem),new {Id = item.Id}, item.AsDto());
        }

        [HttpPut("{Id}")]
        public ActionResult UpdateItem(Guid Id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(Id);
            
            if (existingItem == null){
                return NotFound();            
            }   
            Item UpdateItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            repository.UpdateItem(UpdateItem);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public ActionResult DeleteItem(Guid Id)
        {
            var existingItem = repository.GetItem(Id);
            if (existingItem == null)
            {
                return NotFound();
            }
            repository.DeleteItem(Id);
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