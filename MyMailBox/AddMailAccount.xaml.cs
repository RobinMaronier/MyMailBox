using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MailKit;
using MailKit.Net.Imap;
using MyMailBox.Models;

namespace MyMailBox
{
    /// <summary>
    /// Logique d'interaction pour AddMailAccount.xaml
    /// </summary>
    public partial class AddMailAccount : Window
    {
        private Account inCreationAccount;

        private int stepAddAccount = 0;

        public AddMailAccount()
        {
            this.InitializeComponent();
        }

        private void NextActionAddAccount(object sender, RoutedEventArgs e)
        {
            Boolean canNext = false;
            switch (stepAddAccount)
            {
                case 0:
                    canNext = doStepOne();
                    break;
                case 1:
                    canNext = doStepTwo();
                    break;
                default:
                    break;
            }
            if (canNext)
            {
                goNextStep();
            }
        }

        private void goNextStep()
        {
            stepAddAccount++;
            switch (stepAddAccount)
            {
                case 1:
                    frameStep1.Visibility = Visibility.Collapsed;
                    frameStep2.Visibility = Visibility.Visible;
                    break;
                case 2:
                    frameStep2.Visibility = Visibility.Collapsed;
                    frameStep3.Visibility = Visibility.Visible;
                    break;
                case 3:
                    frameStep3.Visibility = Visibility.Collapsed;
                    frameStep4.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private Boolean doStepOne()
        {
            Account newAccount = new Account(nameUserBox.Text, emailUserBox.Text, passwordUserBox.Password, rememberPasswordBox.IsChecked.Value);
            Boolean wrongField = false;

            if (newAccount.fullName == String.Empty)
            {
                 nameUserBox.BorderBrush = new SolidColorBrush(Colors.Red);
                //fullNameBox.Focus(FocusState.Programmatic);
                wrongField = true;
            }
            if (newAccount.password == String.Empty)
            {
                passwordUserBox.BorderBrush = new SolidColorBrush(Colors.Red);
                //passwordBox.Focus(FocusState.Programmatic);
                wrongField = true;
            }
            if (newAccount.getEmail() == String.Empty)
            {
                emailUserBox.BorderBrush = new SolidColorBrush(Colors.Red);
                //emailBox.Focus(FocusState.Programmatic);
                wrongField = true;
            }
            if (!newAccount.getEmail().Contains("@"))
            {
                emailUserBox.BorderBrush = new SolidColorBrush(Colors.Red);
                wrongField = true;
            }
            if (!wrongField)
            {
                inCreationAccount = newAccount;
                goNextStep();
                return doStepTwo();
            }
            return false;
        }

        private Boolean doStepTwo()
        {

            Boolean result = tryToConnectWith(inCreationAccount, "imap");

            if (result == false)
            {
                showMessageCantConnect();
            }
            fillStep3Information(result);
            return true;

            /*var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            Console.WriteLine("Total messages: {0}", inbox.Count);
            Console.WriteLine("Recent messages: {0}", inbox.Recent);*/


        }

        private Boolean tryToConnectWith(Account thatAccount, String type)
        {
            using (var client = new ImapClient())
            {
                String server = thatAccount.getServer(type);
                int port = thatAccount.port;
                Boolean useSSL = thatAccount.useSSL;

                try
                {
                    client.Connect(server, port, useSSL);
                }
                catch (MailKit.ServiceNotConnectedException)
                {
                    System.Diagnostics.Debug.WriteLine("Error: Connection error");
                    return false;
                }
                catch (System.Exception)
                {
                    System.Diagnostics.Debug.WriteLine("Error: Connection error");
                    return false;
                }

                var identity = thatAccount.getIdentity();

                try
                {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(identity, thatAccount.password);
                }
                catch (MailKit.Net.Imap.ImapProtocolException)
                {
                    System.Diagnostics.Debug.WriteLine("Error: Authentificate error");
                    return false;
                }
            }
            return true;
        }

        private void fillStep3Information(Boolean isAuthentificate)
        {
            NextButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;
            if (!isAuthentificate)
            {
                TitleStep3.Text = "Impossible de se connecter au serveur automatiquement, aide moi !";
            }
            identityConfirmBox.Text = inCreationAccount.getIdentity();
            serverConfirmBox.Text = inCreationAccount.getServer("imap");
            portConfirmBox.Text = inCreationAccount.port + "";
            if (inCreationAccount.useSSL)
            {
                SSLConfirmCheck.IsChecked = true;
            }
            else
            {
                SSLConfirmCheck.IsChecked = false;
            }
            passwordConfirmBox.Password = inCreationAccount.password;
        }

        private void CancelActionAddAccount(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void validateButtonClick(object sender, RoutedEventArgs e)
        {
            TitleStep3.Text = "Tentative de connexion ...";
            String email = identityConfirmBox.Text + "@" + serverConfirmBox.Text.Substring(serverConfirmBox.Text.IndexOf(".") + 1);
            int port = Int32.Parse(portConfirmBox.Text);
            String fullName = "ttt"; //TODO
            Boolean SSLCheck = SSLConfirmCheck.IsChecked.Value;
            String password = passwordConfirmBox.Password;
            Boolean rememberPassword = rememberPasswordBox.IsChecked.Value;

            Account newTryAccount = new Account(fullName, email, password, rememberPassword);
            newTryAccount.useSSL = SSLCheck;
            newTryAccount.port = port;
            if (tryToConnectWith(newTryAccount, "imap") == false)
            {
                showMessageCantConnect();
                TitleStep3.Text = "Impossible de se connecter au serveur automatiquement, aide moi !";
            }
            else
            {
                goNextStep();
            }
        }

        private void showMessageCantConnect()
        {
            MessageBox.Show("Impossible de se connecter à ton serveur de mail. Vérifies ces informations et tient moi au courant :)");
        }

        private void finalValidateClick(object sender, RoutedEventArgs e)
        {
            /*Save new account*/
            this.Close();
        }
    }
}
