using SampleDeviceDriver.DeviceCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceProxy proxy = new DeviceProxy();

            Console.WriteLine("sending...");
            proxy.Send("http://localhost:55930","Door2", "blue");
            Console.WriteLine("sent.");
        }
    }
}
