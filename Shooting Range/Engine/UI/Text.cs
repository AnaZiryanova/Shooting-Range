using Engine.Geometry.Shapes;
using System.Drawing;
using System.Numerics;

namespace Engine.UI {
internal class Text : UIObject {
    public string text { get; set; }
    public Font font { get; private set; }
    public Brush textColor { get; private set; }

    public Text(Vector3 position, Vector2 size,
        string text = "", int fontSize = 20, EGameObjectShape shape = EGameObjectShape.rectangle)
        : base(position, size, shape) {
        this.text = text;
        font = new Font("Arial", fontSize, FontStyle.Bold);
        textColor = Brushes.Black;
    }

    public Text(Vector3 position, Vector2 size, Brush textColor,
        string text = "", int fontSize = 20, EGameObjectShape shape = EGameObjectShape.rectangle)
        : this(position, size, text, fontSize, shape) {
        this.textColor = textColor;
    }
}
}
