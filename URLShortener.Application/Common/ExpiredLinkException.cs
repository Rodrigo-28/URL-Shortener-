namespace URLShortener.Application.Common
{
    public class ExpiredLinkException : Exception
    {
        public ExpiredLinkException(string message) : base(message) { }
    }
}
