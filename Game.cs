using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;

class Game : GameWindow
{
    private Renderer _renderer;
    private Objeto _miFigura;

    private Vector3 _movementX = new Vector3(0.001f, 0.0f, 0.0f); // Movimiento en el eje X
    private Vector3 _movementY = new Vector3(0.0f, 0.001f, 0.0f); // Movimiento en el eje Y


    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _renderer = new Renderer();
        _miFigura = new Objeto();
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _renderer.Initialize(_miFigura);
    } 

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _renderer.Render(_miFigura);
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        // Eje X y Y
        if (KeyboardState.IsKeyDown(Keys.Right))
        {
            _miFigura.Trasladar(-_movementX);
        }
        if (KeyboardState.IsKeyDown(Keys.Left))
        {
            _miFigura.Trasladar(_movementX);
        }
        if (KeyboardState.IsKeyDown(Keys.Up))
        {
            _miFigura.Trasladar(_movementY);
        }
        if (KeyboardState.IsKeyDown(Keys.Down))
        {
            _miFigura.Trasladar(-_movementY);
        }

    }
}
