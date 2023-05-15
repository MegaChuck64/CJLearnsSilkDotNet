using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;


namespace CJ.SilkEngine.Graphics;


public unsafe class Texture : IDisposable
{
    private uint _handle;
    private GL _gl;
    
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public Texture(GL gl, string path)
    {
        _gl = gl;

        _handle = _gl.GenTexture();
        Bind();

        using var img = Image.Load<Rgba32>(path);
        Width = img.Width;
        Height = img.Height;
        
        gl.TexImage2D(
            TextureTarget.Texture2D, 
            0, 
            InternalFormat.Rgba8, 
            (uint)img.Width, 
            (uint)img.Height, 
            0, 
            PixelFormat.Rgba, 
            PixelType.UnsignedByte, 
            null);

        img.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                fixed (void* data = accessor.GetRowSpan(y))
                {
                    gl.TexSubImage2D(
                        TextureTarget.Texture2D, 
                        0,
                        0,
                        y,
                        (uint)accessor.Width,
                        1,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        data);
                    
                }
            }
        });

        SetParameters();
    }
    
    private void SetParameters()
    {
        //Setting some texture perameters so the texture behaves as expected.
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
        //handle texture alpha
        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        
        
        //Generating mipmaps.
        _gl.GenerateMipmap(TextureTarget.Texture2D);
    }
    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(textureSlot);
        _gl.BindTexture(TextureTarget.Texture2D, _handle);
    }

    public void Dispose()
    {
        _gl.DeleteTexture(_handle);
        GC.SuppressFinalize(this);
    }


}