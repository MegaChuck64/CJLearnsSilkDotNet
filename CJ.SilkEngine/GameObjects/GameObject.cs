using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CJ.SilkEngine.GameObjects;

public class GameObject : IDisposable
{
    private bool disposed = false;
    public string Name { get; set; }
    public Rectangle<float> Bounds { get; set; }
    public bool Enabled { get; set; }
    public List<Component> Components { get; private set; }
    
    public CJGame Game { get; private set; }

    public GameObject(CJGame game, string name, Rectangle<float> bounds, bool enabled = true)
    {
        Game = game;
        Name = name;
        Bounds = bounds;
        Enabled = enabled;
        Components = new List<Component>();
    }

    public void AddComponent(Component component)
    {
        Components.Add(component);
    }

    public T? GetComponent<T>() where T : Component
    {
        return Components.FirstOrDefault(c => c is T) as T;
    }

    public void Update(float deltaTime)
    {
        foreach (var component in Components.Where(c => c.Enabled))
        {
            component.Update(deltaTime);
        }
    }

    public void Render()
    {
        foreach (var component in Components.Where(c => c.Enabled))
        {
            component.Render();
        }
    }

    public void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                foreach (var component in Components)
                {
                    component.Dispose();
                }
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}