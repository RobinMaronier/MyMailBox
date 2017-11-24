using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMailBox.Models
{
    public class Mail : MailPreview
    {
        public String body = String.Empty;
        public List<String> listTo;

        public Mail(String body, List<String> listTo, String subject, String from, String date)
            : base(subject, from, date)
        {
            this.body = body;
            this.listTo = listTo;
        }
    }
}
