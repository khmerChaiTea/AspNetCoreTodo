using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.UnitTests
{
	public class TodoItemServiceShould
	{
		[Fact]
		public async Task AddNewItemAsIncompleteWithDueDate()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

			// Set up a context (connection to the "DB") for writing p. 103
			using (var context = new ApplicationDbContext(options))
			{
				var service = new TodoItemService(context);

				var fakeUser = new IdentityUser // Change ApplicationUser to IdentityUser
				{
					Id = "fake-000",
					UserName = "fake@example.com"
				};

				await service.AddItemAsync(new TodoItem
				{
					Title = "Testing?"
				}, fakeUser);
			}

			// Use a separate context to read data back from the "DB"
			using (var context = new ApplicationDbContext(options))
			{
				var itemsInDatabase = await context
					.Items.CountAsync();
				Assert.Equal(1, itemsInDatabase);

				var item = await context.Items.FirstAsync();
				Assert.Equal("Testing?", item.Title);
				Assert.False(item.IsDone);	// use false on the outside

				// Item should be due 3 days from now (give or take a second)
				var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
				Assert.True(difference < TimeSpan.FromSeconds(1));
			}
		}

		// Test that MarkDoneAsync returns false if the item ID doesn’t exist
		[Fact]
		public async Task MarkDoneAsync_ReturnsFalse_WhenItemNotFound()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Test_MarkDoneAsync_ReturnsFalse").Options;

			using (var context = new ApplicationDbContext(options))
			{
				var service = new TodoItemService(context);
				var user = new IdentityUser { Id = "fake-000" };

				var result = await service.MarkDoneAsync(Guid.NewGuid(), user); // Pass IdentityUser

				Assert.False(result);
			}
		}

		// Test that MarkDoneAsync returns true when it marks an item as complete
		[Fact]
		public async Task MarkDoneAsync_ReturnsTrue_WhenItemMarkedAsComplete()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Test_MarkDoneAsync_ReturnsTrue").Options;

			using (var context = new ApplicationDbContext(options))
			{
				var service = new TodoItemService(context);
				var user = new IdentityUser { Id = "fake-000" };

				var item = new TodoItem
				{
					Title = "Test Item",
					IsDone = false,
					UserId = user.Id // Set UserId
				};
				context.Items.Add(item);
				await context.SaveChangesAsync();

				var result = await service.MarkDoneAsync(item.Id, user); // Pass IdentityUser

				Assert.True(result);
				var updatedItem = await context.Items.FindAsync(item.Id);
				Assert.NotNull(updatedItem); // Ensure item is not null
				Assert.True(updatedItem.IsDone);
			}
		}

		// Test that GetIncompleteItemsAsync returns only the items owned by a particular user
		[Fact]
		public async Task GetIncompleteItemsAsync_ReturnsOnlyItemsOwnedByUser()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Test_GetIncompleteItemsAsync").Options;

			var userId1 = "user1";
			var userId2 = "user2";

			using (var context = new ApplicationDbContext(options))
			{
				var service = new TodoItemService(context);

				var item1 = new TodoItem
				{
					Title = "Item for user1",
					IsDone = false,
					UserId = userId1
				};
				var item2 = new TodoItem
				{
					Title = "Item for user2",
					IsDone = false,
					UserId = userId2
				};
				context.Items.AddRange(item1, item2);
				await context.SaveChangesAsync();

				var user1 = new IdentityUser { Id = userId1 }; // Create IdentityUser
				var user2 = new IdentityUser { Id = userId2 }; // Create IdentityUser

				var itemsForUser1 = await service.GetIncompleteItemsAsync(user1); // Pass IdentityUser
				var itemsForUser2 = await service.GetIncompleteItemsAsync(user2); // Pass IdentityUser

				Assert.Single(itemsForUser1);
				Assert.Equal("Item for user1", itemsForUser1.First().Title);

				Assert.Single(itemsForUser2);
				Assert.Equal("Item for user2", itemsForUser2.First().Title);
			}
		}
	}
}
// dotnet add AspNetCoreTodo.UnitTests/AspNetCoreTodo.UnitTests.csproj reference AspNetCoreTodo/AspNetCoreTodo.csproj (p. 100)
// dotnet add AspNetCoreTodo.UnitTests package Microsoft.EntityFrameworkCore.InMemory
// dotnet add AspNetCoreTodo.UnitTests package Microsoft.AspNetCore.Identity.EntityFrameworkCore
// dotnet add package Microsoft.AspNetCore.Identity (Visual Studio) p. 104; make sure to have using Microsoft.AspNetCore.Identity;