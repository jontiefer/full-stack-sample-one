namespace Developer.API;

public static class AppInitialization
{
    public static void LoadEnvironmentFile(IConfigurationBuilder configuration, string fileName)
    {
        try
        {
            if (File.Exists(fileName))
            {
                configuration.AddInMemoryCollection(File.ReadAllLines(fileName)
                    .Where(line => !string.IsNullOrWhiteSpace(line) && !line.Trim().StartsWith("#"))
                    .Select(line => line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries))
                    .ToDictionary(parts => parts[0].Trim(), parts => parts.Length > 1 ? parts[1].Trim() : ""));
            }
            else
            {
                throw new FileNotFoundException($"Environment file '{fileName}' not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading environment variable file into application configuration.{Environment.NewLine}{ex.Message}");
            throw;
        }
    }
}
