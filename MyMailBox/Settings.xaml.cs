using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyMailBox.Models;

namespace MyMailBox
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private List<Account> listAccounts = new List<Account>();
        private MainWindow mainWindow = null;
        private Account currentAccount = null;

        public Settings()
        {
            this.InitializeComponent();
        }

        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void setNewListAccount(List<Account> listAccounts)
        {
            this.listAccounts = listAccounts;
            settingsAccountChoice();
        }

        private void AddMailAccountClick(object sender, RoutedEventArgs e)
        {
            AddMailAccount addMailAccount = new AddMailAccount(this);
            addMailAccount.Show();
        }

        public void saveNewMailAccount(Account newAccount)
        {
            listAccounts.Add(newAccount);
            addAccountIntoSettings(newAccount);
            preventNewAccountCreated(newAccount);
            /**/
        }

        private void preventAccountDelete(Account account)
        {
            if (mainWindow != null)
            {
                if (MessageBox.Show("Tu veux vraiment supprimer ce compte (" + account.getEmail() + ") ?", "Suppression du compte email", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    mainWindow.deleteAccount(account);
                    listAccounts.Remove(account);
                    currentAccount = null;
                    settingsAccountChoice();
                }
            }
        }

        private void preventNewAccountCreated(Account account)
        {
            if (mainWindow != null)
            {
                mainWindow.addNewAccount(account);
            }
        }

        private void addAccountIntoSettings(Account account)
        {
            /*
             * ID
             * FULLNAME
             * EMAIL
             * USESSL
             * PORT
             * IDENTITY
             * MAILSERVICE
             * SERVER
             * SIGNATURE
             * REMEMBER PASSWORD
             * if rememberPassword PASSWORD
             * */
            String accountProperties = String.Empty;
            accountProperties += account.getID() + "";
            accountProperties += "%" + account.fullName;
            accountProperties += "%" + account.getEmail();
            accountProperties += "%" + account.getUseSSL().ToString();
            accountProperties += "%" + account.getPort();
            accountProperties += "%" + account.getIdentity();
            accountProperties += "%" + account.getMailService();
            accountProperties += "%" + account.getServer();
            accountProperties += "%" + account.signature;
            accountProperties += "%" + account.rememberPassword.ToString();
            if (account.rememberPassword)
            {
                accountProperties += "%" + account.getPassword();
            }
            if (Properties.Settings.Default.ListAccount == null)
            {
                Properties.Settings.Default.ListAccount = new StringCollection();
            }
            Properties.Settings.Default.ListAccount.Add(accountProperties);
            Properties.Settings.Default.Save();
        }

        private void displayAccount(Account account)
        {
            currentAccount = account;
            NameAccountBlock.Text = account.fullName;
            EmailAccountBlock.Text = account.getEmail();
            ServerAccountBlock.Text = account.getServer();
            PortAccountBlock.Text = account.getPort() + "";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = 0;
            while (i < listAccounts.Count)
            {
                if (comboBoxListAccount.SelectedIndex == i)
                {
                    if (currentAccount != null && listAccounts[i].getID() == currentAccount.getID())
                    {
                        return;
                    }
                    displayAccount(this.listAccounts[i]);
                    return;
                }
                i++;
            }
        }

        private void settingsAccountChoice()
        {
            comboBoxListAccount.Items.Clear();
            foreach (Account account in this.listAccounts)
            {
                comboBoxListAccount.Items.Add(account.getEmail());
            }
            if (currentAccount == null && this.listAccounts.Count > 0)
            {
                comboBoxListAccount.SelectedIndex = 0;
            }
            else if (this.listAccounts.Count > 0)
            {
                comboBoxListAccount.SelectedIndex = listAccounts.IndexOf(currentAccount);
            }
        }

        private void UpdateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentAccount != null)
            {
                AddMailAccount addMailAccount = new AddMailAccount(this, currentAccount);
                addMailAccount.Show();
            }
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentAccount != null)
            {
                preventAccountDelete(currentAccount);
            }
        }
    }
}
