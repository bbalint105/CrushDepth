using System;

namespace CrushDepth
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new CrushDepth())
                game.Run();
        }
    }
}
