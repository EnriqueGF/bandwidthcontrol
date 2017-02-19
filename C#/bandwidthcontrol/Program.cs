using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;
using System.Net.Security;
using System.Net;
using System.Threading;

namespace ConsoleApplication1
{
    public static class Globals
    {
        public static int contador = 0;
        public static bool si = false;
        public static string usuario;
        public static string iplocal;
        public static string archivosruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string ids;
    }

    class Program
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private NetworkInterface[] nicArr;
        private NetworkInterface nic;
        private double ElapsedTime, BytesSent = 0, BytesReceived = 0;
        private long Freq, Tic = 0, Toc, Run = -1;
        private string FN_ini = Path.Combine(Directory.GetCurrentDirectory(), "config.ini");
        private int chart_x_interval = 20;



        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            UpdateNetworkInterface();

        }

        private void UpdateNetworkInterface()
        {
            try
            {
                Run += 1;

                IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();

                QueryPerformanceCounter(out Toc);
                ElapsedTime = (double)(Toc - Tic) / Freq;
                QueryPerformanceCounter(out Tic);
                double PreviousBytesSent = BytesSent;
                double PreviousBytesReceived = BytesReceived;
                BytesSent = interfaceStats.BytesSent;
                BytesReceived = interfaceStats.BytesReceived;

                if (Run > 0)
                {
                    int bytesSentSpeed = (int)((double)(BytesSent - PreviousBytesSent) / ElapsedTime / 1024);
                    int bytesReceivedSpeed = (int)((double)(BytesReceived - PreviousBytesReceived) / ElapsedTime / 1024);
                    Console.WriteLine("Subida: " + (bytesSentSpeed).ToString() + " / " + "Bajada: " + (bytesReceivedSpeed).ToString() + " KBytes/s");


                    //ENVIAR PETICIÓN
                    string URI = "http://localhost/controlarbandwidth/datos.php";
                    string myParameters = "id=" + Globals.ids +  "&nombre=" + Globals.usuario + "&ip=" + Globals.iplocal + "&usosubida=" + bytesSentSpeed + "&usobajada=" + bytesReceivedSpeed;
                    WebClient wc = new WebClient();
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers[HttpRequestHeader.UserAgent] = "5FT7NY54O9T4FY5OFT9NY";
                    wc.Headers.Add("Content-Encoding", "gzip");
                    string html = wc.UploadString(URI, myParameters); // Codigo HTML resultante (debug)
                    Console.WriteLine("Peticion realizada: " + myParameters);
                    //
                }
            } catch (Exception excepcion) { Console.WriteLine("Excepcion producida en el bucle: " + excepcion); }

        }
        public static void Main()
        {

            Program foo = new Program();
            foo.Principal();


        }

        private void Principal()
        {
       
          Globals.usuario = Environment.UserName;

       // GENERAR ID
            if (System.IO.File.Exists(Globals.archivosruta + "/idtemp") == false)
            {
                //GENERAMOS NUMERO RANDOM PARA EL ID
                var chars = "0123456789";
                var stringChars = new char[12];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var idrandom = new String(stringChars);

                //ESCRIBIRMOS EN UN ARCHIVO EL NUMERO ID
                File.WriteAllText(Globals.archivosruta + "/idtemp", idrandom);
                // Hacer invisible archivo id
                FileAttributes attributes = File.GetAttributes(Globals.archivosruta + @"\idtemp");
                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {

                }
                else
                {
                    // Ocultar archivo en caso de no estarlo
                    File.SetAttributes(Globals.archivosruta + @"\idtemp", File.GetAttributes(Globals.archivosruta + @"\idtemp") | FileAttributes.Hidden);

                }
            }

            try { Globals.ids = File.ReadAllText(Globals.archivosruta + "/idtemp"); } catch (Exception qwe) { Console.WriteLine(qwe); }

            //
            nicArr = NetworkInterface.GetAllNetworkInterfaces();
            List<string> myCollection = new List<string>();

            foreach (NetworkInterface interfaz in nicArr)
            {
                myCollection.Add(Globals.contador.ToString() + " " + "no");

                if (interfaz.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || interfaz.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in interfaz.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.ToString().Contains("192.168.1"))
                        {
                            //  Console.WriteLine(ip.Address.ToString());
                            myCollection.Remove(Globals.contador.ToString() + " " + "no");
                            myCollection.Add(Globals.contador.ToString() + " " + "si" + " " + ip.Address.ToString());

                        }

                    }
                }
                Globals.contador = Globals.contador + 1;
            }
            if (myCollection[0] == "0 si")
            {
                nic = nicArr[0];
            }
            else if (myCollection[1].Contains("si")) {
                nic = nicArr[1];
                string[] cortado = myCollection[1].Split(' ');
                Globals.iplocal = cortado[2];
            }
            else if (myCollection[2].Contains("si")) {
                nic = nicArr[2];
                string[] cortado = myCollection[2].Split(' ');
                Globals.iplocal = cortado[2];
            }
            else if (myCollection[3].Contains("si")) {
                nic = nicArr[3];
                string[] cortado = myCollection[3].Split(' ');
                Globals.iplocal = cortado[2];

            }
            else if (myCollection[4].Contains("si")) {
                nic = nicArr[4];
                string[] cortado = myCollection[4].Split(' ');
                Globals.iplocal = cortado[2];
            }
            else if (myCollection[5].Contains("si")) {
                nic = nicArr[5];
                string[] cortado = myCollection[5].Split(' ');
                Globals.iplocal = cortado[2];
            }

            // DEBUG

            // DEBUG COMPROBACION ADAPTADORES
            //Console.WriteLine(myCollection[0]);
            //Console.WriteLine(myCollection[1]);
             //Console.WriteLine(myCollection[2]);
            //Console.WriteLine(myCollection[3]);
            // Console.WriteLine(myCollection[4]);
            //Console.WriteLine(myCollection[5]);

            if (QueryPerformanceFrequency(out Freq) == false)
                throw new Win32Exception();

            // Timer
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 2000;
            aTimer.Enabled = true;

            //

            while ("a" == "a") { Thread.Sleep(500); }


        }


    }
}
