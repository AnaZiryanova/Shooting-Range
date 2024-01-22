using System;
using System.Numerics;

namespace Engine.Geometry.Shapes {
internal class Ellipse : Shape {
    public Ellipse(Vector3 position, Vector2 size) : base(position, size) {}

    public bool IsPointInsideOrOnEllipse(Vector2 point)
        => (Math.Pow(point.X - position.X, 2) / Math.Pow(Math.Max(size.X, size.Y), 2)) +
           (Math.Pow(point.Y - position.Y, 2) / Math.Pow(Math.Min(size.X, size.Y), 2)) <= 1;

    public bool IsRectangleIntersectingEllipse(System.Drawing.Rectangle rectangle)
        => IsPointInsideOrOnEllipse(new Vector2 (rectangle.X, rectangle.Y)) ||
           IsPointInsideOrOnEllipse(new Vector2 (rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));

    protected override bool IsIntersectingOrOnBorderInternal(Rectangle other) {
        System.Drawing.Rectangle intersection = System.Drawing.Rectangle.Intersect(GetRectangle(), other.GetRectangle());
        return IsRectangleIntersectingEllipse(intersection);
    }

    protected override bool IsIntersectingOrOnBorderInternal(Ellipse other) {
        System.Drawing.Rectangle intersection = System.Drawing.Rectangle.Intersect(GetRectangle(), other.GetRectangle());
        Vector2 intersectionCenter = new Vector2(intersection.X + intersection.Width / 2, intersection.Y + intersection.Height / 2);
        return IsPointInsideOrOnEllipse(intersectionCenter) && other.IsPointInsideOrOnEllipse(intersectionCenter);
    }
}
}
