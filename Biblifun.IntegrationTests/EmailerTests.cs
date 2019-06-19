using Biblifun.Email;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Biblifun.IntegrationTests
{
    public class EmailerTests
    {

        Mock<ILogger<Emailer>> _loggerMock;

        [SetUp]
        public void InitializeMocks()
        {
            _loggerMock = new Mock<ILogger<Emailer>>();
        }

        [Test]
        public async Task SendEmail_When_valid_Then_email_is_sent()
        {
            var emailer = new Emailer(_loggerMock.Object);

            await emailer.SendEmail("dhochee@gmail.com", "Daniel Hochee", "Test Subject", "This is the message body!", false);
        }

    }
}
