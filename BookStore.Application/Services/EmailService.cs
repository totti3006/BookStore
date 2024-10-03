﻿using BookStore.Application.Configurations;
using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Email;
using BookStore.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookStore.Application.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly IUnitOfWork _unitOfWork;
        private readonly SMTPConfig _smtpConfig;

        public EmailService(IUnitOfWork unitOfWork, IOptions<SMTPConfig> smtpConfig)
        {
            _unitOfWork = unitOfWork;
            _smtpConfig = smtpConfig.Value;
        }

        public async Task SendEmailForDailyReport(string filePath)
        {
            try
            {
                var attachment = new Attachment(filePath);

                var userEmailOptions = new UserEmailOptions
                {
                    Subject = $"Daily report {DateTime.Now.ToString("dd/MM/yyyy")}",
                    Body = "Daily report auto generated by the system. Please check the attachment.",
                    RecipientEmails = new() { "gsamme1@instagram.com", "kedat30062001@gmail.com" },
                    Attachments = new() { attachment }
                };

                await SendEmail(userEmailOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailForResetPassword(string email)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.SingleOrDefault(x => x.Email == email);

                if (user is null)
                {
                    throw new BusinessException("Email doesn't exist");
                }

                string otp = await GenerateOTP(user.Id);

                var pair = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.Name),
                    new KeyValuePair<string, string>("{{OTP}}", otp)
                };

                var userEmailOptions = new UserEmailOptions
                {
                    Subject = "Reset password OTP",
                    Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), pair),
                    RecipientEmails = new() { email },
                };

                await SendEmail(userEmailOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailForSuccessOrder(InvoiceEmailDto invoiceEmailDto)
        {
            try
            {
                string bookList = string.Join("\t\n", invoiceEmailDto.Books.Select(b => "\t" + b.Title + $" - ${b.Price}")
                                                                         .ToArray());

                string? content = $"Hi {invoiceEmailDto.Name}\n" +
                                  $"You had placed an order at {invoiceEmailDto.CreatedDate.ToLocalTime()}:\n" + 
                                  bookList + "\n" +
                                  $"Order id: {invoiceEmailDto.OrderId}\n" +
                                  $"Quantity: {invoiceEmailDto.Quantity}\n" +
                                  $"Total price: {invoiceEmailDto.TotalPrice}\n" +
                                  "Cheers.";

                var userEmailOptions = new UserEmailOptions
                {
                    Subject = "Book store E-Invoice",
                    Body = content,
                    RecipientEmails = new() { invoiceEmailDto.Email }
                };

                await SendEmail(userEmailOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GenerateOTP(Guid userId)
        {
            Random rnd = new Random();
            string otpCode = (rnd.Next() % 1000000).ToString("000000");

            var createdDate = DateTime.Now;

            var otp = new Otp
            {
                Id = Guid.NewGuid(),
                Code = otpCode,
                CreatedDate = createdDate,
                ExpiredDate = createdDate.AddMinutes(5),
                IsVerified = false,
                UserId = userId
            };

            await _unitOfWork.OtpRepository.Add(otp);
            await _unitOfWork.SaveChanges();

            return otpCode;
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            string? senderEmail = _smtpConfig.SenderAddress;
            string? appPassword = _smtpConfig.Password;
            string? senderDisplayName = _smtpConfig.SenderDisplayName;

            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(senderEmail, senderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            foreach (Attachment attachment in userEmailOptions.Attachments)
            {
                mail.Attachments.Add(attachment);
            }

            foreach (string recipients in userEmailOptions.RecipientEmails)
            {
                mail.To.Add(recipients);
            }

            var networkCredential = new NetworkCredential(_smtpConfig.UserName, appPassword);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}