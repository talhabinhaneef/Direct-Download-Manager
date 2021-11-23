using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Direct_Download_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        WebClient client;

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string url = txtURL.Text;
            if (!string.IsNullOrEmpty(url))
            {
                Thread thread = new Thread(() =>
                {
                    Uri uri = new Uri(url);
                    string fileName = System.IO.Path.GetFileName(uri.AbsolutePath);
                    client.DownloadDataAsync(uri, Application.StartupPath+"/"+fileName);
                } );
                thread.Start();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
           {
               progressBar.Minimum = 0;
               double receive = double.Parse(e.BytesReceived.ToString());
               double total = double.Parse(e.TotalBytesToReceive.ToString());
               double percentage = receive / total * 100;
               lblStatus.Text = $"Downloaded {string.Format("{0:0.##}",percentage)}%";
               progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());



           }
                ));
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show(" Download Completed !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
