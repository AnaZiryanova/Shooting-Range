using Engine.Geometry.Shapes;
using ShootingRange;
using System;
using System.Drawing;
using System.Numerics;

namespace Engine.UI {
internal class Button : UIObject {
    public event Action OnClick { add => OnClickInternal += value;
                                  remove => OnClickInternal -= value; }
    public Text text { get; set; }
    public override bool active { get => base.active;
                                  set {
                                      base.active = value;
                                      if (text != null)
                                          text.active = value; }
                                }

    private event Action OnClickInternal;
    private bool _press = false;

    public Button(Vector3 position, Vector2 size, Brush color,
        string text = "", int fontSize = 20, EGameObjectShape shape = EGameObjectShape.rectangle)
        : base(position, size, color, shape) {
        this.text = new Text(position, size, text, fontSize);
    }

    public override void Start() {
        base.Start();
        MainForm.sceneContext.OnMouseDown += PressStart;
        MainForm.sceneContext.OnMouseUp += PressEnd;
    }

    public override void PreDestroy() {
        base.PreDestroy();
        text.Destroy();
        MainForm.sceneContext.OnMouseDown -= PressStart;
        MainForm.sceneContext.OnMouseUp -= PressEnd;
    }

    private void PressStart() {
        if (active &&
            shape.GetRectangle().Contains(MainForm.sceneContext.mousePosition.X,
                                          MainForm.sceneContext.mousePosition.Y))
            _press = true;
    }

    private void PressEnd() {
        if (_press) {
            OnClickInternal?.Invoke();
            _press = false;
        }
    }
}
}
