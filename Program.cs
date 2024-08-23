using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

class Program
{
    static void Main()
    {
        var gameWindowSetting = GameWindowSettings.Default;
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(900, 600),
            Title = "OpenTK T Shape",
            WindowState = WindowState.Normal
        };

        using (var game = new Game(gameWindowSetting, nativeWindowSettings))
        {
            game.Run();
        }


    }
}