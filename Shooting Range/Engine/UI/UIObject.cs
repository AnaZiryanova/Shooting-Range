using Engine.Geometry.Shapes;
using System.Drawing;
using System.Numerics;

namespace Engine.UI {
internal class UIObject : GameObject {
    public UIObject(Vector3 position, Vector2 size, EGameObjectShape shape = EGameObjectShape.rectangle)
        : base(position, size, shape) { }

    public UIObject(Vector3 position, Vector2 size, Brush color, EGameObjectShape shape = EGameObjectShape.rectangle)
        : this(position, size, shape) {
        this.color = color;
    }
}
}
