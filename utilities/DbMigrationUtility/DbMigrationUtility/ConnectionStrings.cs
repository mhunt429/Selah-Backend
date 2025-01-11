namespace DbMigrationUtility;

public class ConnectionStrings
{
    public string Local => "User ID=postgres;Password=postgres;Host=localhost;Port=55432;Database=postgres";
    public string Test => "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres";
    
    public string Production => Environment.GetEnvironmentVariable("Selah_DB_Production_Connection_String");
}