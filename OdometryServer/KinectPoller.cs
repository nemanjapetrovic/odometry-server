using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace OdometryServer
{
    class KinectPoller
    {
        /*
         * Podesavanja za server
         */

        private static int rtimeout;
        private static int delay;
        private static UdpClient udpClient;

        public static string [] getKinect(string IP, int port = 15040, int timeout = 10000, int delay = 500)
        {
             try
            { 
                    udpClient = new UdpClient(); 

                    udpClient.Connect(IP, port);

                    udpClient.Client.ReceiveTimeout = timeout;

                    rtimeout = timeout;
                    KinectPoller.delay = delay;

                    Byte[] sendBytes;
                    Byte[] receiveBytes;
                    Byte[] sbuf;
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    sendBytes = Encoding.ASCII.GetBytes("kinect");
                    udpClient.Send(sendBytes, sendBytes.Length);
                    sbuf = udpClient.Receive(ref RemoteIpEndPoint);
                
                    string result = System.Text.Encoding.UTF8.GetString(sbuf);
                    string [] x = result.Split('\n');

                    return x;
                    
                //foreach(var item in sbuf) Console.WriteLine(item);


            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
    
        }
    }
}
