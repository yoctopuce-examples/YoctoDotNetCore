using System;

namespace YoctoCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using Yoctopuce lib " + YAPI.GetAPIVersion());
            string errsmg = "";
            if (YAPI.RegisterHub("usb", ref errsmg) != YAPI.SUCCESS)
            {
                Console.WriteLine("Unable to register the USB port :" + errsmg);
                return;
            }

            YSensor sensor = YSensor.FirstSensor();
            if (sensor == null)
            {
                Console.WriteLine("No Yoctopuce sensor find on USB.");
                return;
            }

            YDisplay display = YDisplay.FirstDisplay();
            if (display == null)
            {
                Console.WriteLine("No Yoctopuce display find on USB.");
                return;
            }

            // display clean up
            display.resetAll();

            YDisplayLayer l1 = display.get_displayLayer(1);
            //YDisplayLayer l2 = display.get_displayLayer(2);
            l1.hide();    // L1 is hidden, l2 stay visible
            int w = display.get_displayWidth();
            int h = display.get_displayHeight();


            while (sensor.isOnline() && display.isOnline())
            {
                string value = sensor.get_currentValue() + " " + sensor.get_unit();
                string name = sensor.get_friendlyName();

                // display a text in the middle of the screen
                l1.clear();
                l1.selectFont("Large.yfm");
                l1.drawText(w / 2, h / 2, YDisplayLayer.ALIGN.CENTER, value);
                l1.selectFont("Small.yfm");
                l1.drawText(w - 1, h - 1, YDisplayLayer.ALIGN.BOTTOM_RIGHT, name);
                display.swapLayerContent(0, 1);
                Console.WriteLine(name + " ->" + value);
                YAPI.Sleep(500,ref errsmg);
            }
            YAPI.FreeAPI();
        }
    }
}
