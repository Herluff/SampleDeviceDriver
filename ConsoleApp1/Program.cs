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
            string stateInput = "off";
            string content = "{'state'  : '" + stateInput + "  '}";
            content = content.Replace('\'', '"');
            Console.WriteLine(content);

            var kvs = content.Split(new char[] { '{', '\'', '"', ' ', ':','}' }, StringSplitOptions.RemoveEmptyEntries);
            string state = kvs[1];

            Console.WriteLine(state);

            
            DeviceProxy proxy = new DeviceProxy();

            Console.WriteLine("sending...");
            proxy.Send("http://localhost:5050/","Door2", "blue");
            Console.WriteLine("sent.");

            string got = proxy.Get("http://localhost:5050/", "Door2");
            Console.WriteLine(got);

        }
    }
}
