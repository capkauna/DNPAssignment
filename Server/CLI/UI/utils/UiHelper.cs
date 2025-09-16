namespace CLI.UI.utils;

public class UiHelper
{
    public static void Pause(string message = "Not implemented yet. Press any key to continue...")
    {
        Console.WriteLine();
        Console.Write(message);
        Console.ReadKey(true);
    }
}