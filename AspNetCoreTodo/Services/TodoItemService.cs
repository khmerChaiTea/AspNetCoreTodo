using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace AspNetCoreTodo.Services
{
	public class TodoItemService : ITodoItemService
	{
		private readonly ApplicationDbContext _context;

		public TodoItemService(ApplicationDbContext context)
		{
			_context = context;
		}

		// P. 81
		public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user)
		{
			return await _context.Items
				.Where(x => x.IsDone == false && x.UserId == user.Id)
				.ToArrayAsync();
		}	// P. 82

		public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
		{
			newItem.Id = Guid.NewGuid();
			newItem.IsDone = false;

			newItem.UserId = user.Id;	// P. 84

			// Ensure DueAt is properly assigned from user input
			if (newItem.DueAt == default)
			{
				// Optionally handle cases where DueAt might be missing
				// For example, set a default value if not provided
				newItem.DueAt = DateTimeOffset.Now.AddDays(3); // Optional default if DueAt is not provided
			}

			_context.Items.Add(newItem);

			var saveResult = await _context.SaveChangesAsync();
			return saveResult == 1;
		}   // P. 67

		public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
		{
			var item = await _context.Items
				.Where(x => x.Id == id && x.UserId == user.Id)	// P.84
				.SingleOrDefaultAsync();

			if (item == null) return false;

			item.IsDone = true;

			var saveResult = await _context.SaveChangesAsync();
			return saveResult == 1; // One entity should have been updated
		}	// P. 73
	}
}	// P. 57; delete FakeTodoItemService.cs
