using Engine;
using Engine.Geometry.Shapes;
using System;
using System.Drawing;
using System.Numerics;

namespace ShootingRange.Core {
internal class Target : GameObject {
    public event Action<Target> OnTargetHit;

    private const float _zDirectionMultiplier = 0.2f;
    private const int _targetsSceneDepth = 10, _sceneDepthShift = 10;

    private static Random _random = new Random();

    private float _speed;
    private Vector3 _direction;
    private ScreenBorderTouchState _lastTouchState = new ScreenBorderTouchState();

    public Target(Vector3 startPosition, Vector2 size, Brush color) : base(startPosition, size, EGameObjectShape.ellipse) {
        float rangeUpper = 7, rangeLower = 0.5f,
              speed = (float)_random.NextDouble() * rangeUpper;
        if (speed < rangeLower)
            speed = rangeLower;
        _speed = speed;
        _direction = RandomizeDirection();
        this.color = color;
    }

    public void ProcessProjectileHit() {
        OnTargetHit?.Invoke(this);
    }

    public override void Update() {
        base.Update();
        Move();
    }

    private void Move() {
        position += _direction;
        ScreenBorderTouchState touchState = CheckBordersTouch();
        if (touchState != _lastTouchState && touchState.isTouching)
            _direction = ChangeDirection(touchState);
        _lastTouchState = touchState;
    }

    private ScreenBorderTouchState CheckBordersTouch() {
        int halfXsize = (int)size.X / 2,
            halfYsize = (int)size.Y / 2;
        ScreenBorderTouchState touchState = new ScreenBorderTouchState();

        if (position.X - halfXsize <= 0)
            touchState.leftTouched = true;
        else if (position.X + halfXsize >= MainForm.sceneContext.width)
            touchState.rightTouched = true;

        if (position.Y - halfYsize <= 0)
            touchState.upperTouched = true;
        else if (position.Y + halfYsize >= MainForm.sceneContext.height)
            touchState.lowerTouched = true;

        if (position.Z <= -(MainForm.sceneContext.depth / 2) + 1 + _sceneDepthShift)
            touchState.frontTouched = true;
        else if (position.Z >= -(MainForm.sceneContext.depth / 2) + 1 + _targetsSceneDepth + _sceneDepthShift)
            touchState.backTouched = true;

        return touchState;
    }

    private Vector3 RandomizeDirection() {
        float x = RandomizeDirectionUnit(),
              y = RandomizeDirectionUnit(),
              z = RandomizeDirectionUnit() * _zDirectionMultiplier;
        if (x == 0 && y == 0 && z == 0)
            x = _speed;
        return new Vector3(x, y, z);

        float RandomizeDirectionUnit() => _random.NextDouble() > 0.5f ? _speed : 0;
    }

    private Vector3 ChangeDirection(ScreenBorderTouchState touchState) {
        Vector3 newDirection = _direction;

        if (touchState.leftTouched)
            newDirection.X = _speed;
        else if (touchState.rightTouched)
            newDirection.X = -_speed;

        if (touchState.upperTouched)
            newDirection.Y = _speed;
        else if (touchState.lowerTouched)
            newDirection.Y = -_speed;

        if (touchState.frontTouched)
            newDirection.Z = _speed * _zDirectionMultiplier;
        else if (touchState.backTouched)
            newDirection.Z = -_speed * _zDirectionMultiplier;

        return newDirection;
    }
}
}
