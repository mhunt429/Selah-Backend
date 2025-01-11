using DbMigrationUtility;
using EvolveDb;
using Npgsql;
public class Program
{
    public static void Main(string[] args)
    {
        var connectionStrings = new ConnectionStrings();
        string location = "migrations";

        if (args.Length == 0)
        {
           throw new Exception("No arguments specified.");
        }

        string environment = args[0].ToLower();

        string connection;
        switch (environment)
        {
            case "local":
                connection = connectionStrings.Local;
                break;
            case "test":
                connection = connectionStrings.Test;
                break;
            case "production":
                connection = connectionStrings.Production;
                if (string.IsNullOrEmpty(connection))
                {
                    throw new Exception("Error: Production connection string is not set in the environment variables.");
                
                }
                break;
            default:
                throw new Exception("Error: Invalid environment specified. Use 'local', 'test', or 'production'.");
        }

        try
        {
            using var dbConnection = new NpgsqlConnection(connection);
            var evolve = new Evolve(dbConnection, 
                msg => Console.WriteLine($"Beginning database migrations with {msg}"))
            {
                Locations = new[] { location }
            };

            evolve.Migrate();
            Console.WriteLine("Database migrations completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during migration: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}