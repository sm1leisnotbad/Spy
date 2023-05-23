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
        TcpListener ShareScreen;
        Thread Listen;
        TcpClient client;
        TcpClient klogClient;
        TcpClient ShareScreenClient;




        public DashBoardForm()
        {
            server = new TcpListener(IPAddress.Any, 8000);
            klog = new TcpListener(IPAddress.Any, 8001);
            ShareScreen = new TcpListener(IPAddress.Any, 8002);


            CheckForIllegalCrossThreadCalls = false;

            Listen = new Thread(ListenFunc);
            Listen.IsBackground = true;
            Listen.Start();

            InitializeComponent();
        }



        void ListenFunc()
        {
            try
            { 
                    server.Start();
                    klog.Start();
                    ShareScreen.Start();
                    while (true)
                    {
                        client = server.AcceptTcpClient();
                        klogClient = klog.AcceptTcpClient();
                        ShareScreenClient = ShareScreen.AcceptTcpClient();

                        ConnectBx.Text += "Connected to " + client.Client.RemoteEndPoint.ToString() + "\r\n";
                        Thread t = new Thread(ClientThread);
                        t.IsBackground = true;
                        t.Start();
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Listen end.");
                StopListen();
                MessageBox.Show(ex.Message);
            }
        }

        void StopListen()
        {

            ShareScreen.Stop();
            server.Stop();
            klog.Stop();
            if (Listen.IsAlive)
              Listen.Abort();
        }

        void ClientThread()
        {
            try
            {
                Form manageForm = new ManageForm(klogClient, ShareScreenClient, client);
                manageForm.Show();
                Application.Run();
            }

            catch (Exception e)
            {
                MessageBox.Show("Manage Form end.");
                MessageBox.Show(e.Message);
                Application.Exit();
            }


        }
        private void DashBoardForm_Load(object sender, EventArgs e)
        {
            

        }
    }
}