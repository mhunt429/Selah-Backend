var builder = DistributedApplication.CreateBuilder(args);



builder.AddProject<Projects.Selah_WebAPI>("selah-webapi");

builder.Build().Run();