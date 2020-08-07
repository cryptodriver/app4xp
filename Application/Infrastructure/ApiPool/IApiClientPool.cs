namespace Application.Infrastructure.ApiPool
{
    public interface IApiClientPool
    {
        int AvailableCount { get; }

        int TotalPoolSize { get; }

        ApiClient Fetch();

        ApiClientConfig Configuration { get; set; }

        void Flush();

        void ResetAuth();
    }
}
