using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblifun.Email
{
    public class Emailer
    {
        readonly ILogger<Emailer> _logger;

        public Emailer(ILogger<Emailer> logger)
        {
            _logger = logger;
        }

        public async Task SendEmail(string toAddress,
                                    string toDisplayName,
                                    string subject,
                                    string body,
                                    bool isHtml = true)
        {
            // input validation
            if (string.IsNullOrWhiteSpace(toAddress))
            {
                throw new ArgumentNullException("toAddress");
            }
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException("subject");
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentNullException("body");
            }

            try
            {
                // create the message
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(Settings.Default.FromAddress, Settings.Default.FromDisplay),
                    Subject = subject
                };

                msg.AddTo(toAddress, toDisplayName);

                var mimeType = isHtml ? MimeType.Html : MimeType.Text;

                msg.AddContent(mimeType, body);

                // get the SendGrid client
                var client = new SendGridClient(Settings.Default.SendGridAPIKey);

                var response = await client.SendEmailAsync(msg);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Accepted:
                        _logger.LogInformation($"Email sent to {toAddress} with Subject:{subject}");
                        break;

                    default:
                        var responseBody = await response?.Body?.ReadAsStringAsync();
                        _logger.LogError($"Error sending email. Client response code:{response?.StatusCode} Message:{responseBody}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email: " + ex.Message);
            }
        }
    }
}
