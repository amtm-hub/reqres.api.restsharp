using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReqResIn.Dto;
using ReqResIn.Helpers;
using RestSharp;
using System.Net;

namespace ReqResIn.Tests
{
    public class LoginTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("eve.holt@reqres.in", "cityslicka")]
        public void TestLogin(string email, string password)
        {
            dynamic body = new JObject();
            body.email = email;
            body.password = password;

            var api = new RestApiHelper<RespLogin>();
            var client = api.SetUrl("/api/login");
            var request = api.CreateRequest(Method.POST, body, ParameterType.RequestBody);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespLogin>(response);

            ((HttpStatusCode)response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((RespLogin)content).Token.Should().NotBeNullOrWhiteSpace();
        }

        [TestCase("peter@klaven")]
        public void TestLoginError(string email)
        {
            dynamic body = new JObject();
            body.email = email;

            var api = new RestApiHelper<RespError>();
            var client = api.SetUrl("/api/login");
            var request = api.CreateRequest(Method.POST, body, ParameterType.RequestBody);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespError>(response);

            ((HttpStatusCode)response.StatusCode).Should().Be(HttpStatusCode.BadRequest);
            ((RespError)content).Error.Should().Be("Missing password");
        }
    }
}
