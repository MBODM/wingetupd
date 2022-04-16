﻿// None of the async calls is using .ConfigureAwait(false) any longer now, since it´s useless
// in console apps. For more information about that topic, take a look at the following links:
// https://devblogs.microsoft.com/dotnet/configureawait-faq/
// https://stackoverflow.com/questions/25817703/configureawaitfalse-not-needed-in-console-win-service-apps-right
// Of course, this is not true for libraries. Always use .ConfigureAwait(false) in libraries.

using WinGet;
using WinGetUpd;
using WinGetUpdCore;

Console.WriteLine();
Console.WriteLine($"{AppData.AppName} {AppData.AppVersion} (by MBODM {AppData.AppDate})");
Console.WriteLine();

var businessLogic = new BusinessLogic(new PrerequisitesHelper(), new WinGetWrapper(new WinGetRunner(new WinGetLogger(AppData.LogFile))));

try
{
    await businessLogic.InitAsync();
    var entries = await businessLogic.GetPackageFileEntries();
    Console.WriteLine($"Package-File found, containing {entries.Count()} {ProgramHelper.GetEntryOrEntriesText(entries.Count())}.");
    Console.WriteLine();
    Console.Write("Processing...");
    var packageInfos = await businessLogic.AnalyzePackagesAsync(entries, new PackageProgress(_ => Console.Write(".")));
    ProgramHelper.ShowSummary(packageInfos);
}
catch (BusinessLogicException e)
{
    Console.WriteLine($"Error: {e.Message}");
    Environment.Exit(1);
}

// have a look at log file or winget log at:
// %LOCALAPPDATA%\Packages\Microsoft.DesktopAppInstaller_8wekyb3d8bbwe\LocalState\DiagOutputDir

Console.WriteLine();