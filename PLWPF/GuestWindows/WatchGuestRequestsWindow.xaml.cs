using System;
using System.Collections.Generic;
using System.Windows;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for WatchGuestRequestsWindow.xaml
    /// </summary>
    public partial class WatchGuestRequestsWindow : Window
    {
        User user;

        public WatchGuestRequestsWindow(User _user)
        {
            InitializeComponent();

            user = _user;
        }
    }
}
