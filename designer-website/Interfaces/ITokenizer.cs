namespace designer_website.Interfaces
{
    public interface ITokenizer
    {
        public string GetRandomToken(); // Получить случайный токен
        public string GetToken(string value); // Получить токен на основе значения
    }
}