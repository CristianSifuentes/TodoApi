using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Context;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {

        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            this._context = context;
            if(this._context.TodoItems.Count() == 0)
            {
                this._context.TodoItems.Add(new Models.TodoItem { Name = "Item1"});
                this._context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return this._context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if(item == null)
            {
                return BadRequest();
            }

            this._context.TodoItems.Add(item);
            this._context.SaveChanges();

            //The CreatedAtRoute method returns a 201 response, which is the standard response for an HTTP POST method that creates a new resource on the server.
            return CreatedAtRoute("GetTodo", new { id = item.Id}, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {

            if(item == null || item.Id != id)
            {
                return BadRequest();
            }
            var todo = this._context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            this._context.TodoItems.Update(todo);
            this._context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = this._context.TodoItems.First(t => t.Id == id);
            if(todo == null)
            {
                return NotFound();
            }

            this._context.TodoItems.Remove(todo);
            this._context.SaveChanges();
            return new NoContentResult();
        }



    }
}
