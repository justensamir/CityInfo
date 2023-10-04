namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string mailTo = "mohamedsamirasaad2000@gmail.com";
        private string mailFrom = "noreply@gmail.com";
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail From {mailFrom} to {mailTo}, " +
                $"with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
