using System.Text;
using System.Text.Json;

namespace phantom.Core.Restful
{
    public class RestfulHelper
    {
        private interface INullType;
        public string? BaseUrl { get; set; }
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public T? GetAysnc<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync($"{BaseUrl}/{url}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var data = JsonSerializer.Deserialize<T>(json, RestfulHelper.JsonSerializerOptions);
                    return data;
                }
                else
                {
                    throw new Exception($"Error fetching data from {url}: {response.ReasonPhrase}");
                }
            }
        }
        public async Task<T?> PostAsync<T>(string url, object body)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                try
                {
                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync($"{BaseUrl}/{url}", content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseContent = await response.Content.ReadAsStringAsync();
                        if (typeof(T) == typeof(INullType)) return default(T);
                        return JsonSerializer.Deserialize<T>(responseContent, RestfulHelper.JsonSerializerOptions);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"POST request failed with status code: {response.StatusCode}\nError details: {errorContent}");
                    }
                }
                catch (HttpRequestException e)
                {
                    throw new Exception($"HTTP request exception: {e.Message}");
                }
                catch (Exception e)
                {
                    throw new Exception($"An error occurred: {e.Message}");
                }
                finally
                {
                    // Best practice to dispose of the HttpClient when no longer needed,
                    // although in this static example, it lives for the application's lifetime.
                    // For instance-based HttpClient, use 'using' or dispose.
                    client.Dispose();
                }
            }
        }
        public async Task PostAsync(string url, object body) => await PostAsync<INullType>(url, body);
    }
}