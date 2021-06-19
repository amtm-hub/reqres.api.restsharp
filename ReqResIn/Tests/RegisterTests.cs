using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReqResIn.Dto;
using ReqResIn.Helpers;
using System.Net;

namespace ReqResIn.Tests
{
    public class RegisterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("eve.holt@reqres.in", "pistol")]
        public void TestRegister(string email, string password)
        {
            dynamic body = new JObject();
            body.email = email;
            body.password = password;

            var api = new RestApiHelper<RespRegister>();
            var client = api.SetUrl("/api/register");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespRegister>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((RespRegister) content).Id.Should().NotBe(null);
            ((RespRegister) content).Token.Should().NotBeNullOrWhiteSpace();
        }

        [TestCase("sydney@fife")]
        public void TestRegisterError(string email)
        {
            dynamic body = new JObject();
            body.email = email;

            var api = new RestApiHelper<RespError>();
            var client = api.SetUrl("/api/register");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespError>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.BadRequest);
            ((RespError) content).Error.Should().Be("Missing password");
        }
    }
}
