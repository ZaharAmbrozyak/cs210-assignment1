namespace cs210_assignment1;

using System.Globalization;

static class Program
{
    private static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        var algorithm = new ShuntingYard();
        
        algorithm.Run();
    }
}