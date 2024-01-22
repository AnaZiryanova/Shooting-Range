using Engine.Geometry.Shapes;
using Engine.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.Graphics {
internal class GameObjectRenderer {
    public void RenderGameObjects(System.Drawing.Graphics g, IEnumerable<GameObject> gameObjects) {
        if (gameObjects == null)
            return;
        gameObjects.OrderByDescending(x => x.position.Z);
        for (int i = 0; i < gameObjects.Count(); ++i) {
            GameObject obj = gameObjects.ElementAt(i);
            if (!obj.active)
                continue;
            RenderShape(g, obj.shape);
            if (obj is ProgressBar bar)
                RenderShape(g, bar.fill);
            else if (obj is Button button)
                RenderText(g, button.text);
            else if (obj is Text text)
                RenderText(g, text);
        }
    }

    private void RenderText(System.Drawing.Graphics g, Text text) {
        g.DrawString(text.text, text.font, text.textColor,
                     new PointF(text.position.X - text.size.X / 2, text.position.Y - text.size.Y / 2));
    }

    private void RenderShape(System.Drawing.Graphics g, Shape shape) {
        if (shape is Geometry.Shapes.Rectangle)
            RenderRectangleGameObject(g, shape);
        else
            RenderEllipseGameObject(g, shape);
    }

    private void RenderRectangleGameObject(System.Drawing.Graphics g, Shape shape) {
        Rotate(g, shape);
        g.FillRectangle(shape.color, shape.GetRectangle());
        g.ResetTransform();
    }

    private void RenderEllipseGameObject(System.Drawing.Graphics g, Shape shape) {
        Rotate(g, shape);
        g.FillEllipse(shape.color, shape.GetRectangle());
        g.ResetTransform();
    }

    private void Rotate(System.Drawing.Graphics g, Shape shape) {
        if (shape.zRotation != 0) {
            g.TranslateTransform(shape.position.X, shape.position.Y);
            g.RotateTransform(shape.zRotation);
            g.TranslateTransform(-shape.position.X, -shape.position.Y);
        }
    }
}
}
