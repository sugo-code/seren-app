using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Sms;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SerenApp.Core.Interfaces;

namespace SugoCode.QueueTrigger
{

    public class AlarmsSender
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration config;
        // COMMUNICATION_SERVICES_CONNECTION_STRING=endpoint=https://sugocode-communication-service.communication.azure.com//;accesskey=B4spR//dQzjRhxOb0qFgBbu5SiXI5COtdvvP//gE3TiALjrJ42n+bbG8aN+rCkeIwsGTtU+xOQoeixbnc7iZYjBw==
        //private readonly string comm_service_conn = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
        //private readonly string comm_service_conn = "endpoint=https://sugocode-communication-service.communication.azure.com/;accesskey=B4spR/dQzjRhxOb0qFgBbu5SiXI5COtdvvP/gE3TiALjrJ42n+bbG8aN+rCkeIwsGTtU+xOQoeixbnc7iZYjBw==";
        private const string mailAndreaZanetti = "andrea.zanetti2892@gmail.com";

        public AlarmsSender(IDeviceRepository deviceRepo,
            IUserRepository userRepo, ILogger<AlarmsSender> logger,
            IConfiguration configuration)
        {
            _deviceRepo = deviceRepo;
            _userRepo = userRepo;
            config = configuration;
        }

        [FunctionName("AlarmsSender")]
        public async Task Run([ServiceBusTrigger("sugocode-servicebus-queue", Connection = "ServiceBusQueueConn")] string myQueueItem, ILogger log)
        {
            var exceptions = new List<Exception>();
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var device = await _deviceRepo.GetById(Guid.Parse(myQueueItem.Split(" ")[0]));
            var user = await _userRepo.GetById(device.User.ID);
            //SmsClient smsClient = new SmsClient(comm_service_conn);
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            if (myQueueItem.Split(" ")[1] == "Battery")
            {
                
                    mailMessage.From = new MailAddress(mailAndreaZanetti);
                    mailMessage.To.Add(new MailAddress("nicola.bovolato@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.To.Add(new MailAddress("riccardo.donadel@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.To.Add(new MailAddress("devis.tollon@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.To.Add(new MailAddress("andrea.zanetti@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.Subject = "Pezzente de gai el devais scarico";
                    mailMessage.Body = $"Pezzente de gai el devais scarico {device.Name}";

                    smtpClient.Port = 587;
                    smtpClient.Host = "smtp.gmail.com"; //for gmail host  
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(mailAndreaZanetti, config["PassworMail"]);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    smtpClient.Send(mailMessage);

                    /*SmsSendResult sendResult = await smsClient.SendAsync(
                        from: adminSugoCode,
                        to: user.PhoneNumber,
                        message: "La batteria del tuo briaccialetto è sotto il 20%! Ricaricalo pezzente!"
                    );*/

                    //if (sendResult.Successful) log.LogInformation($"Battery Message was sent: {sendResult}");
                    //else log.LogError("Error in sending Battery message" + sendResult.ToString());

                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            else if (myQueueItem.Split(" ")[1] == "Fallen")
            {
                
                    mailMessage.From = new MailAddress(mailAndreaZanetti);
                    mailMessage.To.Add(new MailAddress("nicola.bovolato@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.To.Add(new MailAddress("riccardo.donadel@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.To.Add(new MailAddress("devis.tollon@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.To.Add(new MailAddress("andrea.zanetti@stud.tecnicosuperiorekennedy.it"));
                    mailMessage.Subject = "Dio can l'è cadù";
                    mailMessage.Body = $"Vara ostia che l'è na en tera quel col devais {device.Name}";

                    smtpClient.Port = 587;
                    smtpClient.Host = "smtp.gmail.com"; //for gmail host  
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(mailAndreaZanetti, config["PassworMail"]);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    smtpClient.Send(mailMessage);
                    // Response<IReadOnlyList<SmsSendResult>> sendResult = await smsClient.SendAsync(
                    //    from: adminSugoCode,
                    //    to: new string[] { user.PhoneNumber, user.SecureContactPhoneNumber },
                    //    message: $"La persona del briaccialetto {device.ID} è caduta! Chiama i soccorsi e prega!"
                    //);

                    // IEnumerable<SmsSendResult> results = sendResult.Value;
                    // foreach (SmsSendResult result in results)
                    // {
                    //     if (result.Successful) log.LogInformation($"Fallen Message was sent: {result}");
                    //     else log.LogError("Error in sending Fallen Message: " + result.ToString());
                    // }

                }
                catch (Exception ex)
                {

                    exceptions.Add(ex);
                }
            }
            await Task.Delay(5000);
            await Task.Yield();

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
