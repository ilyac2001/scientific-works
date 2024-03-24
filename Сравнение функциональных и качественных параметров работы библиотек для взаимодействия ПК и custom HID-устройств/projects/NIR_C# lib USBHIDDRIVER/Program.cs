using System;
using System.Diagnostics;
using USBHIDDRIVER;

namespace NIR
{
    internal class Program
    {
        public static int i = 0;
        public static Stopwatch time = new Stopwatch();
        public static Stopwatch time_w = new Stopwatch();
        public static Stopwatch time_r = new Stopwatch();
        static void MyEventRead(object sender, EventArgs e)
        {
            if (i < 1000)
            {
                USBHIDDRIVER.List.ListWithEvent EventPayload = (USBHIDDRIVER.List.ListWithEvent)sender;
                byte[] buf = (byte[])EventPayload[EventPayload.Count - 1];
                Console.Out.WriteLine("Reading: " + i++ + " : " + buf[0] + " " + buf[1]);
            }
            else { time_r.Stop(); }
        }
        static void Main(string[] args)
        {            
            Console.WriteLine("Run: C#, USBHIDDRIVER");

            time.Start();
            USBHIDDRIVER.USBInterface host_usb = new USBInterface("vid_0483", "pid_5750");
            byte[] buf = { 0x00, 0x00 };            
            USBHIDDRIVER.USB.HIDUSBDevice.OutputReport myOutputReport = new USBHIDDRIVER.USB.HIDUSBDevice.OutputReport();

            Console.WriteLine("Opening");
            host_usb.Connect();
            if (host_usb.isConnected)
            {
                Console.WriteLine("Device open!");
            }
            else {
                Console.WriteLine("Could not open device!");
            }

            time_r.Start();
            buf[0] = 0x01;
            host_usb.enableUsbBufferEvent(MyEventRead);
            host_usb.startRead();

            time_w.Start();            
            buf[0] = 0x02;
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("Writing " + i + " : " + buf[0] + " " + buf[1]);             
                if(!myOutputReport.Write(buf, host_usb.usbdevice.myUSB.HidHandle))
                {
                    Console.WriteLine("Write failed!");
                }
            }
            time_w.Stop();            

            Console.WriteLine("Closing");
            if (host_usb.isConnected)
            {
                try
                {
                    host_usb.stopRead();
                    time_r.Stop();
                    host_usb.Disconnect();
                }
                catch { }
            }            
            
            time.Stop();
            Console.WriteLine("Runtime = " + time.Elapsed);
            Console.WriteLine("Runtime read = " + time_r.Elapsed);
            Console.WriteLine("Runtime write = " + time_w.Elapsed);
        }
    }
}
