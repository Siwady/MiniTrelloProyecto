using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MiniTrello.Win8Phone
{
    public partial class board : PhoneApplicationPage
    {
        public board()
        {
            string parameter = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("parameter", out parameter))
            {
                string s=parameter;
            }
            InitializeComponent();
        }
    }
}