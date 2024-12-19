using IntersectionOverUnion;
using System.Drawing;

class Program
{
    static void Main()
    {
        Trapezoid trapezoid1 = new Trapezoid(new PointF(0, 0), new PointF(2, 0), new PointF(3, 1), new PointF(1, 1));
        Trapezoid trapezoid2 = new Trapezoid(new PointF(1, 0), new PointF(3, 0), new PointF(4, 1), new PointF(2, 1));

        float iou = Trapezoid.CalculateIoU(trapezoid1, trapezoid2);
        Console.WriteLine("Intersection over Union: " + iou);
    }
}
