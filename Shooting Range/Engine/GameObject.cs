using Engine.Geometry.Shapes;
using System;
using System.Drawing;
using System.Numerics;

namespace Engine {
internal class GameObject : ITickDependant, ILifetimeStaged {
    public static event Action<GameObject> OnNewGameObjectCreated { add => OnNewGameObjectCreatedInternal += value;
                                                                    remove => OnNewGameObjectCreatedInternal -= value; }
    /// <summary>
    /// Called before game object will be destroyed.
    /// </summary>
    public static event Action<GameObject> OnGameObjectBeingDestroyed { add => OnGameObjectBeingDestroyedInternal += value;
                                                                        remove => OnGameObjectBeingDestroyedInternal -= value; }
    public Vector3 position { get => shape.position; set => shape.position = value; }
    public float zRotation { get => shape.zRotation; set => shape.zRotation = value; }
    public Vector2 size { get => shape.size; }
    public Brush color { get => shape.color; set => shape.color = value; }
    public Shape shape { get; set; }
    public virtual bool active { get; set; }

    private static event Action<GameObject> OnNewGameObjectCreatedInternal;
    private static event Action<GameObject> OnGameObjectBeingDestroyedInternal;

    public GameObject(Vector3 position, Vector2 size, EGameObjectShape shape = EGameObjectShape.rectangle) {
        if (shape == EGameObjectShape.rectangle)
            this.shape = new Geometry.Shapes.Rectangle(position, size);
        else
            this.shape = new Ellipse(position, size);
        color = Brushes.Transparent;
        active = true;
        OnNewGameObjectCreatedInternal?.Invoke(this);
    }

    public GameObject(Vector3 position, Vector2 size, Brush color, EGameObjectShape shape = EGameObjectShape.rectangle) 
        : this(position, size, shape) {
        this.color = color;
    }

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void PreDestroy() { }

    public void Destroy() {
        PreDestroy();
        OnGameObjectBeingDestroyedInternal?.Invoke(this);
    }

    /// <summary>
    /// Called when gameObject entered collision.
    /// </summary>
    /// <param name="collided">Object, current one collided with.</param>
    public virtual void ProcessCollision(GameObject collided) { }
}
}
