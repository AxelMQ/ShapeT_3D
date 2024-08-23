using System.Numerics;

public class Objeto
{
    private readonly List<Vector3> vertices;
    private readonly List<uint> indices;
    private Vector3 _posicion;
    private Vector3 _centro;

    public Objeto()
    {
        vertices = new List<Vector3>();
        indices = new List<uint>();
        _posicion = Vector3.Zero;
        DefinirVertices();
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
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] += desplazamiento;
        }
        //_posicion += desplazamiento;
    }

    public List<Vector3> ObtenerVerticesTransformados()
    {
        var verticesTransformados = new List<Vector3>();
        foreach (var vertice in vertices)
        {
            // Trasladar respecto al nuevo centro y la posición
            verticesTransformados.Add(vertice - _centro + _posicion);
        }
        return verticesTransformados;
    }


    public List<Vector3> ObtenerVertices() => vertices;
    public List<uint> ObtenerIndices() => indices;
    public Vector3 ObtenerPosicion() => _posicion;
    public Vector3 ObtenerCentro() => _centro;


}