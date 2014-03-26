using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RestSharp;

namespace MiniTrello.Win8Phone
{
    public partial class login : PhoneApplicationPage
    {

        public login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/organization.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            /*var client = new RestClient("fsd");
                var request = new RestRequest("/login", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(loginModel);*/
            NavigationService.Navigate(new Uri("/register.xaml", UriKind.Relative));
        }
    }


}