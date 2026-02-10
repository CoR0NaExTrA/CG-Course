//поправить ресайз окна (картинка начинает краями обрезаться, а не уменьшается с сохранением пропорций)

namespace Task1;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}