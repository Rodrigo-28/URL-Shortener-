namespace URL_Shortener.Options
{
    public class RateLimitOptions
    {
        // Nombre del header para particionar por cliente (si está presente)
        public string ApiKeyHeader { get; set; } = "X-Api-Key";

        // Política de creación (POST /)
        public CreatePolicyOptions Create { get; set; } = new();

        public class CreatePolicyOptions
        {
            public int PermitLimit { get; set; } = 3;
            public int WindowSeconds { get; set; } = 20;
            public int QueueLimit { get; set; } = 0;
            public int RetryAfterSeconds { get; set; } = 60;
        }
    }
}
