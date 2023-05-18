using CJ.SilkEngine.GameObjects;
using Silk.NET.Maths;

namespace CJ.SilkEngine.Test.Prefabs;

public class PlayerPrefab : IPrefab
{
    public GameObject Instantiate(CJGame game)
    {
        var player = new GameObject(game, "Player", new Rectangle<float>(0, 0, 128, 128));
        player.AddComponent(new Sprite(player, Path.Combine("Assets", "Textures", "demo.png"), new Rectangle<float>(0, 0, 64, 64), 0));
        player.AddComponent(new PlayerController(player, 128f));
        return player;
    }
}