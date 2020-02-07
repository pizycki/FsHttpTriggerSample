var target = Argument("target", "Start");
var configuration = Argument("configuration", "Release");

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./FsHttpTriggerSample/bin");
});

var build = Task("Build")
    .Does(() => {
        DotNetCoreBuild("./FsHttpTriggerSample/FsHttpTriggerSample.fsproj", new DotNetCoreBuildSettings{
            Configuration = configuration
        });
    });

var start = Task("Start")
    .IsDependentOn(build)
    .Does(() => {
        var ps = new ProcessSettings {
            Arguments = "host start",
            WorkingDirectory = "C:/dev/FsHttpTriggerSample/FsHttpTriggerSample/bin/" + configuration + "/netstandard2.0",
            Silent = true
        };
        using(var process = StartAndReturnProcess("func.cmd", ps))
        {
            process.WaitForExit();
            // This should output 0 as valid arguments supplied
            Information("Exit code: {0}", process.GetExitCode());
        }
    });

RunTarget(target);
