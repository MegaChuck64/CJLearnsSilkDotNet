using Silk.NET.Input;
using System.Drawing;

using CJ.SilkEngine.Test.Prefabs;


namespace CJ.SilkEngine.Test;

public class MainGame : CJGame
{
    public MainGame(int width, int height, string title, Color backgroundColor) : base(width, height, title, backgroundColor)
    {
    }


    public override void Load()
    {
        var player = new PlayerPrefab().Instantiate(this);
        GameObjects.Add(player);
    }
    public override void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            GameWindow.Close();
    }

    public override void Update(float dt)
    {
        
    }
    
    public override void Draw(float dt)
    {
    }

    public override void Close()
    {
    }

    public static uint UintFromColor(Color col) =>
        (uint)((col.R) | (col.G << 8) | (col.B << 16) | (col.A << 24));
    

}