using URLShortener.Application.Interfaces;
using URLShortener.Domian.Common;

namespace URLShortener.Application.Services
{
    public class RandomCodeGeneratorService : ICodeGeneratorService
    {
        private readonly Random _random = new();
        public string Generate()
        {
            var codeChars = new char[ShortLinkSettings.Length];
            int maxValue = ShortLinkSettings.Alphabet.Length;

            for (int i = 0; i < ShortLinkSettings.Length; i++)
            {
                var randomIndex = _random.Next(maxValue);
                codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
            }

            return new string(codeChars);
        }
    }
}
