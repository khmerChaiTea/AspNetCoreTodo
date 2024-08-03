using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCoreTodo.IntegrationTests
{
	public class TestFixture : IDisposable
	{
		private readonly WebApplicationFactory<Program> _factory;

		public HttpClient Client { get; }

		public TestFixture()
		{
			_factory = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureAppConfiguration((context, config) =>
					{
						config.AddJsonFile("appsettings.json");
					});
				});

			Client = _factory.CreateClient();
			Client.BaseAddress = new Uri("http://localhost:8888");
		}

		public void Dispose()
		{
			Client.Dispose();
			_factory.Dispose();
		}
	}
}