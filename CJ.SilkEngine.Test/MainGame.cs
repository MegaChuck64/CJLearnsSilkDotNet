using Silk.NET.Input;
using Silk.NET.Maths;
using System.Drawing;
using CJ.SilkEngine.GameObjects;

namespace CJ.SilkEngine.Test;

public class MainGame : CJGame
{
    GameObject testGo;
    float speed = 128f;
    public MainGame(int width, int height, string title, Color backgroundColor) : base(width, height, title, backgroundColor)
    {
    }


    public override void Load()
    {
        testGo = new GameObject(this, "Test GO", new Rectangle<float>(100, 100, 128f, 128f));
        var sprt = new Sprite(
            testGo, 
            Path.Combine("Assets", "Textures", "demo.png"),
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
        if (Input == null || Input.Keyboards.Count == 0)
            return;

        var keyboard = Input.Keyboards[0];
        var bounds = testGo.Bounds;

        if (keyboard.IsKeyPressed(Key.W))
        {
            bounds.Origin.Y -= speed * dt;            
        }
        if (keyboard.IsKeyPressed(Key.S))
        {
            bounds.Origin.Y += speed * dt;
        }
        if (keyboard.IsKeyPressed(Key.A))
        {
            bounds.Origin.X -= speed * dt;
        }
        if (keyboard.IsKeyPressed(Key.D))
        {
            bounds.Origin.X += speed * dt;
        }

        testGo.Bounds = bounds;
        
    }
    
    public override void Draw(float dt)
    {
    }

    public override void Close()
    {
    }

}