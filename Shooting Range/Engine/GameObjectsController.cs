using System;
using System.Collections.Generic;

namespace Engine {
internal class GameObjectsController : ITickDependant, ILifetimeStaged {
    public bool breakCollisionChecking { get; set; }

    private const float _zCollisionDelta = 1f;

    private List<GameObject> _gameObjects = new List<GameObject>();
    private List<GameObject> _objectsToAdd = new List<GameObject>();
    private List<GameObject> _objectsToDestroy = new List<GameObject>();

    public GameObjectsController() {
        GameObject.OnNewGameObjectCreated += AddGameObject;
        GameObject.OnGameObjectBeingDestroyed += RemoveGameObject;
    }

    public IReadOnlyCollection<GameObject> GetGameObjects() => _gameObjects.AsReadOnly();

    /// <summary>
    /// Clear all object that are pending to be added.
    /// </summary>
    public void ClearAllAddQuered() => _objectsToAdd.Clear();

    public void DestroyQueueredObjects() {
        for (int i = 0; i < _objectsToDestroy.Count; ++i)
            _gameObjects.Remove(_objectsToDestroy[i]);
        _objectsToDestroy.Clear();
    }

    public void Start() {
        for (int i = 0; i < _gameObjects.Count; ++i)
            _gameObjects[i].Start();
        UpdateObjectsContainers();
    }

    public void Update() {
        for (int i = 0; i < _gameObjects.Count; ++i)
            _gameObjects[i].Update();
        UpdateObjectsContainers();
        CheckCollisions();
        UpdateObjectsContainers();
    }

    public void PreDestroy() {
        List<GameObject> objects = new List<GameObject>(_gameObjects);
        for (int i = 0; i < objects.Count; ++i)
            objects[i].PreDestroy();
        _gameObjects.Clear();
        _objectsToAdd.Clear();
        _objectsToDestroy.Clear();
        GameObject.OnNewGameObjectCreated -= AddGameObject;
        GameObject.OnGameObjectBeingDestroyed -= RemoveGameObject;
    }

    public void Destroy() => PreDestroy();

    /// <summary>
    /// Removes game objects queued for disposal, adds game objects queued for addition.
    /// </summary>
    private void UpdateObjectsContainers() {
        for (int i = 0; i < _objectsToDestroy.Count; ++i) {
            _gameObjects.Remove(_objectsToDestroy[i]);
        }
        _objectsToDestroy.Clear();
        for (int i = 0; i < _objectsToAdd.Count; ++i) {
            _gameObjects.Add(_objectsToAdd[i]);
            _objectsToAdd[i].Start();
        }
        _objectsToAdd.Clear();
    }

    /// <summary>
    /// Adds game object to queue for addition.
    /// </summary>
    /// <param name="gameObject"></param>
    private void AddGameObject(GameObject gameObject)
        => _objectsToAdd.Add(gameObject);

    /// <summary>
    /// Adds game object to queue for disposal.
    /// </summary>
    /// <param name="gameObject"></param>
    private void RemoveGameObject(GameObject gameObject)
        => _objectsToDestroy.Add(gameObject);

    private void CheckCollisions() {
        for (int i = 1; i < _gameObjects.Count; ++i) {
            for (int j = i - 1; j >= 0; --j) {
                if (Math.Abs(_gameObjects[i].position.Z - _gameObjects[j].position.Z) < _zCollisionDelta &&
                    _gameObjects[i].shape.IsIntersectingOrOnBorder(_gameObjects[j].shape)) {
                    _gameObjects[i].ProcessCollision(_gameObjects[j]);
                    if (breakCollisionChecking) {
                        breakCollisionChecking = false;
                        return;
                    }
                    _gameObjects[j].ProcessCollision(_gameObjects[i]);
                }
            }
        }
    }
}
}
