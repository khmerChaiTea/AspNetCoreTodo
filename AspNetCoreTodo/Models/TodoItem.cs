using System;
using System.ComponentModel.DataAnnotations;
namespace AspNetCoreTodo.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public bool IsDone { get; set; }
        [Required]
        public string? Title { get; set; }  // ? to handle nullable p.24
        public DateTimeOffset? DueAt { get; set; }
    }
}