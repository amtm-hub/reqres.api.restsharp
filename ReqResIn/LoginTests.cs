using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReqResIn.Dto;
using ReqResIn.Helpers;
using System.Net;

namespace ReqResIn
{
    public class LoginTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLogin()
        {
            dynamic body = new JObject();
            body.email = "eve.holt@reqres.in";
            body.password = "cityslicka";

            var api = new RestApiHelper<RespLogin>();
            var client = api.SetUrl("/api/login");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespLogin>(response);

            ((HttpStatusCode)response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((RespLogin)content).Token.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void TestLoginError()
        {
            dynamic body = new JObject();
            body.email = "peter@klaven";

            var api = new RestApiHelper<RespError>();
            var client = api.SetUrl("/api/login");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespError>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.BadRequest);
            ((RespError) content).Error.Should().Be("Missing password");
        }
    }
}
