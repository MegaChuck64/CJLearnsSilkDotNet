using Silk.NET.Input;
using CJ.SilkEngine.GameObjects;

namespace CJ.SilkEngine.Test;

public class PlayerController : Component
{
    public float Speed { get; set; }
    public PlayerController(GameObject owner, float speed, bool enabled = true) : base(owner, enabled)
    {
        Speed = speed;
    }

    public override void Update(float dt)
    {
        if (Owner?.Game.Input == null || Owner.Game.Input.Keyboards.Count == 0)
            return;

        var keyboard = Owner?.Game.Input.Keyboards[0];
        var bounds = Owner.Bounds;

        if (keyboard.IsKeyPressed(Key.W))
        {
            bounds.Origin.Y -= Speed * dt;
        }
        if (keyboard.IsKeyPressed(Key.S))
        {
            bounds.Origin.Y += Speed * dt;
        }
        if (keyboard.IsKeyPressed(Key.A))
        {
            bounds.Origin.X -= Speed * dt;
        }
        if (keyboard.IsKeyPressed(Key.D))
        {
            bounds.Origin.X += Speed * dt;
        }

        Owner.Bounds = bounds;
    }
}