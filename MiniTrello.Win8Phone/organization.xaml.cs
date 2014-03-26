using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using RestSharp;

namespace MiniTrello.Win8Phone
{
    public partial class organization : PhoneApplicationPage
    {
        public organization()
        {
            var client = new RestClient("http://minitrelloapis.apphb.com");
            var request = new RestRequest("/organizations/"+App.Token, Method.PUT);
            request.RequestFormat = DataFormat.Json;
            var asyncHandler = client.ExecuteAsync<ReturnOrganizationsModel>(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.Data != null)
                    {
                        App.ListOrganizations = (IList<Organization>) r.Data.Organizations;
                        //NavigationService.Navigate(new Uri("/organization.xaml", UriKind.Relative));
                    }
                }
            });
            InitializeComponent();
        }
    }
}