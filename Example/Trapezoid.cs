using ClipperLib;
using System.Drawing;

namespace IntersectionOverUnion
{
    public class Trapezoid(PointF p1, PointF p2, PointF p3, PointF p4)
    {
        public PointF TopLeft { get; } = p1;
        public PointF TopRight { get; } = p2;
        public PointF BottomLeft { get; } = p3;
        public PointF BottomRight { get; } = p4;

        public static float Area(Trapezoid trapezoid)
        {
            return Area(trapezoid.TopLeft, trapezoid.TopRight, trapezoid.BottomLeft, trapezoid.BottomRight);
        }

        public static float Area(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            return 0.5f * Math.Abs(p1.X * p2.Y + p2.X * p3.Y + p3.X * p4.Y + p4.X * p1.Y - (p2.X * p1.Y + p3.X * p2.Y + p4.X * p3.Y + p1.X * p4.Y));
        }

        public static float IntersectionArea(Trapezoid t1, Trapezoid t2)
        {
            var factor = 10_000f; // use for accuracy

            var polygon1 = new List<IntPoint> {
                new IntPoint((int)(t1.TopLeft.X * factor), (int)(t1.TopLeft.Y * factor)),
                new IntPoint((int)(t1.TopRight.X * factor), (int)(t1.TopRight.Y * factor)),
                new IntPoint((int)(t1.BottomLeft.X * factor), (int)(t1.BottomLeft.Y * factor)),
                new IntPoint((int)(t1.BottomRight.X * factor), (int)(t1.BottomRight.Y * factor)),
            };

            var polygon2 = new List<IntPoint> {
                new IntPoint((int)(t2.TopLeft.X * factor), (int)(t2.TopLeft.Y * factor)),
                new IntPoint((int)(t2.TopRight.X * factor), (int)(t2.TopRight.Y * factor)),
                new IntPoint((int)(t2.BottomLeft.X * factor), (int)(t2.BottomLeft.Y * factor)),
                new IntPoint((int)(t2.BottomRight.X * factor), (int)(t2.BottomRight.Y * factor)),
            };

            var clipper = new Clipper();
            clipper.AddPolygon(polygon1, PolyType.ptSubject);
            clipper.AddPolygon(polygon2, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctIntersection, solution);

            float intersectionArea = 0;

            foreach (var polygon in solution)
            {
                intersectionArea += Area(
                    new PointF(polygon[0].X / factor, polygon[0].Y / factor),
                    new PointF(polygon[1].X / factor, polygon[1].Y / factor),
                    new PointF(polygon[2].X / factor, polygon[2].Y / factor),
                    polygon.Count > 3 ? new PointF(polygon[3].X / factor, polygon[3].Y / factor) : new PointF(0, 0)
                );
            }

            return intersectionArea;
        }

        public static float CalculateIoU(Trapezoid t1, Trapezoid t2)
        {
            float areaT1 = Area(t1);
            float areaT2 = Area(t2);

            float intersectionArea = IntersectionArea(t1, t2);

            float unionArea = areaT1 + areaT2 - intersectionArea;
            if (unionArea == 0) return 0;

            return intersectionArea / unionArea;
        }
    }
}
