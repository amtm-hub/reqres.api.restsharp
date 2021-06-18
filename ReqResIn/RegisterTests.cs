using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReqResIn.Dto;
using ReqResIn.Helpers;
using System.Net;

namespace ReqResIn
{
    public class RegisterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRegisterSuccessful()
        {
            dynamic body = new JObject();
            body.email = "eve.holt@reqres.in";
            body.password = "pistol";

            var api = new RestApiHelper<RespRegister>();
            var client = api.SetUrl("/api/register");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespRegister>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((RespRegister) content).Id.Should().NotBe(null);
            ((RespRegister) content).Token.Should().NotBeNullOrWhiteSpace();
        }
    }
}
