using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.web.Utils
{
    public static class HttpClientExtensions
    {
        private static MediaTypeHeaderValue contentType = 
            new MediaTypeHeaderValue("application/json");

        public static async Task<T> ReadContentAsAsync<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"something Went wrong calling the API: " + 
                    $"{response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>
                (dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;

            return await httpClient.PostAsync(url, content);
        }
        
        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;

            return await httpClient.PutAsync(url, content);
        }


    }
}
