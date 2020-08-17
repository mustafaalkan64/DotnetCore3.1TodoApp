using DotnetCore_ToDo_App;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Todo_App.IntegrationTest
{

    public class AppTestFixture : WebApplicationFactory<Startup>
    {
        //override methods here as needed for Test purpose
    }
    public class TodoListIntegrationTest : IClassFixture<AppTestFixture>
    {
        readonly AppTestFixture _fixture;
        readonly HttpClient _client;
        public TodoListIntegrationTest(AppTestFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.CreateClient();
        }


        [Theory]
        [InlineData("/api/TodoListApi")]
        [InlineData("/api/TodoListApi/getById/1")]
        public async void Get_GetBookListAll_Valid_Success(string url)
        {
            //TODO: Stil Preparing ..
            // Act
            var response = await _client.GetAsync(url);
            // Assert1
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Assert2
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
