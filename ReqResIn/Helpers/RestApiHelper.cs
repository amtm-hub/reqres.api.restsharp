using Newtonsoft.Json;
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

        public RestRequest CreateRequest(Method method)
        {
            _restRequest = new RestRequest(method);
            _restRequest.AddHeader("Accept", "application/json");
            return _restRequest;
        }

        public RestRequest CreateRequest(Method method, dynamic value, ParameterType parameterType)
        {
            _restRequest = new RestRequest(method);
            _restRequest.AddHeader("Accept", "application/json");

            switch (parameterType)
            {
                case ParameterType.QueryString:
                    {
                        Dictionary<string, string> values = value;
                        foreach (var (key, s) in values) _restRequest.AddQueryParameter(key, s);
                        break;
                    }
                case ParameterType.RequestBody:
                    _restRequest.AddParameter("application/json", JsonConvert.SerializeObject(value), ParameterType.RequestBody);
                    break;
            }

            return _restRequest;
        }

        public IRestResponse GetResponse(RestClient client, RestRequest request)
        {
            return client.Execute(request);
        }

        public DTO GetContent<DTO>(IRestResponse response)
        {
            var content = response.Content;
            return JsonConvert.DeserializeObject<DTO>(content);
        }
    }
}
