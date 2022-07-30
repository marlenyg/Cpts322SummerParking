using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;


namespace ParkingNew
{
    
    internal class Classes
    {
    }
    public class Slots
    {
        public int Total { get; set; }
        public Slot[] data { get; set; }
        public Slots(int size, Graphics G)
        {
            data = new Slot[size];
            for (int i = 0; i < size; i++) {
                data[i] = new Slot(i,G);
            }

        }

    }

    public class Slot
    {
        public Point[] corners { get; set; }
        Rectangle rect;
        public int spotNumber;
        public Slots slot;
        Pen blackPen = new Pen(Color.White, 1);
        SolidBrush myBrush = new SolidBrush(Color.Blue);
        SolidBrush myBrushR = new SolidBrush(Color.IndianRed);

        public Slot(int i, Graphics G)
        { 
            if (i < 6)
            {
                rect= new Rectangle(100 * i, 50, 100, 160);
                G.FillRectangle(myBrush, rect);
                G.DrawRectangle(blackPen, rect);
                DrawStringFloatFormat((i + 1).ToString(), 100 * i + 50, 150.0F, G);
            }
            else
            {
                rect = new Rectangle(100 * (i - 6), 270, 100, 160);
                G.FillRectangle(myBrush, rect);
                G.DrawRectangle(blackPen, rect);
                DrawStringFloatFormat((i + 1).ToString(), 100 * (i - 6) + 50, 300.0F,G);
            }
       


            /*
            if (i < 6)
            {
                corners[0] = new Point(1.5 * i, 0);
                corners[1] = new Point((i + 1) * 1.5, 0);
                corners[2] = new Point(1.5 * i, 2);
                corners[3] = new Point((i + 1) * 1.5, 2);
            }
            else
            {
                corners[0] = new Point(1.5 * i, 3);
                corners[1] = new Point((i + 1) * 1.5, 3);
                corners[2] = new Point(1.5 * i, 5);
                corners[3] = new Point((i + 1) * 1.5, 5);
            }*/


        }

        public void ColorR(Graphics G) {
            G.FillRectangle(myBrushR, rect);
            G.DrawRectangle(blackPen, rect);
        }
        public void ColorB(Graphics G)
        {
            G.FillRectangle(myBrush, rect);
            G.DrawRectangle(blackPen, rect);
        }

        public void DrawStringFloatFormat(String drawString, float x, float y, Graphics G)
        {

            // Create font and brush.
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);


            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            // drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            // Draw string to screen.
            G.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
        }

    }

    public class Beacon
    {
        public double D1 { get; set; }
        public double D2 { get; set; }
        public double D3 { get; set; }
        public double D4 { get; set; }
        public long Id { get; set; }
        public long Time { get; set; }

        private Point location;
        public void update(Beacon data)
        {

            D1 = data.D1;
            D2 = data.D2;
            D3 = data.D3;
            D4 = data.D4;
            Time = data.Time;
        }
        public Point getXY(Sensors s)
        {
            Point P = new Point();

            double x1 = s.data[0].location.x;
            double y1 = s.data[0].location.y;
            double x2 = s.data[1].location.x;
            double y2 = s.data[1].location.y;
            double x3 = s.data[2].location.x;
            double y3 = s.data[2].location.y;
            double x4 = s.data[3].location.x;
            double y4 = s.data[3].location.y;

            double A = 2 * x2 - 2 * x1;
            double B = 2 * y2 - 2 * y1;
            double C = Math.Pow(D1, 2) - Math.Pow(D2, 2) - Math.Pow(x1, 2) + Math.Pow(x2, 2) - Math.Pow(y1, 2) + Math.Pow(y2, 2);
            double D = 2 * x3 - 2 * x2;
            double E = 2 * y3 - 2 * y2;
            double F = Math.Pow(D2, 2) - Math.Pow(D3, 2) - Math.Pow(x2, 2) + Math.Pow(x3, 2) - Math.Pow(y2, 2) + Math.Pow(y3, 2);

            P.x = (C * E - F * B) / (E * A - B * D);
            P.y = (C * D - A * F) / (B * D - A * E);
            

            Console.WriteLine($"(x,y)=({P.x},{P.y})");
           
            
            if (P.y >= 3 && P.y <= 5)
            {
               P.z =(int) Math.Floor(P.x / 1.5) + 1;
            }
            else if (P.y >= 0 && P.y <= 2)
            {
               P.z= (int)Math.Floor(P.x / 1.5) + 6;
            }

            

            return P;
        }


    }
    public class Beacons
    {
        public int Total { get; set; }

        public Beacon[] data { get; set; }

    }

    public class Sensors
    {
        public int Total { get; set; }
        public Sensor[] data { get; set; }

        public Sensors(int size)
        {
            data = new Sensor[size];
            data[0] = new Sensor();
            data[1] = new Sensor();
            data[2] = new Sensor();
            data[3] = new Sensor();
        }
    }

    public class Sensor
    {
        public Point location { get; set; }
        public Sensor()
        {
            location = new Point();
        }
        public void setCordinates(double x, double y)
        {
            location.x = x;
            location.y = y;
        }
    }

    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public int z { get; set; }

        public Point(double x, double y) {
            x = x;
            y = y;
        }

        public Point()
        {
        }
    }

    public class Response

    {
        public bool success { get; set; }
        public int index { get; set; }
        public string message { get; set; }
        public string received { get; set; }
        public string companyId { get; set; }
        public string color { get; set; }
        public int[] infected { get; set; }
        public string key { get; set; }

    }
}
