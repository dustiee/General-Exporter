using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
namespace GeneralExporter;


internal static class LogTools
{

  // Timing


  internal static void EndStopwatchAndDebugPrint(
      Stopwatch stopwatch,
      string timePrefixMessage)
  {
    stopwatch.Stop();

    double milliseconds = stopwatch.ElapsedTicks * (1_000.0 / Stopwatch.Frequency);

    if (milliseconds > 10.0)
    {
      double microseconds = milliseconds * 1_000.0;
      Debug($"{timePrefixMessage} : {microseconds:F0} µs");
    }
    else
    {
      Debug($"{timePrefixMessage} : {milliseconds:F3} ms");
    }
  }

  // Logging 

  private static string FormatMessage(
      object? obj,
      string caller,
      string callerPath)
      => $"""
            {obj} [ FROM: {caller} @ {Path.GetFileName(callerPath)}]
            """;

  internal static void Print(
      object? obj,
      [CallerMemberName] string caller = "",
      [CallerFilePath] string callerPath = "")
  {
    GeneralExporter.Log!.LogInfo(FormatMessage(obj, caller, callerPath));
  }

  internal static void Debug(
      object? obj,
      [CallerMemberName] string caller = "",
      [CallerFilePath] string callerPath = "")
  {
    GeneralExporter.Log!.LogDebug(FormatMessage(obj, caller, callerPath));
  }

  internal static void Verbose(
      object? obj,
      [CallerMemberName] string caller = "",
      [CallerFilePath] string callerPath = "")
  {
    GeneralExporter.Log!.LogDebug(FormatMessage(obj, caller, callerPath));
  }


  internal static void Warn(
      object? obj,
      [CallerMemberName] string caller = "",
      [CallerFilePath] string callerPath = "")
  {
    GeneralExporter.Log!.LogWarning(FormatMessage(obj, caller, callerPath));
  }

  internal static void Error(
      object? obj,
      [CallerMemberName] string caller = "",
      [CallerFilePath] string callerPath = "")
  {
    GeneralExporter.Log!.LogError(FormatMessage(obj, caller, callerPath));
  }

  internal static void Fatal(
      object? obj,
      [CallerMemberName] string caller = "",
      [CallerFilePath] string callerPath = "")
  {
    GeneralExporter.Log!.LogFatal(FormatMessage(obj, caller, callerPath));
  }

}
