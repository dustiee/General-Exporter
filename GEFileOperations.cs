using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;

using static GeneralExporter.LogTools;

namespace GeneralExporter;

internal static class FileOperations
{
  private static readonly HashSet<string> _clearedContexts = [];
  private static readonly string _basePath = Path.Combine(Paths.BepInExRootPath, "GeneralExporterOutput");

  internal static void SaveContentsToFolder
    (IEnumerable<KeyValuePair<string, string>> titleContentPairs, string folderName)
  {
    string folderPath = Path.Combine(_basePath, folderName);
    Directory.CreateDirectory(folderPath);

    // Remove files so we dont get duplicates from previous sessions
    if (!_clearedContexts.Contains(folderPath))
    {
      try
      {
        _clearedContexts.Add(folderPath);
        foreach (var file in Directory.GetFiles(folderPath))
        {
          File.Delete(file);
        }
      }
      catch (Exception ex)
      {
        Error($"Failed to clear files. {ex.Message}");
      }
    }

    // Add the content
    foreach (KeyValuePair<string, string> titleContentPair in titleContentPairs)
    {
      string fileName = ValidateFileName(titleContentPair.Key);
      ResolveDuplicates(folderPath, ref fileName);
      string fileContent = titleContentPair.Value;
      string fullPath = Path.Combine(folderPath, fileName);

      try
      {
        File.WriteAllText(fullPath, fileContent);
      }
      catch (Exception ex)
      {
        Error($"Failed to add file: {ex.Message}");
      }

    }

  }

  internal static void ResolveDuplicates(string path, ref string fileName)
  {
    string name = Path.GetFileNameWithoutExtension(fileName);
    string extension = Path.GetExtension(fileName);

    string candidate = fileName;
    int number = 1;

    while (File.Exists(Path.Combine(path, candidate)))
    {
      candidate = $"{name}_{number}{extension}";
      number++;
    }

    fileName = candidate;
  }

  private static string ValidateFileName(string fileName)
  {
    if (string.IsNullOrWhiteSpace(fileName))
      throw new ArgumentException("Got empty file.", nameof(fileName));

    if (Path.IsPathRooted(fileName))
      throw new ArgumentException("Got rooted path.", nameof(fileName));

    string normalized = fileName.Replace('\\', '/');

    string[] parts = normalized.Split('/');

    if (parts.Any(part => part == ".."))
      throw new ArgumentException(
          "Path going towards root.",
          nameof(fileName));

    return fileName;
  }
}
