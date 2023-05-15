
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.IO;
using System.Numerics;

namespace CJLearnsSilkDotNet;

public class Sprite : IDisposable
{
    public VertexArrayObject<float, uint> Vao { get; private set; }
    public Texture Texture { get; private set; }
    public Shader Shader { get; private set; }

    public Rectangle<float> Bounds { get; set; }
    public Rectangle<float> Source { get; set; }

    public float Z { get; set; }

    private readonly GL _gl;
    private readonly Game _game;

    private BufferObject<uint> ebo;
    private BufferObject<float> vbo;


    private float[] Vertices => 
        GetVerticesFromRectangle(
            Bounds, 
            Source, 
            new Vector2(Texture.Width, Texture.Height), 
            _game.WindowSize,
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

    public Sprite(GL gl, Game game, string path, Rectangle<float> bounds, Rectangle<float> source, float z)
    {
        _gl = gl;
        _game = game;
        Bounds = bounds;
        Source = source;
        Z = z;
        Texture = new Texture(_gl, path);
        
        ebo = new BufferObject<uint>(_gl, Indices, BufferTargetARB.ElementArrayBuffer);
        vbo = new BufferObject<float>(_gl, Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(_gl, vbo, ebo);

        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

        Shader = new Shader(
            _gl, 
            Path.Combine("Assets", "Shaders", "defaultTexture.vert"),
            Path.Combine("Assets", "Shaders", "defaultTexture.frag"));

    }

    public void Update(float dt)
    {
        vbo.Dispose(); // Dispose the old buffer object

        vbo = new BufferObject<float>(_gl, Vertices, BufferTargetARB.ArrayBuffer); // Create a new buffer object

        Vao.Bind();
        vbo.Bind();
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);
    }

    public unsafe void Draw()
    {
        Vao.Bind();
        Shader.Use();

        Texture.Bind(TextureUnit.Texture0);        
        Shader.SetUniform("uTexture0", 0);

        _gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
    }

    public void Dispose()
    {
        vbo.Dispose();
        ebo.Dispose();
        Vao.Dispose();
        Shader.Dispose();
        Texture.Dispose();

        GC.SuppressFinalize(this);
    }
}