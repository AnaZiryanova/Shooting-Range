using System;
using System.Drawing;
using System.Numerics;

namespace Engine.Geometry.Shapes {
internal abstract class Shape {
    public Vector3 position { get => _position;
                              set {
                                  _position = value;
                                  size = new Vector2(_zeroZRelativeSize.X - _position.Z, _zeroZRelativeSize.Y - _position.Z);
                                  if (size.X < 0 || size.Y < 0)
                                    size = Vector2.Zero;
                              }
                            }
    public float zRotation { get; set; }
    public Vector2 size { get; private set; }
    public Brush color { get; set; }

    private Vector3 _position;
    private Vector2 _zeroZRelativeSize;

    public Shape(Vector3 position, Vector2 size) {
        SetSize(size);
        _position = position;
    }

    public void SetSize(Vector2 size) {
        _zeroZRelativeSize = new Vector2(Math.Abs(size.X) - Math.Abs(position.Z),
                                         Math.Abs(size.Y) - Math.Abs(position.Z));
        this.size = size;
    }

    public bool IsIntersectingOrOnBorder(Shape other) {
        bool intersecting = false;
        if (other is Rectangle rect)
            intersecting = IsIntersectingOrOnBorderInternal(rect);
        else if (other is Ellipse el)
            intersecting = IsIntersectingOrOnBorderInternal(el);
        return intersecting;
    }

    public System.Drawing.Rectangle GetRectangle()
        => new System.Drawing.Rectangle((int)(position.X - (int)size.X / 2),
                                        (int)(position.Y - (int)size.Y / 2),
                                        (int)size.X, (int)size.Y);

    protected abstract bool IsIntersectingOrOnBorderInternal(Rectangle other);
    protected abstract bool IsIntersectingOrOnBorderInternal(Ellipse other);
}
}
