using Engine;
using Engine.Geometry.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace ShootingRange.Core {
internal class Cannon : GameObject, IShooting {
    private const float _progressBarAnimationDelta = 0.1f,
                        _maxProjectileForceMultiplier = 6.3f,
                        _projectileMass = 0.5f;

    private Action UpdateCall;

    private float _projectileSpeed;
    private Engine.UI.ProgressBar _progressBar;
    private bool _progressBarValueIncreasing = true;
    private List<Projectile> _projectiles = new List<Projectile>();

    public Cannon(Vector3 position, Vector2 size, float projectileSpeed) : base(position, size, EGameObjectShape.ellipse) {
        color = Brushes.Black;
        _projectileSpeed = projectileSpeed;
        _progressBar = new Engine.UI.ProgressBar(new Vector3(position.X, position.Y, position.Z - 1), new Vector2(120, 20), 0, _maxProjectileForceMultiplier) {
            active = false,
            color = Brushes.LightGray,
        };
        _progressBar.fill.color = CreateProgressBarFillColor();
    }

    public void Shoot() {
        _progressBar.active = false;
        UpdateCall = null;
        float radius = size.Y * 0.9f,
              rotation = (float)(Math.PI / 180 * Math.Abs(zRotation + 90) + 135.1);
        Vector3 projectilePosition = new Vector3(position.X + radius * (float)Math.Cos(rotation),
                                                 position.Y + radius * (float)Math.Sin(rotation),
                                                 position.Z),
                direction = (projectilePosition - position) * _progressBar.value;
        direction.Z = 15;
        Projectile projectile = new Projectile(projectilePosition, new Vector2(40, 40),
                                               new ProjectileInfo() { shape = EGameObjectShape.ellipse,
                                                                      color = Brushes.Black,
                                                                      direction = direction,
                                                                      speed = _projectileSpeed,
                                                                      mass = _projectileMass,
                                                                    });
        projectile.OnHitTarget += ProcessProjectileHitTarget;
        _projectiles.Add(projectile);
        _progressBar.value = 0;
    }

    public override void Start() {
        base.Start();
        MainForm.sceneContext.OnMouseDown += PreShoot;
        MainForm.sceneContext.OnMouseUp += Shoot;
    }

    public override void Update() {
        base.Update();
        RotateTowards(new Vector2(MainForm.sceneContext.mousePosition.X, MainForm.sceneContext.mousePosition.Y));
        UpdateCall?.Invoke();
    }

    public override void PreDestroy() {
        base.PreDestroy();
        for (int i = 0; i < _projectiles.Count; ++i) {
            _projectiles[i].OnHitTarget -= ProcessProjectileHitTarget;
            _projectiles[i].Destroy();
        }
        _projectiles.Clear();
        _progressBar.Destroy();
        MainForm.sceneContext.OnMouseDown -= PreShoot;
        MainForm.sceneContext.OnMouseUp -= Shoot;
    }

    private void RotateTowards(Vector2 objPosition) {
        float angle = (float)(Math.Atan2(position.Y - objPosition.Y, position.X - objPosition.X) * 180 / Math.PI) - 90f;
        zRotation = angle;
    }

    private void ProcessProjectileHitTarget(Projectile projectile) {
        projectile.OnHitTarget -= ProcessProjectileHitTarget;
        _projectiles.Remove(projectile);
        projectile.Destroy();
    }

    private void PreShoot() {
        _progressBar.active = true;
        UpdateCall = HandleProgressBar;
    }

    private void HandleProgressBar() {
        if (_progressBarValueIncreasing) {
            if (_progressBar.value < _progressBar.max) {
                _progressBar.value += _progressBarAnimationDelta;
            } else {
                _progressBarValueIncreasing = false;
                _progressBar.value -= _progressBarAnimationDelta;
            }
        } else {
            if (_progressBar.value > _progressBar.min) {
                _progressBar.value -= _progressBarAnimationDelta;
            } else {
                _progressBarValueIncreasing = true;
                _progressBar.value += _progressBarAnimationDelta;
            }
        }
    }

    private LinearGradientBrush CreateProgressBarFillColor() {
        LinearGradientBrush fillColor = new LinearGradientBrush(_progressBar.shape.GetRectangle(),
                                                                Color.Yellow, Color.Red, 0f);
        ColorBlend blend = new ColorBlend(3) {
            Colors = new Color[3] { Color.Blue, Color.Yellow, Color.Red },
            Positions = new float[3] { 0f, 0.5f, 1f }
        };
        fillColor.InterpolationColors = blend;
        return fillColor;
    }
}
}
