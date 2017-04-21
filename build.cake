var configuration = Argument("configuration", "Debug");

Task("Clean")
.Does(() =>
{
    CleanDirectory("./artifacts/");
});

Task("Restore")
.Does(() => 
{
    DotNetCoreRestore();
});

Task("Build")
.Does(() =>
{
    var projects = GetFiles("./src/**/*.csproj");
    Console.WriteLine("Building {0} projects", projects.Count());

    foreach (var project in projects)
    {
        DotNetCoreBuild(project.FullPath, new DotNetCoreBuildSettings
        {
            Configuration = configuration
        });
    }
});

Task("Pack")
.Does(() =>
{
    var projects = GetFiles("./src/**/*.csproj");
    Console.WriteLine("Packing {0} projects", projects.Count());

    foreach (var project in projects)
    {
        DotNetCorePack(project.FullPath, new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = "./artifacts/"
        });
    }
});

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Pack");


var target = Argument("target", "Default");
RunTarget(target);