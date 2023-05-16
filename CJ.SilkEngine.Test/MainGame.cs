using Silk.NET.Input;
using Silk.NET.Maths;
using System.Drawing;
using CJ.SilkEngine.GameObjects;

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

}