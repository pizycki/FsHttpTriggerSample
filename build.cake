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
    .Does(async () => {
        await System.Threading.Tasks.Task.Run(() => {
            var exitCodeWithArgument = StartProcess("powershell", new ProcessSettings {
                Arguments = "func host start",
                WorkingDirectory = "./FsHttpTriggerSample/bin/" + configuration + "/netstandard2.0"
            });
            if (exitCodeWithArgument != 0) throw new Exception ("Failed to host functions!");
        });
    });

RunTarget(target);
