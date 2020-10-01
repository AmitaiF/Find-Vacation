using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BL;
using BE;

namespace PLWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for MoneyStatsWindow.xaml
    /// </summary>
    public partial class MoneyStatsWindow : Window
    {
        IBL bl;
        List<Host> hosts;

        public MoneyStatsWindow()
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            hosts = bl.GetHosts();

            double TotalDebt = 0;

            foreach (var host in hosts)
                TotalDebt += GetDebt(host);

            if (ContainsEnglish(Header.Content.ToString()))
                MoneyEarned.Text = "You Earned " + TotalDebt + "$.";
            else
                MoneyEarned.Text = "." + TotalDebt + "$ הרווחת";
        }

        private bool ContainsEnglish(string text)
        {
            foreach (var c in text)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    return true;
            return false;
        }

        private double GetDebt(Host host)
        {
            double debt = 0;
            foreach (var hu in bl.GetHostingUnitsOfHost(host))
                debt += hu.DebtToAdmin;
            return debt;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window Window = new AdminMainWindow();
            Window.Show();
            Close();
        }

        private void Log_off_Click(object sender, RoutedEventArgs e)
        {
            Window LoginWindow = new Login();
            LoginWindow.Show();
            Close();
        }
    }
}
