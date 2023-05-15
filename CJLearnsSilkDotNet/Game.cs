using System.IO;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace CJLearnsSilkDotNet;

public class Game
{
    private readonly IWindow window;
    private IInputContext? input;
    private GL? gl;

    private Sprite? spriteA;
    private Sprite? spriteB;
    
    private FPSCounter? fpsCounter;

    public Game(int width, int height, string title)
    {
        window = Window.Create(WindowOptions.Default with
        {
            Size = new(width, height),
            Title = title,
            FramesPerSecond = 60,
            WindowBorder = WindowBorder.Fixed,
            TransparentFramebuffer = true
        });
        window.Load += OnLoad;
        window.Render += OnRender;
        window.Update += OnUpdate;
        window.Closing += OnClose;
    }

    public void Run() => window.Run();

    public Vector2D<int> WindowSize => window.Size;

    private void OnLoad()
    {
        input = window.CreateInput();

        for (int i = 0; i < input.Keyboards.Count; i++)
        {
            input.Keyboards[i].KeyDown += OnKeyDown;
        }

        gl = window.CreateOpenGL();
        gl.ClearColor(System.Drawing.Color.Orange);

        spriteA = new Sprite(
            gl, 
            this, 
            Path.Combine("Assets", "Textures", "demo.png"), 
            new Rectangle<float>(0f, 0f, 256, 256f), 
            new Rectangle<float>(0f, 0f, 64f, 64f),
            0.1f);
        
        spriteB = new Sprite(
            gl,
            this,
            Path.Combine("Assets", "Textures", "demo.png"),
            new Rectangle<float>(64f, 0f, 64f, 64f),
            new Rectangle<float>(64f, 0f, 64f, 64f),
            0.2f);


        fpsCounter = new FPSCounter();
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
        {
            window.Close();
        }
    }

    private void OnRender(double delta)
    {
        gl?.Clear(ClearBufferMask.ColorBufferBit);
        
        spriteA?.Draw();
        spriteB?.Draw();
    }



    private void OnUpdate(double delta)
    {
        fpsCounter?.Update((float)delta);
        window.Title = $"CJ's Game Window - FPS: {fpsCounter?.FramesPerSecond ?? 0:0.0}";
        spriteA?.Update((float)delta);
        spriteB?.Update((float)delta);

        var speed = 128f * (float)delta;
        var rect = new Rectangle<float>()
        {
            Origin = spriteB?.Bounds.Origin ?? new Vector2D<float>(),
            Size = spriteB?.Bounds.Size ?? new Vector2D<float>()
        };
        if (input?.Keyboards[0].IsKeyPressed(Key.D) ?? false)
        {
            if (rect.Origin.X + rect.Size.X < WindowSize.X)
            {
                rect.Origin.X += speed;
            }
        }
        if (input?.Keyboards[0].IsKeyPressed(Key.A) ?? false)
        {
            if (rect.Origin.X > 0)
            {
                rect.Origin.X -= speed;
            }
        }
        if (input?.Keyboards[0].IsKeyPressed(Key.W) ?? false)
        {
            if (rect.Origin.Y > 0)
            {
                rect.Origin.Y -= speed;
            }
        }
        if (input?.Keyboards[0].IsKeyPressed(Key.S) ?? false)
        {
            if (rect.Origin.Y + rect.Size.Y < WindowSize.Y)
            {
                rect.Origin.Y += speed;
            }
        }

        if (spriteB != null)
        {
            spriteB.Bounds = rect;
        }

    }

    private void OnClose()
    {     
        gl?.Dispose();
        input?.Dispose();
        spriteA?.Dispose();
    }

}
