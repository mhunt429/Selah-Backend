using Selah.Infrastructure;
using Selah.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory>(provider =>
{
    var connectionString = builder.Configuration.GetValue<string>("SelahDbConnectionString");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ApplicationException("No connection string configured.");
    }

    return new SelahDbConnectionFactory(connectionString);
});

builder.Services.RegisterRepositories();

IConfigurationRoot configuration = builder.Configuration;

builder.Services.RegisterJwt(configuration);

builder.Services.RegisterHashIds(configuration);

builder.Services.AddConfiguration(configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddAuthorization(options => { });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();