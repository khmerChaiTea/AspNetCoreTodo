using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
	public interface ITodoItemService
	{
		Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user);	// P. 81

		Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user);  // P. 67, 84

		Task<bool> MarkDoneAsync(Guid id, IdentityUser user);	// P. 72, 84
	}
}
// P. 32-33