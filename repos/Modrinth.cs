using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Stargazer.Repos.Models;

namespace Stargazer.Repos {
    public class ModrinthAPI {
        private HttpClient client;
        private const string BaseUrl = @"https://api.modrinth.com/v2/";

        public ModrinthAPI() {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-agent", "Mercurius");
        }
        public async Task<SearchModel> SearchAsync(string query) {
                SearchModel deserializedRes;

                try {
                    Stream responseStream = await client.GetStreamAsync(BaseUrl + $@"search?query={query}");
                    deserializedRes = await JsonSerializer.DeserializeAsync<SearchModel>(responseStream);
                } catch (Exception) {
                    throw new ApiException("Connection Failed!");
                }

                if (deserializedRes.hits.Length <= 0) {
                    throw new ApiException("No results found");
                }

                return deserializedRes;
            }
    }
    public class ApiException : Exception {
        public ApiException() { }
        public ApiException(string message) : base(message) { }
        public ApiException(string message, System.Exception inner) : base(message, inner) { }
        protected ApiException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class VersionInvalidException : System.Exception
    {
        public VersionInvalidException() { }
        public VersionInvalidException(string message) : base(message) { }
        public VersionInvalidException(string message, System.Exception inner) : base(message, inner) { }
        protected VersionInvalidException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class ProjectInvalidException : System.Exception
    {
        public ProjectInvalidException() { }
        public ProjectInvalidException(string message) : base(message) { }
        public ProjectInvalidException(string message, System.Exception inner) : base(message, inner) { }
        protected ProjectInvalidException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}