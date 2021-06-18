using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReqResIn.Dto;
using ReqResIn.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ReqResIn
{
    public class UsersTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreate()
        {
            dynamic body = new JObject();
            body.name = "morpheus";
            body.job = "leader";

            var api = new RestApiHelper<RespCreate>();
            var client = api.SetUrl("/api/users");
            var request = api.CreatePostRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespCreate>(response);

            ((HttpStatusCode)response.StatusCode).Should().Be(HttpStatusCode.Created);
            ((string)content.Name).Should().Be((string)body.name);
            ((string)content.Job).Should().Be((string)body.job);
        }

        [Test]
        public void TestUpdate()
        {
            var now = DateTime.UtcNow;

            dynamic body = new JObject();
            body.name = "morpheus";
            body.job = "zion resident";

            var api = new RestApiHelper<RespUpdate>();
            var client = api.SetUrl("/api/users/2");
            var request = api.CreatePutRequest(body);
            var response = api.GetResponse(client, request);
            var content = api.GetContent<RespUpdate>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.OK);
            ((string)content.Name).Should().Be((string)body.name);
            ((string)content.Job).Should().Be((string)body.job);
            ((DateTime) content.UpdatedAt).Should().BeOnOrAfter(now);
        }

        [Test]
        public void TestDelete()
        {
            var api = new RestApiHelper<object>();
            var client = api.SetUrl("/api/users/2");
            var request = api.CreateDeleteRequest();
            var response = api.GetResponse(client, request);
            var content = api.GetContent<object>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.NoContent);
            content.Should().BeNull();
        }

        [Test]
        public void TestListUsers()
        {
            var api = new RestApiHelper<RespListUsers>();
            var client = api.SetUrl("/api/users");
            var request = api.CreateGetRequest(new Dictionary<string, string> {{"page", "2"}});
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

        [Test]
        public void TestSingleUser()
        {
            var api = new RestApiHelper<RespSingleUser>();
            var client = api.SetUrl("/api/users/2");
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

        [Test]
        public void TestSingleUserNotFound()
        {
            var api = new RestApiHelper<object>();
            var client = api.SetUrl("/api/users/23");
            var request = api.CreateGetRequest();
            var response = api.GetResponse(client, request);
            var content = api.GetContent<object>(response);

            ((HttpStatusCode) response.StatusCode).Should().Be(HttpStatusCode.NotFound);
            ((IEnumerable) content).Should().BeEmpty();
        }
    }
}