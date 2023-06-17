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
        TcpListener klogServer;
        TcpListener ShareScreenServer;
        Thread Listen;
        TcpClient client;
        TcpClient klogClient;
        TcpClient ShareScreenClient;




        public DashBoardForm()
        {
            server = new TcpListener(IPAddress.Any, 9000);
            klogServer = new TcpListener(IPAddress.Any, 9001);
            ShareScreenServer = new TcpListener(IPAddress.Any, 9002);


            /*server = new TcpListener(IPAddress.Any, 9003);
            klogServer = new TcpListener(IPAddress.Any, 9004);
            ShareScreenServer = new TcpListener(IPAddress.Any, 9005);*/
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
                    klogServer.Start();
                    ShareScreenServer.Start();
                    while (true)
                    {
                        client = server.AcceptTcpClient();
                        klogClient = klogServer.AcceptTcpClient();
                        ShareScreenClient = ShareScreenServer.AcceptTcpClient();

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

            ShareScreenServer.Stop();
            server.Stop();
            klogServer.Stop();
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