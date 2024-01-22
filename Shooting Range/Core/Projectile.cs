using Engine;
using Engine.Geometry.Shapes;
using System;
using System.Drawing;
using System.Numerics;

namespace ShootingRange.Core {
internal struct ProjectileInfo {
    public EGameObjectShape shape;
    public Brush color;
    public Vector3 direction;
    public float speed;
    public float mass;
}

internal class Projectile : GameObject {
    public event Action<Projectile> OnHitTarget;

    private const float _impulsePeakDelta = 0.1f;
    private const float _impulseSpeedMultiplier = 20f;

    private Vector3 _direction;
    private float _speed;
    private float _mass;
    private bool _reachedImpulsePeak = false;

    public Projectile(Vector3 position, Vector2 size, ProjectileInfo info) : base(position, size, info.shape) {
        color = info.color;
        _direction = info.direction;
        _speed = info.speed;
        _mass = info.mass;
    }

    public override void Update() {
        base.Update();
        Move();
    }

    public override void ProcessCollision(GameObject collided) {
        base.ProcessCollision(collided);
        if (collided is Target target) {
            target.ProcessProjectileHit();
            OnHitTarget?.Invoke(this);
        }
    }

    private void Move() {
        if (!_reachedImpulsePeak && _direction.X <= _impulsePeakDelta &&
            _direction.Y <= _impulsePeakDelta && _direction.Z <= _impulsePeakDelta)
            _reachedImpulsePeak = true;
        if (_reachedImpulsePeak) {
            color = Brushes.DimGray;
            _direction += new Vector3(_mass * 0.5f, _mass * 0.5f, _mass);
            _direction *= _speed;
        } else {
            _direction *= (_speed * _impulseSpeedMultiplier / MainForm.gameContext.deltaTime * _mass);
        }
        position += _direction;
        if (!IsVisibileOnScreen())
        Destroy();
    }

    private bool IsVisibileOnScreen() {
        int halfXsize = (int)size.X / 2,
            halfYsize = (int)size.Y / 2;
        bool overflowedX = position.X + halfXsize < 0 || position.X - halfXsize > MainForm.sceneContext.width,
             overflowedY = position.Y + halfYsize < 0 || position.Y - halfYsize > MainForm.sceneContext.height,
             overflowedZ = position.Z < -MainForm.sceneContext.depth || position.Z > MainForm.sceneContext.depth;
        if (overflowedX || overflowedY || overflowedZ)
            return false;
        return true;
    }
}
}
