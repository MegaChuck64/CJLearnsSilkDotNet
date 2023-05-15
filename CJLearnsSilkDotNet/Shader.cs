using Silk.NET.OpenGL;
using System;
using System.IO;

namespace CJLearnsSilkDotNet;

public class Shader : IDisposable
{
    private uint _handle;
    private GL _gl;

    public Shader(GL gl, string vertexPath, string fragmentPath)
    {
        _gl = gl;

        var vertex = LoadShader(ShaderType.VertexShader, vertexPath);
        var fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
        _handle = gl.CreateProgram();
        gl.AttachShader(_handle, vertex);
        gl.AttachShader(_handle, fragment);
        gl.LinkProgram(_handle);
        gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
        if (status == 0)
            throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");

        gl.DetachShader(_handle, vertex);
        gl.DetachShader(_handle, fragment);
        gl.DeleteShader(vertex);
        gl.DeleteShader(fragment);
    }

    public void Use() => _gl.UseProgram(_handle);
    
    public void SetUniform(string name, float value)
    {
        var location = _gl.GetUniformLocation(_handle, name);
        
        if (location == -1)
            throw new Exception($"Could not find uniform {name}");
        
        _gl.Uniform1(location, value);
    }
    public void SetUniform(string name, int value)
    {
        var location = _gl.GetUniformLocation(_handle, name);

        if (location == -1)
            throw new Exception($"Could not find uniform {name}");

        _gl.Uniform1(location, value);
    }

    private uint LoadShader (ShaderType type, string path)
    {
        var src = File.ReadAllText(path);
        var handle = _gl.CreateShader(type);
        _gl.ShaderSource(handle, src);
        _gl.CompileShader(handle);
        var infoLog = _gl.GetShaderInfoLog(handle);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader ({type}): {infoLog}");
        }

        return handle;
    }

    public void Dispose()
    {
        _gl.DeleteProgram(_handle);
    }

}