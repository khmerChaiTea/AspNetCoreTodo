namespace AspNetCoreTodo.Models
{
    public class TodoViewModel
    {
        // Help Items stop complaining about null p. 26
        public TodoViewModel()
        {
            Items = Array.Empty<TodoItem>();
        }

        public TodoItem[] Items { get; set; }
    }
}
