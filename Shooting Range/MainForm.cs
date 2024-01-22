using Engine;
using Engine.GameInfo;
using Engine.Graphics;
using ShootingRange.Core;
using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;

namespace ShootingRange {
public partial class MainForm : Form {
    internal static GameContext gameContext;
    internal static SceneContext sceneContext;

    private const int _sceneDepth = 30;
    private GameObjectsController _gameObjectsController;
    private GameObjectRenderer _renderer;
    private TargetController _targetController;
    private Cannon _cannon;
    private Engine.UI.Button _resetButton;
    private Engine.UI.Text _finalScoreText;
    private bool _gameOver = false;

    public MainForm() => InitializeComponent();

    private void MainForm_Load(object sender, EventArgs e) {
        DoubleBuffered = true;

        string inputFilePath = $"{Path.GetFullPath(@"..\..\")}\\{InputDefinitions.inputFilePath}";
        if (!File.Exists(inputFilePath)) {
            MessageBox.Show("Input file was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            return;
        }

        gameContext = GameContext.ReadGameContext(File.ReadAllLines(inputFilePath), TickTimer, CountdownTimer);
        sceneContext = new SceneContext(Width, Height, _sceneDepth);
        _renderer = new GameObjectRenderer();
        _gameObjectsController = new GameObjectsController();
        InitializeGame();
        _resetButton = new Engine.UI.Button(new Vector3(sceneContext.width / 2, sceneContext.height / 2, -(sceneContext.depth / 2 + 1)),
                                            new Vector2(270, 60), System.Drawing.Brushes.Azure, "RESTART", 40) {
            active = false
        };
        _finalScoreText = new Engine.UI.Text(new Vector3(sceneContext.width / 2, sceneContext.height / 2 - 60, -(sceneContext.depth / 2 + 1)),
                                             new Vector2(270, 60), System.Drawing.Brushes.Black, "tdfgdfgdfgdf", 40) {
            active = false
        };
        _resetButton.OnClick += RestartGame;
        _gameObjectsController.Start();
        gameContext.state = EGameState.gameplay;
        Invalidate();
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
        => _renderer.RenderGameObjects(e.Graphics, _gameObjectsController?.GetGameObjects());

    private void MainForm_MouseMove(object sender, MouseEventArgs e)
        => sceneContext.UpdateMousePosition(e.Location);

    private void MainForm_MouseDown(object sender, MouseEventArgs e)
        => sceneContext.CallMouseDownEvent();

    private void MainForm_MouseUp(object sender, MouseEventArgs e)
        => sceneContext.CallMouseUpEvent();

    private void TickTimer_Tick(object sender, EventArgs e) {
        _gameObjectsController.Update();
        if (_gameOver)
            GameOver();
        Invalidate();
    }

    private void CountdownTimer_Tick(object sender, EventArgs e) {
        _gameOver = gameContext.HandleTimer();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
        _resetButton.OnClick -= RestartGame;
        _gameObjectsController.Destroy();
    }

    private void InitializeGame() {
        _targetController = new TargetController(Vector3.Zero, Vector2.Zero, gameContext.targetsCount);
        _targetController.OnAllTargetsHit += GameOver;
        _cannon = new Cannon(new Vector3(sceneContext.width / 2,
                                         sceneContext.height - 20,
                                         -(sceneContext.depth / 2)),
                             new Vector2(50, 90),
                             gameContext.speed);
    }

    private void GameOver() {
        gameContext.state = EGameState.postGameplay;
        _targetController.OnAllTargetsHit -= GameOver;
        _finalScoreText.text = $"{gameContext.targetsCount - _targetController.targetsCount} / {gameContext.targetsCount}";
        _targetController.Destroy();
        _targetController = null;
        _cannon.Destroy();
        _cannon = null;
        _gameObjectsController.ClearAllAddQuered();
        _gameObjectsController.breakCollisionChecking = true;
        _gameObjectsController.DestroyQueueredObjects();
        _resetButton.active = true;
        _finalScoreText.active = true;
    }
    
    private void RestartGame() {
        _resetButton.active = false;
        _finalScoreText.active = false;
        InitializeGame();
        _gameObjectsController.Start();
        _gameOver = false;
        gameContext.state = EGameState.gameplay;
        Invalidate();
    }
}
}
