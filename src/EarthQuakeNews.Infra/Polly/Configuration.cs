using Polly;

namespace EarthQuakeNews.Infra.Polly
{
    public static class Configuration
    {
        public static async Task<HttpResponseMessage> GetAsyncWithRetry(this HttpClient client, string url, CancellationToken cancellationToken = default)
        {
            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(c => c?.Content is null || !c.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .Or<Exception>()
                .WaitAndRetryAsync(7, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var response = await retryPolicy.ExecuteAsync(async () => await client.GetAsync(url, cancellationToken));

            return response;
        }
    }
}
