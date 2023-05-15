using System;

namespace CJ.SilkEngine.GameObjects;

public abstract class Component : IDisposable
{
    public GameObject? Owner { get; private set; }
    public bool Enabled { get; set; } 
    
    public Component(GameObject owner, bool enabled = true)
    {
        Owner = owner;
        Enabled = enabled;
    }
    
    public virtual void Update(float deltaTime) { }
    public virtual void Render() { }

    public virtual void Dispose(bool disposing) { }


    ~Component()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
