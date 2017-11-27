namespace TodoApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using TodoApi.Models;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        public TodoController(ITodoRepository todoItems)
        {
            TodoItems = todoItems;
        }

        public ITodoRepository TodoItems { get; set; }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll() => TodoItems.GetAll();

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var item = TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create ([FromBody]TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            TodoItems.Add(item);
            var s = new { id = item.Key };
            Console.WriteLine("get type check : " + s.GetType());
            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update (
            string id,
            [FromBody]TodoItem item)
        {
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }

            var todo = TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoItems.Update(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var item = TodoItems.Remove(id);
            Console.WriteLine($"remove item -> {item.Key} , {item.Name} ");
        }
    }
}