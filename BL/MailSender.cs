using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace BL
{
    public class MailSender
    {
        BackgroundWorker bgWorker;
        bool MailSucceeded = false;
        int orderKey;

        public MailSender()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += Bg_DoWork;
            bgWorker.RunWorkerCompleted += Bg_RunWorkerCompleted;
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            MailMessage mail = e.Argument as MailMessage;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("noreply.VacationSystem@gmail.com", "b,iutnh,h");
            smtp.EnableSsl = true;
            while (!MailSucceeded)
            {
                try
                {
                    smtp.Send(mail);
                    MailSucceeded = true;
                }
                catch
                {

                }
            }
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (orderKey != 0)
                SingletonFactoryBL.GetBL().MailSent(orderKey);
        }

        public void SendMail(MailMessage mail, int orderKey)
        {
            bgWorker.RunWorkerAsync(mail);
            this.orderKey = orderKey;
        }
    }
}
