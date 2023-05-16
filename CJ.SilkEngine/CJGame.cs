using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using System.Drawing;
using System;
using CJ.SilkEngine.GameObjects;
using System.Collections.Generic;

namespace CJ.SilkEngine;
public abstract class CJGame : IDisposable
{
    private bool disposedValue;

    public IWindow GameWindow { get; private set; }

    public IInputContext? Input { get; private set; }

    public GL? Context { get; private set; }

    public Color Backgroundcolor { get; private set; }

    public List<GameObject> GameObjects { get; private set; }

    public CJGame(int width, int height, string title, Color backgroundColor)
    {
        GameObjects = new List<GameObject>();

        GameWindow = Window.Create(WindowOptions.Default with
        {
            Size = new(width, height),
            Title = title,
            FramesPerSecond = 60,
            WindowBorder = WindowBorder.Fixed,
            TransparentFramebuffer = true
        });

        Backgroundcolor = backgroundColor;

        GameWindow.Load += GameWindow_Load;
        GameWindow.Update += GameWindow_Update;
        GameWindow.Render += GameWindow_Render;
        GameWindow.Closing += GameWindow_Closing;
    }

    public void Run() => GameWindow.Run();

    private void GameWindow_Load()
    {
        Input = GameWindow.CreateInput();
        for (int i = 0; i < Input.Keyboards.Count; i++)
        {
            Input.Keyboards[i].KeyDown += OnKeyDown;
        }

        Context = GameWindow.CreateOpenGL();
        Context.ClearColor(Backgroundcolor);

        Load();
    }

    private void GameWindow_Update(double dt)
    {
        foreach (var go in GameObjects)
        {
            go.Update((float)dt);
        }
        
        Update((float)dt);
    }

    private void GameWindow_Render(double dt)
    {
        Context?.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var go in GameObjects)
        {
            go.Render();
        }
        
        Draw((float)dt);
    }


    private void GameWindow_Closing()
    {
        Context?.Dispose();
        Input?.Dispose();

        foreach (var gameObject in GameObjects)
        {
            gameObject.Dispose();
        }

        GameObjects.Clear();

        
        Close();
    }


    private void OnKeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        KeyDown(keyboard, key, keyCode);
    }

    public abstract void Load();

    public abstract void Update(float dt);

    public abstract void Draw(float dt);

    public abstract void KeyDown(IKeyboard keyboard, Key key, int keyCode);

    public abstract void Close();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                GameWindow?.Dispose(); 
            }

            disposedValue = true;
        }
    }


    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
