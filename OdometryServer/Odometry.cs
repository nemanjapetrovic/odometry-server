using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace OdometryServer
{
    struct position
    {
        public double x;        /* meter */
        public double y;        /* meter */
        public double theta;    /* radian (counterclockwise from x-axis) */
    };
    class Odometry
    {
        /*
        * Podesavanja za odometriju robota
        */
        private static double WHEEL_DIAMETER = 0.144;
        private static double PULSES_PER_REVOLUTION = 336;
        private static double AXLE_LENGTH = 0.465;
        /*
        * Podesavanja za server
        */
        private static int rtimeout;
        private static int delay;
        /*
        * Konstante
        */
        private static double PI = System.Math.PI;
        /*
        * Ostale private promenljive
        */
        public static position current_position;
        private static UdpClient udpClient;

        //private static double PI = System.Math.PI;
        private static  double dist_left;
        private static  double dist_right;
        private static long left_ticks;
        private static long right_ticks;
        private static double expr1;
        private static double cos_current;
        private static double sin_current;
        private static double right_minus_left;
        private static double MUL_COUNT;
        private static long greska = 75;
        private static long plevo, pdesno, levo, desno;
        //private Byte[] sendBytes;/// = Encoding.ASCII.GetBytes("init");
        
        //private Byte[] receiveBytes;
        /*
        * Inicijalizuje softver
        */
        public static void Init()
        {
            current_position.x = 0;
            current_position.y = 0;
            current_position.theta = 0;
        }
        /*
        * Konektuje se na robota
        */
        public static void Connect(string IP, int port = 15010, int timeout = 100,int delay = 500)
        {
            udpClient = new UdpClient(port);

            udpClient.Connect(IP, port);

            udpClient.Client.ReceiveTimeout = timeout;

            Odometry.rtimeout = timeout;
            Odometry.delay = delay;

            Byte[] sendBytes;
            Byte[] receiveBytes;
            Byte[] sbuf;
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            sendBytes = Encoding.ASCII.GetBytes("data");
            udpClient.Send(sendBytes, sendBytes.Length);
            sbuf = udpClient.Receive(ref RemoteIpEndPoint);
            plevo = ((((long)sbuf[8] << 24)) + (((long)sbuf[7] << 16)) + (((long)sbuf[6] << 8)) + ((long)sbuf[5])) - greska;
            pdesno = ((((long)sbuf[16] << 24)) + (((long)sbuf[15] << 16)) + (((long)sbuf[14] << 8)) + ((long)sbuf[13])) - greska;
            MUL_COUNT = PI * WHEEL_DIAMETER / PULSES_PER_REVOLUTION;

            levo = 0;
            desno = 0;

            sendBytes = Encoding.ASCII.GetBytes("data");
            udpClient.Send(sendBytes, sendBytes.Length);
            sbuf = udpClient.Receive(ref RemoteIpEndPoint);
            System.Threading.Thread.Sleep(100);
            levo = ((((long)sbuf[8] << 24)) + (((long)sbuf[7] << 16)) + (((long)sbuf[6] << 8)) + ((long)sbuf[5])) - greska;
            desno = ((((long)sbuf[16] << 24)) + (((long)sbuf[15] << 16)) + (((long)sbuf[14] << 8)) + ((long)sbuf[13])) - greska;

        }
        /*
        * Zatvara soket
        */
        public static void Disconnect()
        {
            if (udpClient != null)
                udpClient.Close();
        }
        /*
        * Prati odometriju
        */
        public static void Work()
        {



            Byte[] sendBytes;
            Byte[] receiveBytes;
            Byte[] sbuf;
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //udpClient.Send(sendBytes, sendBytes.Length);

            //IPEndPoint object will allow us to read datagrams sent from any source.


            // Blocks until a message returns on this socket from a remote host.
            // = udpClient.Receive(ref RemoteIpEndPoint);
            //string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            //Console.WriteLine("This is the message you received " + returnData.ToString());
            //Console.WriteLine("This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());

            //Console.WriteLine("pLevo: " + (plevo).ToString() + "\t= ");
            //Console.WriteLine("pDesno:" + (pdesno).ToString() + "\t= ");
            ////Console.WriteLine("UgaoL: " + ((plevo - levo) / 3.1).ToString());
            ////Console.WriteLine("UgaoD: " + (((pdesno - desno)) / 3.1).ToString());
            //Console.WriteLine("---------------------------------------------");
            //Console.WriteLine("mLevo: " + (plevo - levo).ToString() + "\t= " + ((plevo - levo) * 1.34 / 1000).ToString() + "m");
            //Console.WriteLine("mDesno:" + (pdesno - desno).ToString() + "\t= " + ((pdesno - desno) * 1.34 / 1000).ToString() + "m");
            //Console.WriteLine("mUgaoL: " + ((plevo - levo) / 3.1).ToString());
            //Console.WriteLine("mUgaoD: " + (((pdesno - desno)) / 3.1).ToString());

            /* while (true)
             {*/
            sendBytes = Encoding.ASCII.GetBytes("data");
                udpClient.Send(sendBytes, sendBytes.Length);
                sbuf = udpClient.Receive(ref RemoteIpEndPoint);
                System.Threading.Thread.Sleep(100);
                levo = ((((long)sbuf[8] << 24)) + (((long)sbuf[7] << 16)) + (((long)sbuf[6] << 8)) + ((long)sbuf[5])) - greska;
                desno = ((((long)sbuf[16] << 24)) + (((long)sbuf[15] << 16)) + (((long)sbuf[14] << 8)) + ((long)sbuf[13])) - greska;
                //Console.WriteLine("Levo: " + (levo).ToString());
                //Console.WriteLine("Desno:" + (desno).ToString());
                //stavljamo trenutni kao pocetni
                //plevo = lLevo;
                // = lDesno;
                //180995 - 153718
                //returnData = Encoding.ASCII.GetString(receiveBytes);
                //Console.Clear();
                //Console.WriteLine(String.Format("X:{0} Y:{1} theta:{2}", current_position.x, current_position.y, current_position.theta * 180 / PI));

                left_ticks = plevo - levo;
                right_ticks = pdesno - desno;
                plevo = levo;
                pdesno = desno;

                //Console.WriteLine("ticks L:" + left_ticks);
                //Console.WriteLine("ticks R:" + right_ticks);
                dist_left = (double)left_ticks * MUL_COUNT;
                dist_right = (double)right_ticks * MUL_COUNT;
                //Console.WriteLine("distance L:" + dist_left);
                //Console.WriteLine("distance R:" + dist_right);

                cos_current = System.Math.Cos(current_position.theta);
                sin_current = System.Math.Sin(current_position.theta);

                current_position.x = System.Math.Round(current_position.x, 3);
                current_position.y = System.Math.Round(current_position.y, 3);

                if (System.Math.Abs(left_ticks - right_ticks) <= 10)
                {
                    /* Moving in a straight line */
                    current_position.x += System.Math.Round(dist_left * cos_current, 3);
                    current_position.y += System.Math.Round(dist_left * sin_current, 3);
                }
                else
                {
                    /* Moving in an arc */
                    expr1 = AXLE_LENGTH * (dist_right + dist_left) / 2.0 / (dist_right - dist_left);
                    right_minus_left = dist_right - dist_left;
                    current_position.x += System.Math.Round(expr1 * (System.Math.Sin(right_minus_left / AXLE_LENGTH + current_position.theta) - sin_current), 3);
                    current_position.y -= System.Math.Round(expr1 * (System.Math.Cos(right_minus_left / AXLE_LENGTH + current_position.theta) - cos_current), 3);
                    /* Calculate new orientation */
                    current_position.theta += right_minus_left / AXLE_LENGTH;
                    /*Keep in the range -PI to +PI */
                    
                    while (current_position.theta > PI)
                        current_position.theta -= (2.0 * PI);
                    while (current_position.theta < -PI)
                        current_position.theta += (2.0 * PI);

                    /* while (current_position.theta > 2*PI)
                         current_position.theta = 0;
                     while (current_position.theta < 0)
                         current_position.theta = 2.0 * PI;*/
                }

                System.Threading.Thread.Sleep(delay);
            //}
        }
    }
}
