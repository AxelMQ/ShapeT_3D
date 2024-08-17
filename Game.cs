using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

class Game : GameWindow
{
    private Renderer _renderer;

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _renderer = new Renderer();
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _renderer.Initialize();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _renderer.Render();
        SwapBuffers();
    }
}
