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

    public void Initialize()
    {
        GL.ClearColor(Color4.CornflowerBlue);
        GL.Enable(EnableCap.DepthTest);

        // Definir las matrices de perspectiva y vista
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(15f), (float)800 / 600, 0.1f, 100f);
        _viewMatrix = CreateViewMatrix(new Vector3(3f, 2f, 3f), Vector3.Zero, Vector3.UnitY);
        _modelMatrix = Matrix4.Identity;

        // Cargar y usar shaders
        _shaderProgram = LoadShaders("vertex_shader.glsl", "fragment_shader.glsl");
        GL.UseProgram(_shaderProgram);

        float[] vertices = {
            // Barra horizontal (superior) - Cara frontal (roja)
            -0.3f,  0.2f,  0.0f,   1.0f, 0.0f, 0.0f,  // V0 - Frente Izquierda (Rojo)
             0.3f,  0.2f,  0.0f,   1.0f, 0.0f, 0.0f,  // V1 - Frente Derecha (Rojo)
            -0.3f,  0.0f,  0.0f,   1.0f, 0.0f, 0.0f,  // V2 - Frente Izquierda abajo (Rojo)
             0.3f,  0.0f,  0.0f,   1.0f, 0.0f, 0.0f,  // V3 - Frente Derecha abajo (Rojo)

            // Barra horizontal (superior) - Cara trasera (verde)
            -0.3f,  0.2f, -0.1f,   0.0f, 1.0f, 0.0f,  // V4 - Atrás Izquierda (Verde)
             0.3f,  0.2f, -0.1f,   0.0f, 1.0f, 0.0f,  // V5 - Atrás Derecha (Verde)
            -0.3f,  0.0f, -0.1f,   0.0f, 1.0f, 0.0f,  // V6 - Atrás Izquierda abajo (Verde)
             0.3f,  0.0f, -0.1f,   0.0f, 1.0f, 0.0f,  // V7 - Atrás Derecha abajo (Verde)


            // Barra vertical (inferior) - Cara frontal (azul)
            -0.08f,  0.0f,  0.0f,  0.0f, 0.0f, 1.0f,  // V8 - Frente Izquierda (Azul)
             0.08f,  0.0f,  0.0f,  0.0f, 0.0f, 1.0f,  // V9 - Frente Derecha (Azul)
            -0.08f, -0.6f,  0.0f,  0.0f, 0.0f, 1.0f,  // V10 - Frente Izquierda abajo (Azul)
             0.08f, -0.6f,  0.0f,  0.0f, 0.0f, 1.0f,  // V11 - Frente Derecha abajo (Azul)

            // Barra vertical (inferior) - Cara trasera (amarilla)
            -0.08f,  0.0f, -0.1f,  1.0f, 1.0f, 0.0f,  // V12 - Atrás Izquierda (Amarillo)
             0.08f,  0.0f, -0.1f,  1.0f, 1.0f, 0.0f,  // V13 - Atrás Derecha (Amarillo)
            -0.08f, -0.6f, -0.1f,  1.0f, 1.0f, 0.0f,  // V14 - Atrás Izquierda abajo (Amarillo)
             0.08f, -0.6f, -0.1f,  1.0f, 1.0f, 0.0f,  // V15 - Atrás Derecha abajo (Amarillo)
        };

        uint[] indices = {
            // Barra horizontal (superior)
            0, 1, 2, 2, 1, 3, // Frente
            4, 5, 6, 6, 5, 7, // Atrás
            0, 2, 4, 4, 2, 6, // Izquierda
            1, 3, 5, 5, 3, 7, // Derecha
            0, 1, 4, 4, 1, 5, // Parte superior
            2, 3, 6, 6, 3, 7, // Parte inferior

            // Barra vertical (inferior)
            8, 9, 10, 10, 9, 11, // Frente
            12, 13, 14, 14, 13, 15, // Atrás
            8, 10, 12, 12, 10, 14, // Izquierda
            9, 11, 13, 13, 11, 15, // Derecha
            8, 9, 12, 12, 9, 13, // Parte superior
            10, 11, 14, 14, 11, 15, // Parte inferior
        };


        _vertexArray = GL.GenVertexArray();
        _vertexBuffer = GL.GenBuffer();
        _elementBuffer = GL.GenBuffer();

        GL.BindVertexArray(_vertexArray);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        //GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //GL.BindVertexArray(0);
    }

    public void Render()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_shaderProgram);

        // Enviar las matrices al shader
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projectionMatrix);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _viewMatrix);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref _modelMatrix);


        GL.BindVertexArray(_vertexArray);
        GL.DrawElements(PrimitiveType.Triangles, 6 * 12, DrawElementsType.UnsignedInt, 0); // Dibujar todas las caras
    }

    private int LoadShaders(string vertexPath, string fragmentPath)
    {
        string vertexCode = File.ReadAllText(vertexPath);
        string fragmentCode = File.ReadAllText(fragmentPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexCode);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentCode);
        GL.CompileShader(fragmentShader);

        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
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
    }

}
