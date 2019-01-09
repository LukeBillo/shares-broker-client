using System.Windows;
using SharesBrokerClient.Data;
using SharesBrokerClient.Services;

namespace SharesBrokerClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ConnectionService _connectionService;

        public LoginWindow()
        {
            InitializeComponent();
            _connectionService = ConnectionService.Instance;
        }

        private void ShowErrorMessage(string message)
        {
            ErrorMessage.Content = message;
            ErrorMessage.Width = double.NaN;
            ErrorMessage.HorizontalContentAlignment = HorizontalAlignment.Center;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // reset the error message visibility
            ErrorMessage.Visibility = Visibility.Hidden;

            var username = usernameTextBox.Text;
            var password = passwordBox.Password;

            if (username == string.Empty || password == string.Empty)
            {
                ShowErrorMessage("You must enter a username and password.");
                return;
            }

            var connectionStatus = await _connectionService.Connect(username, password);

            if (connectionStatus != ConnectionState.Connected)
            {
                switch (connectionStatus)
                {
                    case ConnectionState.Forbidden:
                    case ConnectionState.Unauthorized:
                        ShowErrorMessage("The username or password was incorrect.");
                        return;
                    default:
                        ShowErrorMessage("An error has occurred. Please try again later.");
                        return;
                }
            }

            var sharesWindow = new SharesWindow();
            Application.Current.MainWindow = sharesWindow;
            Close();
            sharesWindow.Show();
        }
    }
}
