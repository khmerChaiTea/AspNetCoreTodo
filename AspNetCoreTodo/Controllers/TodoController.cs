using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
	// P. 77. Add "Authorize"; using Microsoft.AspNetCore.Authorization;
	[Authorize]
	public class TodoController : Controller
    {
		private readonly ITodoItemService _todoItemService;
		private readonly UserManager<IdentityUser> _userManager;

		public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
		{
			_todoItemService = todoItemService;
			_userManager = userManager;
		}   // P. 36; 79, using Microsoft.AspNetCore.Identity;

		// P. 38 update
		public async Task<IActionResult> Index()
		{
			// P. 80
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser == null) return Challenge();

			// Get to-do items from database p. 37; add currentUser
			var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

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

			var currentUser = await _userManager.GetUserAsync(User);	// P. 83
			if (currentUser == null) return Challenge();

			var successful = await _todoItemService.AddItemAsync(newItem, currentUser);

			if (!successful)
			{
				return BadRequest("Could not add item.");
			}

			return RedirectToAction("Index");
		}   // P. 64

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> MarkDone(Guid id)
		{
			if (id == Guid.Empty)
			{
				return RedirectToAction("Index");
			}

			var currentUser = await _userManager.GetUserAsync(User);	// P. 83
			if (currentUser == null) return Challenge();

			var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

			if (!successful)
			{
				return BadRequest("Could not mark item as done.");
			}

			return RedirectToAction("Index");
		}	// P. 71
	}
}
// make sure to have using AspNetCoreTodo.Services; using AspNetCoreTodo.Models;
// Run App and test by type http://localhost:xxxx/todo (xxxx is the numbers)