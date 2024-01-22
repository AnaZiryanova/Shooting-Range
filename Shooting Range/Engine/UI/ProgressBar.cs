using Engine.Geometry.Shapes;
using System.Drawing;
using System.Numerics;

namespace Engine.UI {
internal class ProgressBar : UIObject {
    public Vector2 borderThickness { get; set; }
    public float min { get; set; }
    public float max { get; set; }
    public float value { get => _value;
                         set {
                             _value = value;
                             fill.SetSize(new Vector2(_value * _stepMultiplier, fill.size.Y));
                             fill.position = new Vector3(borderThickness.X + (position.X - size.X / 2) + fill.size.X / 2,
                                                         borderThickness.Y + fill.position.Y, fill.position.Z);
                         }
                       }
    public Shape fill { get; private set; }

    private float _value;
    private float _stepMultiplier = 1;

    public ProgressBar(Vector3 position, Vector2 size, float min = 0, float max = 1,
                       float startValue = 0, EGameObjectShape shape = EGameObjectShape.rectangle)
        : base(position, size, shape) {
        fill = new Geometry.Shapes.Rectangle(new Vector3(position.X, position.Y, position.Z - 1),
                                             new Vector2(startValue, size.Y - borderThickness.Y));
        color = Brushes.DarkGray;
        fill.color = Brushes.Gray;
        this.min = min;
        this.max = max;
        value = startValue;
        _stepMultiplier = size.X / (max - min);
    }

    public ProgressBar(Vector3 position, Vector2 size, Vector2 borderThickness, float min = 0, float max = 1,
                       float startValue = 0, EGameObjectShape shape = EGameObjectShape.rectangle)
        : this(position, size, min, max, startValue, shape) {
        this.borderThickness = borderThickness;
    }
}
}
