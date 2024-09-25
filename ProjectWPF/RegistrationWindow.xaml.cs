using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using ProjectWPF;

namespace ProjectWPF
{

    public partial class RegistrationWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private string _captchaText;

        public RegistrationWindow()
        {
            InitializeComponent();
            GenerateAndDisplayCaptcha();
        }
        private void GenerateAndDisplayCaptcha()
        {
            _captchaText = CaptchaGenerator.GenerateCaptcha();
            CaptchaTextBlock.Text = _captchaText;
        }


        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (CaptchaTextBox.Text != _captchaText)
            {
                MessageBox.Show("Неверная капча.");
                GenerateAndDisplayCaptcha();
                return;
            }
            var registerData = new
            {
                username = RegisterLoginTextBox.Text,
                email = EmailTextBox.Text,
                password = RegisterPasswordBox.Password
            };

            string json = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://localhost:7135/api/Auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Регистрация успешна!");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка регистрации: {responseContent}");
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                MessageBox.Show($"Ошибка сети: {httpRequestException.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
            }
        }
    }
}
