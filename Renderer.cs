using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

class Renderer
{
    private int _vertexArray;
    private int _vertexBuffer;
    private int _elementBuffer;
    private int _shaderProgram;

    private Matrix4 _projectionMatrix;
    private Matrix4 _viewMatrix;
    private Matrix4 _modelMatrix;

    private int _indexCount;

    public void Initialize(Objeto figura)
    {
        GL.ClearColor(Color4.CornflowerBlue);
        GL.Enable(EnableCap.DepthTest);

        // Definir las matrices de perspectiva y vista
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(25f), (float)900/600, 0.1f, 500f);
        _viewMatrix = CreateViewMatrix(new Vector3(5f, 5f, 5f), Vector3.Zero, Vector3.UnitY);
        _modelMatrix = Matrix4.Identity;

        // Cargar y usar shaders
        _shaderProgram = LoadShaders("vertex_shader.glsl", "fragment_shader.glsl");
        GL.UseProgram(_shaderProgram);

        var vertices = figura.ObtenerVertices().ToArray();
        var indices = figura.ObtenerIndices().ToArray();

        // Almacenar la longitud de los índices
        _indexCount = indices.Length;

        // Configuración de buffers
        _vertexArray = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArray);

        _vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

        _elementBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indexCount * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        int vertexLocation = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

        // Asumiendo que el color está en otro buffer, esto se puede ajustar como sea necesario.
        //int colorLocation = GL.GetAttribLocation(_shaderProgram, "aColor");
        //GL.EnableVertexAttribArray(colorLocation);
        //GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

        GL.BindVertexArray(0); // Desvincular el VAO

    }

    public void Render(Objeto figura)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_shaderProgram);

        // Enviar las matrices al shader
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projectionMatrix);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _viewMatrix);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref _modelMatrix);

        GL.BindVertexArray(_vertexArray);
        var verticesTransformados = figura.ObtenerVerticesTransformados().ToArray();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, verticesTransformados.Length * Vector3.SizeInBytes, verticesTransformados, BufferUsageHint.StaticDraw);
        GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0); 

    }

    private int LoadShaders(string vertexPath, string fragmentPath)
    {
        string vertexCode = File.ReadAllText(vertexPath);
        string fragmentCode = File.ReadAllText(fragmentPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexCode);
        GL.CompileShader(vertexShader);
        CheckShaderCompileErrors(vertexShader, "VERTEX");

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentCode);
        GL.CompileShader(fragmentShader);
        CheckShaderCompileErrors(fragmentShader, "FRAGMENT");


        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);
        CheckShaderLinkErrors(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
    }

    private void CheckShaderCompileErrors(int shader, string type)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            Console.WriteLine($"ERROR::SHADER_COMPILATION_ERROR of type: {type}\n{infoLog}\n");
        }
    }

    private void CheckShaderLinkErrors(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(program);
            Console.WriteLine($"ERROR::PROGRAM_LINKING_ERROR of type: PROGRAM\n{infoLog}\n");
        }
    }

    private Matrix4 CreateViewMatrix(Vector3 eye, Vector3 center, Vector3 up)
    {
        Vector3 f = Vector3.Normalize(center - eye);
        Vector3 r = Vector3.Normalize(Vector3.Cross(up, f));
        Vector3 u = Vector3.Cross(f, r);

        Matrix4 viewMatrix = new Matrix4(
            new Vector4(r.X, u.X, -f.X, 0.0f),
            new Vector4(r.Y, u.Y, -f.Y, 0.0f),
            new Vector4(r.Z, u.Z, -f.Z, 0.0f),
            new Vector4(-Vector3.Dot(r, eye), -Vector3.Dot(u, eye), Vector3.Dot(f, eye), 1.0f)
        );

        return viewMatrix;
        //return Matrix4.LookAt(eye, center, up);
    }

}
