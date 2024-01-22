using System.Windows.Forms;

namespace Engine.GameInfo {
    internal class GameContext {
    public readonly int targetsCount;
    public readonly float speed;
    public EGameState state { get => _state;
                              set {
                                  _state = value;
                                  if (value == EGameState.gameplay)
                                      StartTimers();
                                  else if (value == EGameState.postGameplay)
                                      StopTimers();
                              }
                            }
    public float deltaTime => _updateTimer.Interval;

    private EGameState _state;
    private Timer _updateTimer, _countdownTimer;
    private float _playTime, _timePassed = 0;

    private GameContext(int targetsCount, float speed, float time, Timer updateTimer, Timer countdownTimer) {
        this.targetsCount = targetsCount;
        this.speed = speed;
        _playTime = time;
        _updateTimer = updateTimer;
        _countdownTimer = countdownTimer;
        state = EGameState.preGameplay;
    }

    /// <summary>
    /// Reads game context values from input. If some values are skipped, they are considered as 0.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>Newly created GameContext</returns>
    public static GameContext ReadGameContext(string[] input, Timer updateTimer, Timer countdownTimer) {
        if (input == null)
            input = new string[0];
        int targetsCount = 1;
        float time = 10, speed = 50;
        for (int i = 0; i < input.Length; ++i) {
            int param = int.Parse(input[i].Substring(input[i].IndexOf('=') + 1));
            if (input[i].StartsWith(InputDefinitions.targetsCount))
                targetsCount = param;
            else if (input[i].StartsWith(InputDefinitions.projectileSpeed))
                speed = param * 0.001f * updateTimer.Interval;
            else if (input[i].StartsWith(InputDefinitions.playTime))
                time = param * 1000f / countdownTimer.Interval;
        }
        return new GameContext(targetsCount, speed, time, updateTimer, countdownTimer);
    }

    public bool HandleTimer() {
        ++_timePassed;
        if (_timePassed >= _playTime)
            return true;
        return false;
    }

    private void StartTimers() {
        _updateTimer.Start();
        _countdownTimer.Start();
    }

    private void StopTimers() {
        _updateTimer.Stop();
        _countdownTimer.Stop();
        _timePassed = 0;
    }
}
}
