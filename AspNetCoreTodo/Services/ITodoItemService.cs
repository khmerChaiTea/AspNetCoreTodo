using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
	// This is an interface so change from class to interface
	public interface ITodoItemService
	{
		Task<TodoItem[]> GetIncompleteItemsAsync();

		Task<bool> AddItemAsync(TodoItem newItem);
	}
}
