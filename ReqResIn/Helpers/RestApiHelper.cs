using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace ReqResIn.Helpers
{
    public sealed class RestApiHelper<T>
    {
        private RestClient _restClient;
        private RestRequest _restRequest;
        private readonly string _baseUrl;

        public RestApiHelper()
        {
            _baseUrl = "https://reqres.in";
        }

        public RestClient SetUrl(string value)
        {
            _restClient = new RestClient($"{_baseUrl}{value}");
            return _restClient;
        }

        public RestRequest CreatePostRequest(JObject value)
        {
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", JsonConvert.SerializeObject(value), ParameterType.RequestBody);
            return _restRequest;
        }

        public RestRequest CreatePutRequest(JObject value)
        {
            _restRequest = new RestRequest(Method.PUT);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", value, ParameterType.RequestBody);
            return _restRequest;
        }

        public RestRequest CreateGetRequest()
        {
            _restRequest = new RestRequest(Method.GET);
            _restRequest.AddHeader("Accept", "application/json");
            return _restRequest;
        }

        public RestRequest CreateGetRequest(Dictionary<string, string> queryParams)
        {
            _restRequest = new RestRequest(Method.GET);
            _restRequest.AddHeader("Accept", "application/json");
            foreach (var q in queryParams)
                _restRequest.AddQueryParameter(q.Key, q.Value);
            return _restRequest;
        }

        public RestRequest CreateDeleteRequest()
        {
            _restRequest = new RestRequest(Method.DELETE);
            _restRequest.AddHeader("Accept", "application/json");
            return _restRequest;
        }

        public IRestResponse GetResponse(RestClient restClient, RestRequest restRequest)
        {
            return restClient.Execute(restRequest);
        }

        public DTO GetContent<DTO>(IRestResponse response)
        {
            var content = response.Content;
            return JsonConvert.DeserializeObject<DTO>(content);
        }
    }
}
