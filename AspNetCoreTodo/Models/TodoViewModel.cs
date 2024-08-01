namespace AspNetCoreTodo.Models
{
	public class TodoViewModel
	{
		// Help Items stop complaining about null
		public TodoViewModel()
		{
			Items = Array.Empty<TodoItem>();
		}

		public TodoItem[] Items { get; set; }
	}
}
