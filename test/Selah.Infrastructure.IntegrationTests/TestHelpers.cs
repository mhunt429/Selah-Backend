using System.Data;

namespace Selah.Infrastructure.IntegrationTests;

public static class TestHelpers
{
    public static string TestConnectionString { get; } =
        "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres";
    
    public static SelahDbConnectionFactory TestDbFactory { get; } = new SelahDbConnectionFactory(TestConnectionString); 
}