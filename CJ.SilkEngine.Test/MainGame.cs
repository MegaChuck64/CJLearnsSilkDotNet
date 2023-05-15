using Silk.NET.Input;
using Silk.NET.Maths;
using System.Drawing;
using CJ.SilkEngine.GameObjects;
using Silk.NET.OpenGL;

namespace CJ.SilkEngine.Test;

public class MainGame : CJGame
{
    GameObject testGo;
    public MainGame(int width, int height, string title, Color backgroundColor) : base(width, height, title, backgroundColor)
    {
    }


    public override void Load()
    {
        testGo = new GameObject(this, "Test GO", new Rectangle<float>(100, 100, 128f, 128f));
        var sprt = new Sprite(
            testGo, 
            Path.Combine("Assets", "Textures", "demo.png"), 
            new Rectangle<float>(0f, 0f, 128f, 128f),
            new Rectangle<float>(0f, 0f, 64f, 64f),
            0.1f);

        testGo.AddComponent(sprt);
        GameObjects.Add(testGo);
    }
    public override void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
    }

    public override void Update(float dt)
    {
        if (Input.Keyboards[0].IsKeyPressed(Key.D))
        {
            if (testGo.Components.First(t => t is Sprite) is Sprite spr)
                spr.Bounds = new Rectangle<float>(spr.Bounds.Origin.X + 10 * dt, spr.Bounds.Origin.Y, spr.Bounds.Size);
        }
        
        if (Input.Keyboards[0].IsKeyPressed(Key.A))
        {
            if (testGo.Components.First(t => t is Sprite) is Sprite spr)
                spr.Bounds = new Rectangle<float>(spr.Bounds.Origin.X - 10 * dt, spr.Bounds.Origin.Y, spr.Bounds.Size);
        }

        if (Input.Keyboards[0].IsKeyPressed(Key.W))
        {
            if (testGo.Components.First(t => t is Sprite) is Sprite spr)
                spr.Bounds = new Rectangle<float>(spr.Bounds.Origin.X, spr.Bounds.Origin.Y - 10 * dt, spr.Bounds.Size);
        }

        if (Input.Keyboards[0].IsKeyPressed(Key.S))
        {
            if (testGo.Components.First(t => t is Sprite) is Sprite spr)
                spr.Bounds = new Rectangle<float>(spr.Bounds.Origin.X, spr.Bounds.Origin.Y + 10 * dt, spr.Bounds.Size);
        }
        
    }
    
    public override void Draw(float dt)
    {
    }

    public override void Close()
    {
    }

}