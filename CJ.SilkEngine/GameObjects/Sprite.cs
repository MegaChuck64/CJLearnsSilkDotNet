
using CJ.SilkEngine.Graphics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.IO;
using System.Numerics;

namespace CJ.SilkEngine.GameObjects;

public class Sprite : Component
{
    public VertexArrayObject<float, uint> Vao { get; private set; }
    public Graphics.Texture Texture { get; private set; }
    public Graphics.Shader Shader { get; private set; }
    
    public Rectangle<float> Source { get; set; }

    public float Z { get; set; }

    private BufferObject<uint> ebo;
    private BufferObject<float> vbo;


    private float[] Vertices => 
        GetVerticesFromRectangle(
            Owner.Bounds, 
            Source, 
            new Vector2(Texture.Width, Texture.Height), 
            Owner.Game.GameWindow.Size,
            Z);


    private static readonly uint[] Indices =
    {
        0, 1, 3,
        1, 2, 3
    };

    private float[] GetVerticesFromRectangle(
        Rectangle<float> boundsRect, 
        Rectangle<float> sourceRect, 
        Vector2 sourceSize, 
        Vector2D<int> windowSize, 
        float zDepth)
    {
        float left = boundsRect.Origin.X / windowSize.X * 2 - 1;
        float right = (boundsRect.Origin.X + boundsRect.Size.X) / windowSize.X * 2 - 1;
        float top = 1 - boundsRect.Origin.Y / windowSize.Y * 2;
        float bottom = 1 - (boundsRect.Origin.Y + boundsRect.Size.Y) / windowSize.Y * 2;

        float texLeft = sourceRect.Origin.X / sourceSize.X;
        float texRight = (sourceRect.Origin.X + sourceRect.Size.X) / sourceSize.X;
        float texTop = sourceRect.Origin.Y / sourceSize.Y;
        float texBottom = (sourceRect.Origin.Y + sourceRect.Size.Y) / sourceSize.Y;

        float[] vertices = new float[]
        {
            left,   top,        zDepth,     texLeft,    texTop,
            right,  top,        zDepth,     texRight,   texTop,
            right,  bottom,     zDepth,     texRight,   texBottom,
            left,   bottom,     zDepth,     texLeft,    texBottom
        };

        return vertices;
    }

    public Sprite(GameObject owner, string path, Rectangle<float> source, float z) : base(owner, true)
    {
        if (Owner == null || Owner.Game.Context == null) 
            throw new InvalidOperationException("Cannot create sprite without a valid game context.");
        
        Source = source;
        Z = z;
        Texture = new Graphics.Texture(Owner.Game.Context, path);
        
        ebo = new BufferObject<uint>(Owner.Game.Context, Indices, BufferTargetARB.ElementArrayBuffer);
        vbo = new BufferObject<float>(Owner.Game.Context, Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(Owner.Game.Context, vbo, ebo);

        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

        Shader = new Graphics.Shader(
            Owner.Game.Context, 
            Path.Combine("Assets", "Shaders", "defaultTexture.vert"),
            Path.Combine("Assets", "Shaders", "defaultTexture.frag"));

    }

    public override void Update(float dt)
    {
        if (Owner == null || Owner.Game.Context == null) throw new Exception("Cannot update sprite without a valid game context.");
        
        vbo.Dispose(); // Dispose the old buffer object

        vbo = new BufferObject<float>(Owner.Game.Context, Vertices, BufferTargetARB.ArrayBuffer); // Create a new buffer object

        Vao.Bind();
        vbo.Bind();
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);
    }

    public unsafe override void Render()
    {
        if (Owner == null || Owner.Game.Context == null) throw new Exception("Cannot render sprite without a valid game context.");
        
        Vao.Bind();
        Shader.Use();

        Texture.Bind(TextureUnit.Texture0);
        Shader.SetUniform("uTexture0", 0);

        Owner.Game.Context.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
    }

    public override void Dispose(bool disposing)
    {
        Vao.Dispose();
        vbo.Dispose();
        ebo.Dispose();
        Shader.Dispose();
        Texture.Dispose();
    }
}