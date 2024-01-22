using System.Numerics;

namespace Engine.Geometry.Shapes {
internal class Rectangle : Shape {
    public Rectangle(Vector3 position, Vector2 size) : base(position, size) {}

    protected override bool IsIntersectingOrOnBorderInternal(Rectangle other) {
        return !System.Drawing.Rectangle.Intersect(GetRectangle(), other.GetRectangle()).IsEmpty;
    }

    protected override bool IsIntersectingOrOnBorderInternal(Ellipse other) {
        System.Drawing.Rectangle intersection = System.Drawing.Rectangle.Intersect(GetRectangle(), other.GetRectangle());
        return other.IsRectangleIntersectingEllipse(intersection);
    }
}
}
