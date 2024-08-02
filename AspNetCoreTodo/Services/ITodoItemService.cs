using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
	public interface ITodoItemService
	{
		Task<TodoItem[]> GetIncompleteItemsAsync();

		Task<bool> AddItemAsync(TodoItem newItem);  // P. 67

		Task<bool> MarkDoneAsync(Guid id);	// P. 72
	}
}
// P. 32-33