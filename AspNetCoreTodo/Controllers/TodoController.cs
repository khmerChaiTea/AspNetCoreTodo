using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;
namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
		private readonly ITodoItemService _todoItemService;

		public TodoController(ITodoItemService todoItemService)
		{
			_todoItemService = todoItemService;
		}   // P. 36

		// P. 38 update
		public async Task<IActionResult> Index()
		{
			// Get to-do items from database p. 37
			var items = await _todoItemService.GetIncompleteItemsAsync();

			// Put items into a model
			var model = new TodoViewModel()
			{
				Items = items
			};

			// Render view using the model
			return View(model);	// P. 41
		}   // P.22-23

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddItem(TodoItem newItem)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
			var successful = await _todoItemService.AddItemAsync(newItem);
			if (!successful)
			{
				return BadRequest("Could not add item.");
			}
			return RedirectToAction("Index");
		}	// P. 64
	}
}
// make sure to have using AspNetCoreTodo.Services; using AspNetCoreTodo.Models;
// Run App and test by type http://localhost:xxxx/todo (xxxx is the numbers)