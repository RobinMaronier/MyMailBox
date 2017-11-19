using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMailBox.Models
{
    class Account
    {
        public String fullName = String.Empty;
        private String emailAddress = String.Empty;
        public String password = String.Empty;
        public Boolean rememberPassword = false;
        public String signature = String.Empty;

        private String mailService = String.Empty;
        private String identity = String.Empty;
        public int port = 993;
        public Boolean useSSL = true;

        public Account(String fullName, String emailAddress, String password, Boolean rememberPassword)
        {
            this.fullName = fullName;
            this.emailAddress = emailAddress;
            this.password = password;
            this.rememberPassword = rememberPassword;
        }

        public Account() { }

        public void initPassword()
        {
            password = String.Empty;
        }

        public String getMailService()
        {
            if (this.mailService == String.Empty)
            {
                this.mailService = emailAddress.Substring(emailAddress.IndexOf("@") + 1);
            }
            return this.mailService;
        }

        public String getServer(String type)
        {
            return type + "." + getMailService();
        }

        public String getEmail()
        {
            return emailAddress;
        }

        public void setEmail(String newEmail)
        {
            mailService = String.Empty;
            identity = String.Empty;
            emailAddress = newEmail;
        }

        public String getIdentity()
        {
            if (this.identity == String.Empty)
            {
                this.identity = emailAddress.Substring(0, emailAddress.IndexOf("@"));
            }
            return this.identity;
        }
    }
}
