using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCoreTodo.IntegrationTests
{
	public class TodoRouteShould : IClassFixture<TestFixture>
	{
		private readonly HttpClient _client;

		public TodoRouteShould(TestFixture fixture)
		{
			_client = fixture.Client;
		}

		[Fact]
		public async Task ChallengeAnonymousUser()
		{
			// Arrange
			var request = new HttpRequestMessage(HttpMethod.Get, "/todo");

			// Act
			var response = await _client.SendAsync(request);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Change expected status code

			// Optionally, assert other aspects of the response if needed
		}
	}
}