using Engine;
using Engine.Geometry.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ShootingRange.Core {
internal class TargetController : GameObject {
    public event Action OnAllTargetsHit;
    public int targetsCount => _targets.Count;

    private const float _targetRadius = 35;
    private static Random _random = new Random();

    private List<Target> _targets = new List<Target>();

    public TargetController(Vector3 position, Vector2 size, int targetsCount, EGameObjectShape shape = EGameObjectShape.rectangle)
        : base(position, size, shape) {
        for (int i = 0; i < targetsCount; ++i) {
            Target target = new Target(RandomizeStartPosition(), new Vector2(_targetRadius * 2, _targetRadius * 2), System.Drawing.Brushes.Yellow);
            target.OnTargetHit += ProcessTargetHit;
            _targets.Add(target);
        }
    }

    public override void PreDestroy() {
        base.PreDestroy();
        for (int i = 0; i < _targets.Count; ++i)
            _targets[i].Destroy();
        _targets.Clear();
    }

    private void ProcessTargetHit(Target target) {
        _targets.Remove(target);
        target.OnTargetHit -= ProcessTargetHit;
        target.Destroy();
        if (!_targets.Any())
            OnAllTargetsHit?.Invoke();
    }

    private Vector3 RandomizeStartPosition() {
        Vector3 position = Vector3.Zero;
        float rangeUpper = MainForm.sceneContext.width - _targetRadius,
              rangeLower = _targetRadius;
        position.X = RandomRange(rangeLower, rangeUpper);
        rangeUpper = MainForm.sceneContext.height - _targetRadius;
        position.Y = RandomRange(rangeLower, rangeUpper);
        return position;

        float RandomRange(float min, float max) {
            float result = (float)_random.NextDouble() * max;
            if (result < min)
                result = min;
            return result;
        }
    }
}
}
