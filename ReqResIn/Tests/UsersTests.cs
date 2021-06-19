using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReqResIn.Dto;
using ReqResIn.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ReqResIn.Tests
{
    public class UsersTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("morpheus", "leader")]
        [TestCase("neo", "chosen")]
        public void TestCreate(string name, string job)
        {
            dynamic body = new JObject();
            body.name = name;
            body.job = job;

            var api = new RestApiHelper<RespCreate>();
            var client = api.SetUrl("/api/users");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespCreate>(response);

            ((HttpStatusCode)response.StatusCode).Should().Be(HttpStatusCode.Created);
            ((string)content.Name).Should().Be((string)body.name);
            ((string)content.Job).Should().Be((string)body.job);
        }

        [TestCase("morpheus", "zion resident", 2)]
        public void TestUpdate(string name, string job, int param)
        {
            var now = DateTime.UtcNow;
            dynamic body = new JObject();
            body.name = name;
            body.job = job;

            var api = new RestApiHelper<RespUpdate>();
            var client = api.SetUrl($"/api/users/{param}");
            var request = api.CreatePutRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespUpdate>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((string)content.Name).Should().Be((string)body.name);
            ((string)content.Job).Should().Be((string)body.job);
            ((DateTime) content.UpdatedAt).Should().BeOnOrAfter(now);
        }

        [TestCase(2)]
        public void TestDelete(int param)
        {
            var api = new RestApiHelper<object>();
            var client = api.SetUrl($"/api/users/{param}");
            var request = api.CreateDeleteRequest();
            var response = api.GetResponse(client, request);
            var content = api.GetContent<object>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.NoContent);
            content.Should().BeNull();
        }

        [TestCase(1)]
        [TestCase(2)]
        public void TestListUsers(int param)
        {
            var api = new RestApiHelper<RespListUsers>();
            var client = api.SetUrl("/api/users");
            var request = api.CreateGetRequest(new Dictionary<string, string> {{"page", $"{param}"}});
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespListUsers>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.OK);
            foreach (var datum in content.Data)
            {
                datum.Avatar.Should().NotBeNullOrWhiteSpace();
                datum.Email.Should().NotBeNullOrWhiteSpace();
                datum.First_Name.Should().NotBeNullOrWhiteSpace();
                datum.Id.Should().NotBe(null);
                datum.Last_Name.Should().NotBeNullOrWhiteSpace();
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        public void TestSingleUser(int param)
        {
            var api = new RestApiHelper<RespSingleUser>();
            var client = api.SetUrl($"/api/users/{param}");
            var request = api.CreateGetRequest();
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespSingleUser>(response);

            ((HttpStatusCode)response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((RespSingleUser)content).Data.Avatar.Should().NotBeNullOrWhiteSpace();
            ((RespSingleUser)content).Data.Email.Should().NotBeNullOrWhiteSpace();
            ((RespSingleUser)content).Data.First_Name.Should().NotBeNullOrWhiteSpace();
            ((RespSingleUser)content).Data.Id.Should().NotBe(null);
            ((RespSingleUser)content).Data.Last_Name.Should().NotBeNullOrWhiteSpace();
        }

        [TestCase(0)]
        [TestCase(23)]
        public void TestSingleUserNotFound(int param)
        {
            var api = new RestApiHelper<object>();
            var client = api.SetUrl($"/api/users/{param}");
            var request = api.CreateGetRequest();
            var response = api.GetResponse(client, request);
            var content = api.GetContent<object>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.NotFound);
            ((IEnumerable) content).Should().BeEmpty();
        }
    }
}