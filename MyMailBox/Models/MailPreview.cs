using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;

namespace MyMailBox.Models
{
    public class MailPreview
    {
        public String mailObject { get; set; }
        public String mailFrom { get; set; }
        public String date { get; set; }
        private UniqueId uniqueId;

        public MailPreview(String mailObject, String mailFrom, String date, UniqueId index)
            : this(mailObject, mailFrom, date)
        {
            this.uniqueId = index;
        }

        protected MailPreview(String mailObject, String mailFrom, String date)
        {
            this.mailObject = mailObject;
            this.mailFrom = mailFrom;
            this.date = date;
        }

        public UniqueId getUniqueID()
        {
            return (this.uniqueId);
        }
    }
}
