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
		}

		// Change IActionResult to async Task<IActionResult>
		public async Task<IActionResult> Index()
		{
			// Get to-do items from database
			var items = await _todoItemService.GetIncompleteItemsAsync();

			// Put items into a model
			var model = new TodoViewModel()
			{
				Items = items
			};

			// Render view using the model
			return View(model);
		}

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
		}
	}
}
