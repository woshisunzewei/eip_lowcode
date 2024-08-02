/// <summary>
/// 控制台logo
/// </summary>
public static class ConsoleLogo
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static void AddConsoleLogoSetUp(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"
    ______  _____  _____              _____  _____    _____  _                _   
   |  ____||_   _||  __ \      /\    |  __ \|_   _|  / ____|| |              | |  
   | |__     | |  | |__) |    /  \   | |__) | | |   | (___  | |_  __ _  _ __ | |_ 
   |  __|    | |  |  ___/    / /\ \  |  ___/  | |    \___ \ | __|/ _` || '__|| __|
   | |____  _| |_ | |       / ____ \ | |     _| |_   ____) || |_| (_| || |   | |_ 
   |______||_____||_|      /_/    \_\|_|    |_____| |_____/  \__|\__,_||_|    \__|");
    }
}