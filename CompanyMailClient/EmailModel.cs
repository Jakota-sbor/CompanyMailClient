using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CompanyMailClient
{
    public class EmailModel : DependencyObject
    {
        public static readonly DependencyProperty TextProperty =
DependencyProperty.Register("Text", typeof(string), typeof(EmailModel), new PropertyMetadata(""));
        public static readonly DependencyProperty EmailDateProperty =
DependencyProperty.Register("EmailDate", typeof(DateTime), typeof(EmailModel), new PropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty EmailToProperty =
DependencyProperty.Register("EmailTo", typeof(string), typeof(EmailModel), new PropertyMetadata(""));
        public static readonly DependencyProperty EmailFromProperty =
DependencyProperty.Register("EmailFrom", typeof(string), typeof(EmailModel), new PropertyMetadata(""));
        public static readonly DependencyProperty EmailTopicProperty =
DependencyProperty.Register("Topic", typeof(string), typeof(EmailModel), new PropertyMetadata(""));
        public static readonly DependencyProperty SendStatusProperty =
DependencyProperty.Register("Status", typeof(string), typeof(EmailModel), new PropertyMetadata(""));

        public EmailModel() { }
        public EmailModel(Email email)
        {
            this.EmailFrom = email.EmailFrom;
            this.EmailTo = email.EmailTo;
            this.Text = email.EmailText;
            this.Topic = email.EmailTopic;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public DateTime EmailDate
        {
            get { return (DateTime)GetValue(EmailDateProperty); }
            set { SetValue(EmailDateProperty, value); }
        }
        public string Status
        {
            get { return (string)GetValue(SendStatusProperty); }
            set { SetValue(SendStatusProperty, value); }
        }
        public string EmailTo
        {
            get { return (string)GetValue(EmailToProperty); }
            set { SetValue(EmailToProperty, value); }
        }
        public string EmailFrom
        {
            get { return (string)GetValue(EmailFromProperty); }
            set { SetValue(EmailFromProperty, value); }
        }
        public string Topic
        {
            get { return (string)GetValue(EmailTopicProperty); }
            set { SetValue(EmailTopicProperty, value); }
        }

        public Email ReturnEmail
        {
            get
            {
                return new Email() { EmailFrom = EmailFrom, EmailTo = EmailTo, EmailTopic = Topic, EmailText = Text };
            }
        }
    }

    public class EmailsCollectionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EmailModel> emailsCollection;
        public ObservableCollection<EmailModel> EmailsCollection
        {
            get
            {
                return emailsCollection;
            }

            set
            {
                emailsCollection = value;
                OnPropertyChanged("EmailsCollection");
            }
        }

        private EmailModel emailInbox = new EmailModel();

        private ICommand getEmailsCommand;
        public ICommand GetEmailsCommand
        {
            get
            {
                return getEmailsCommand ??
                  (getEmailsCommand = new RelayCommand(async obj =>
                  {
                      try
                      {
                      if (obj != null && !string.IsNullOrEmpty((string)obj))
                              EmailsCollection = new ObservableCollection<EmailModel>(await EmailHttpClient.GetEmails((string)obj));
                      }
                      catch (Exception e)
                      {
                          MessageBox.Show("Ошибка", e.InnerException.ToString());
                      }
                  }));
            }
        }

        public EmailModel EmailInbox
        {
            get
            {
                return emailInbox;
            }

            set
            {
                emailInbox = value;
                OnPropertyChanged("EmailInbox");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
        }
    }

    public class Email
    {
        public int Id { get; set; }
        public System.DateTime EmailTime { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTopic { get; set; }
        public string EmailText { get; set; }
    }
}
