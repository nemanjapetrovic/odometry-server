using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
namespace OdometryServer
{
    
    class Program
    {
        public static string[] kinect;
        public static double x;
        public static double y;
        public static double theta;
        public static void func()
        {
            while (true)
            {
                try
                {

                    Odometry.Connect("192.168.1.106", 15010, 100, 100);
                    while (true)
                    {
                        kinect = KinectPoller.getKinect("127.0.0.1");
                        Odometry.Work();
                        x = Odometry.current_position.x;
                        y = Odometry.current_position.y;
                        theta = Odometry.current_position.theta;
                        //Console.WriteLine("X: " + Odometry.current_position.x + "\tY: " + Odometry.current_position.y + "\tU: " + Odometry.current_position.theta * 180 / System.Math.PI);
                    }

                }
                catch (SocketException e)
                {
                    Console.WriteLine("Greska!" + e.StackTrace);
                    //throw;
                }
                Odometry.Disconnect();
            }
        }

        static void Main(string[] args)
        {

            Odometry.Init();
            Thread thread = new Thread(new ThreadStart(func));
            thread.Start();
            /*
            while (true)
            {
                try
                {
                   
                    Odometry.Connect("192.168.1.106",15010,100,100);
                    while(true)
                    {
                        
                        if (Console.ReadLine() == "1")
                        {
                            //Odometry.Work();
                            KinectPoller.getKinect("127.0.0.1");
                        }
                        Odometry.Work();
                        Console.WriteLine("X: " + Odometry.current_position.x + "\tY: " + Odometry.current_position.y + "\tU: " + Odometry.current_position.theta * 180 / System.Math.PI);
                    }
                    
                }
                catch( SocketException e)
                {
                     Console.WriteLine("Greska!" +  e.StackTrace);
                    //throw;
                }
                Odometry.Disconnect();
            }
            */
            System.Threading.Thread.Sleep(10000);
            System.IO.StreamWriter pisac = new System.IO.StreamWriter("log.txt");
            int num = 0;
            while(true)
            {
                //if (num >= 300)
                //    break;

                    Console.WriteLine("Odometry " + x + " " + y + " " + theta * 180 / System.Math.PI);
                    Console.Write("Laser 60 ");


                    pisac.WriteLine("Odometry " + x + " " + y + " " + theta);
                    pisac.Write("Laser 60 ");
                    foreach (string item in kinect)
                    {
                        Console.Write(item + " ");
                        pisac.Write(item + " ");
                    }
                pisac.WriteLine();
                num++;
                Console.WriteLine("Brojac: "  + num);
                System.Threading.Thread.Sleep(100);
            }
            pisac.Close();
            Console.Read();
        }

       
    }
}
