using System.Collections.Generic;
using MimeKit;
namespace EmailService
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public List<string> ToAdressList { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
