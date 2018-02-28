using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CompanyMailClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmailModel emailModel = new EmailModel();
        EmailsCollectionViewModel eCollection = new EmailsCollectionViewModel();
        public EmailModel EmailCurrent
        {
            get
            {
                return emailModel;
            }

            set
            {
                emailModel = value;
            }
        }

        public EmailsCollectionViewModel ECollection
        {
            get
            {
                return eCollection;
            }

            set
            {
                eCollection = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            popup_Status.DataContext = EmailCurrent;
            tab_SendEmail.DataContext = EmailCurrent;
            tab_Incoming.DataContext = ECollection;
        }

        private async void button_SendEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasErrors = false;
                Dispatcher.Invoke(() =>
                {
                    hasErrors = Validation.GetHasError(textBox_MessageFromAddress) || Validation.GetHasError(textBox_MessageToAddress);
                });
                if (hasErrors)
                    return;
                else
                {
                    var statusCode = await EmailHttpClient.SendEmail(EmailCurrent.ReturnEmail);
                    if (statusCode == System.Net.HttpStatusCode.OK)
                        EmailCurrent.Status = "Сообщение успешно отправлено!";
                    else
                        EmailCurrent.Status = "Сообщение не отправлено: " + statusCode.ToString();
                    EmailCurrent = new EmailModel();
                    popup_Status.IsOpen = true;
                }
            }
            catch (System.Net.WebException exc)
            {
                MessageBox.Show("Ошибка: " + exc.InnerException.ToString(), "Ошибка");
            }
            catch (Exception exc2)
            {
                MessageBox.Show("Ошибка: " + exc2.InnerException.ToString(), "Ошибка");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popup_Status.IsOpen = false;
        }
    }
}
