using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utilities
{
    public class EmailSender : IEmailSender
    {
        // Simulate email sending by logging the action
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // Log the email sending simulation
                Console.WriteLine($"Simulating email send to: {email}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Message: {htmlMessage}");

                // If you had a logger, you could log this information here instead
                // e.g., _logger.LogInformation($"Simulated email sent to: {email}");

                return Task.CompletedTask;  // Simulate a successful email sending
            }
            catch (Exception ex)
            {
                // Log any errors (you could use a logging framework here like Serilog or NLog)
                Console.WriteLine($"Error simulating email send: {ex.Message}");

                // Optionally, rethrow the error if needed, or return a failed task
                throw new InvalidOperationException("Failed to simulate sending email.", ex);
            }
        }
    }
}
