using CGCourse;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

var gameWindowSettings = GameWindowSettings.Default;
var nativeWindowSettings = new NativeWindowSettings
{
    Title = "Задание 1 — Кубическая кривая Безье",
    Size = new Vector2i(1200, 800),
    APIVersion = new Version(3, 3)
};

using var window = new BezierWindow(gameWindowSettings, nativeWindowSettings);
window.Run();
