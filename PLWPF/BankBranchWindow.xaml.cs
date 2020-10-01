using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for BankWindow.xaml
    /// </summary>
    public partial class BankBranchWindow : Window
    {
        BackgroundWorker bg = new BackgroundWorker();
        HttpWebRequest httpRequest;
        HostRegistrationWindow window;
        public BankBranch bankBranch;

        public BankBranchWindow(Window window)
        {
            InitializeComponent();
            
            comboBoxBanks.ItemsSource = null;
            this.window = window as HostRegistrationWindow;

            bg.DoWork += Bg_DoWork;
            bg.RunWorkerCompleted += Bg_RunWorkerCompleted;
            bg.ProgressChanged += Bg_ProgressChanged;
            bg.WorkerReportsProgress = true;
        }

        private void Bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = e.Result as DataTable;
            DataView dv = dt.DefaultView;
            banksDatagrid.DataContext = dv;
            var banknames = dv.ToTable(true, "Bank_Name").AsEnumerable();

            comboBoxBanks.ItemsSource = banknames.Select(dr => dr["Bank_Name"].ToString().Trim()).Distinct();
            comboBoxBanks.SelectedIndex = 0;
            populateBanksBtn.IsEnabled = true;
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            bg.ReportProgress(10);

            // Construct HTTP request to get the file
            string uriString = (String)e.Argument;
            httpRequest = (HttpWebRequest)WebRequest.Create(uriString);
            httpRequest.Method = WebRequestMethods.Http.Get;

            bg.ReportProgress(30);

            // Get back the HTTP response for web server
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Stream httpResponseStream = httpResponse.GetResponseStream();

            bg.ReportProgress(40);

            // Construct the Datset and read the data from the stream
            DataSet ds = new DataSet();
            ds.ReadXml(httpResponseStream);

            bg.ReportProgress(90);

            DataTable dt = ds.Tables[0];
            e.Result = dt;

            bg.ReportProgress(100);
        }

        private void populateBanksBtn_Click(object sender, RoutedEventArgs e)
        {
            populateBanksBtn.IsEnabled = false;
            bg.RunWorkerAsync(@"https://www.boi.org.il/en/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_en.xml");
        }

        private void comboBoxBanks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxBanks.ItemsSource != null)
            {
                string str = comboBoxBanks.SelectedItem as String;

                foreach (DataRowView drv in (DataView)banksDatagrid.ItemsSource)
                {
                    if (drv["Bank_Name"].ToString() == str)
                    {
                        // This is the data row view record you want...
                        banksDatagrid.SelectedItem = drv;
                        banksDatagrid.ScrollIntoView(drv);
                        break;
                    }
                }
            }
        }

        private void banksDatagrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            window.bankBranch = new BankBranch();
            var data = banksDatagrid.SelectedItem as DataRowView;
            if (data == null) ;
            else
            {
                window.bankBranch.BankName = data["Bank_Name"].ToString();
                window.bankBranch.BankNumber = int.Parse(data["Bank_Code"].ToString());
                window.bankBranch.BranchAddress = data["Address"].ToString();
                window.bankBranch.BranchCity = data["City"].ToString();
                window.bankBranch.BranchNumber = int.Parse(data["Branch_Code"].ToString());
                Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}