using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ProjectWPF
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private string _captchaText;

        public MainWindow()
        {
            InitializeComponent();
            GenerateAndDisplayCaptcha();
        }

        private void GenerateAndDisplayCaptcha()
        {
            _captchaText = CaptchaGenerator.GenerateCaptcha();
            CaptchaTextBlock.Text = _captchaText;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (CaptchaTextBox.Text != _captchaText)
            {
                MessageBox.Show("Неверная капча.");
                GenerateAndDisplayCaptcha();
                return;
            }

            var loginData = new
            {
                username = LoginTextBox.Text,
                password = PasswordBox.Password
            };

            string json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://localhost:7135/api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Авторизация успешна!");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка авторизации: {responseContent}");
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

        private void OpenRegisterWindow(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
