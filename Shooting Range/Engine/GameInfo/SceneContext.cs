using System;
using System.Drawing;

namespace Engine.GameInfo {
internal class SceneContext {
    public event Action OnMouseDown { add => OnMouseDownInternal += value; remove => OnMouseDownInternal -= value; }
    public event Action OnMouseUp { add => OnMouseUpInternal += value; remove => OnMouseUpInternal -= value; }
    public int width { get; }
    public int height { get; }
    public int depth { get; }
    public Point mousePosition { get; private set; }

    private event Action OnMouseDownInternal,
                         OnMouseUpInternal;

    private const int _xDelta = 15;
    private const int _yDelta = 38;

    public SceneContext(int width, int height, int depth) {
        this.width = width - _xDelta;
        this.height = height - _yDelta;
        this.depth = depth;
    }

    public void UpdateMousePosition(Point mousePosition) => this.mousePosition = mousePosition;
    public void CallMouseDownEvent() => OnMouseDownInternal?.Invoke();
    public void CallMouseUpEvent() => OnMouseUpInternal?.Invoke();
}
}
