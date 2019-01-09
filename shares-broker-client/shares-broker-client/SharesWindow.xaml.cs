using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SharesBrokerClient.Data.Models;
using SharesBrokerClient.Services;

namespace SharesBrokerClient
{
    /// <summary>
    /// Interaction logic for SharesWindow.xaml
    /// </summary>
    public partial class SharesWindow : Window
    {
        private readonly SharesService _sharesService = SharesService.Instance;

        public SharesWindow()
        {
            InitializeComponent();
        }

        private void SharesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var companyShares = _sharesService.GetShares().Select(share => new CompanyShareViewModel(share));
            SharesGrid.ItemsSource = companyShares;
        }
    }
}
