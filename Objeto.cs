using OpenTK.Mathematics;

public class Objeto
{
    private readonly List<Vector3> vertices;
    private readonly List<Vector3> colors;
    private readonly List<uint> indices;
    private Vector3 _posicion;
    private Vector3 _centro;
    private Matrix4 _rotacion;

    public Objeto()
    {
        vertices = new List<Vector3>();
        colors = new List<Vector3>();
        indices = new List<uint>();
        _posicion = Vector3.Zero;
        _rotacion = Matrix4.Identity;
        DefinirVertices();
        DefinirColores();
        _centro = CalcularCentro();
        
    } 

    public void DefinirVertices()
    {
        // Definir los vértices de la figura en "T"
        vertices.AddRange(new Vector3[] {
        // Barra superior - Frente
        new Vector3(-0.3f, 0.2f, 0.0f),  // V0
            new Vector3(0.3f, 0.2f, 0.0f), // V1
            new Vector3(-0.3f, 0.0f, 0.0f), // V2
            new Vector3(0.3f, 0.0f, 0.0f), // V3

            // Barra superior - Atrás
            new Vector3(-0.3f, 0.2f, -0.1f), // V4
            new Vector3(0.3f, 0.2f, -0.1f), // V5
            new Vector3(-0.3f, 0.0f, -0.1f), // V6
            new Vector3(0.3f, 0.0f, -0.1f), // V7

            // Barra vertical - Frente
            new Vector3(-0.08f, 0.0f, 0.0f), // V8
            new Vector3(0.08f, 0.0f, 0.0f), // V9
            new Vector3(-0.08f, -0.6f, 0.0f), // V10
            new Vector3(0.08f, -0.6f, 0.0f), // V11

            // Barra vertical - Atrás
            new Vector3(-0.08f, 0.0f, -0.1f), // V12
            new Vector3(0.08f, 0.0f, -0.1f), // V13
            new Vector3(-0.08f, -0.6f, -0.1f), // V14 
            new Vector3(0.08f, -0.6f, -0.1f), // V15
        });

        indices.AddRange(new uint[] {
            0, 1, 2, 2, 1, 3,  // B. superior - Frente
            4, 5, 6, 6, 5, 7,  // B. superior - Atrás
            0, 2, 4, 4, 2, 6,  // B. superior - Lado izquierdo
            1, 3, 5, 5, 3, 7,  // B. superior - Lado derecho
            0, 1, 4, 4, 1, 5,  // B. superior - Superior
            2, 3, 6, 6, 3, 7,  // B. superior - Inferior
            8, 9, 10, 10, 9, 11,  // B. vertical - Frente
            12, 13, 14, 14, 13, 15,  // B. vertical - Atrás
            8, 10, 12, 12, 10, 14,  // B. vertical - Lado izquierdo
            9, 11, 13, 13, 11, 15,  // B. vertical - Lado derecho
            8, 9, 12, 12, 9, 13,  // B. vertical - Superior
            10, 11, 14, 14, 11, 15,  // B. vertical - Inferior
        });
    }

    public void DefinirColores()
    {
        // Definir un color para cada vértice
        colors.AddRange(new Vector3[] {
            new Vector3(1.0f, 0.0f, 0.0f), // Rojo
            new Vector3(0.0f, 1.0f, 0.0f), // Verde
            new Vector3(0.0f, 0.0f, 1.0f), // Azul
            new Vector3(1.0f, 1.0f, 0.0f), // Amarillo
            new Vector3(1.0f, 0.0f, 1.0f), // Magenta
            new Vector3(0.0f, 1.0f, 1.0f), // Cian
            new Vector3(1.0f, 0.5f, 0.0f), // Naranja
            new Vector3(0.5f, 0.0f, 1.0f), // Púrpura
            new Vector3(1.0f, 0.0f, 0.0f), // Rojo
            new Vector3(0.0f, 1.0f, 0.0f), // Verde
            new Vector3(0.0f, 0.0f, 1.0f), // Azul
            new Vector3(1.0f, 1.0f, 0.0f), // Amarillo
            new Vector3(1.0f, 0.0f, 1.0f), // Magenta
            new Vector3(0.0f, 1.0f, 1.0f), // Cian
            new Vector3(1.0f, 0.5f, 0.0f), // Naranja
            new Vector3(0.5f, 0.0f, 1.0f), // Púrpura
        });
    }

    private Vector3 CalcularCentro()
    {
        Vector3 suma = Vector3.Zero;
        foreach (var vertice in vertices)
        {
            suma += vertice;   
        }
        return suma / vertices.Count;
    }

    // metodo para trasladar la figura a una nueva posicion
    public void Trasladar(Vector3 desplazamiento)
    {
        //for (int i = 0; i < vertices.Count; i++)
        //{
        //    vertices[i] += desplazamiento;
        //}
        _posicion += desplazamiento;
    }

    public void Rotar(Vector3 angulos)
    {
        Matrix4 rotX = Matrix4.CreateRotationX(angulos.X);
        Matrix4 rotY = Matrix4.CreateRotationY(angulos.Y);
        Matrix4 rotZ = Matrix4.CreateRotationZ(angulos.Z);

        // Combinar las rotaciones
        _rotacion *= rotX * rotY * rotZ;
    }
    public Matrix4 ObtenerMatrizTransformacion()
    {
        return _rotacion * Matrix4.CreateTranslation(_posicion);
    }

    public List<Vector3> ObtenerVerticesTransformados()
    {
        var verticesTransformados = new List<Vector3>();
        foreach (var vertice in vertices)
        {
            // Trasladar respecto al nuevo centro y la posición
            var verticeEnCentro = vertice - _centro;
            var verticeConRotacion = Vector3.TransformPerspective(verticeEnCentro, _rotacion);
            verticesTransformados.Add(verticeConRotacion + _posicion);
        }
        return verticesTransformados;
    }

    public void EstablecerPosicion(Vector3 newPosicion)
    {
        _posicion = newPosicion;
    }

    public List<Vector3> ObtenerColores() => colors;
    public List<Vector3> ObtenerVertices() => vertices;
    public List<uint> ObtenerIndices() => indices;
    public Vector3 ObtenerPosicion() => _posicion;
    public Vector3 ObtenerCentro() => _centro;


}