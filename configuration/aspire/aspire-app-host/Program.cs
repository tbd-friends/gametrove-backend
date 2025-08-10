var builder = DistributedApplication.CreateBuilder(args);

var gamesApi = builder.AddProject<Projects.games_api>("games-api")
    .WithOtlpExporter();

var igdbApi = builder.AddProject<Projects.igdb_api>("igdb-api")
    .WithOtlpExporter();

var apiGateway = builder.AddProject<Projects.api_gateway>("api-gateway")
    .WithReference(gamesApi)
    .WithReference(igdbApi)
    .WithOtlpExporter();

await builder.Build().RunAsync();