using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using HarmonyLib;
using UnityEngine;

using static GeneralExporter.FileOperations;
using static GeneralExporter.LogTools;
using static GeneralExporter.XmlTools;

namespace GeneralExporter;

[HarmonyPatch(typeof(AchievementManager), "AddBuiltinAchievements")]
internal static class AchievementExporter
{
  private static bool _exportedAchievements = false;
  internal static bool AreAchievementsExported { get => _exportedAchievements; }

  [HarmonyPrefix]
  static void Prefix(AchievementManager __instance, XmlSerializer serial)
  {
    if (_exportedAchievements)
    {
      return;
    }
    _exportedAchievements = true;

    List<AchievementManager.AchieveData>? achievements = __instance.achievementsList;

    if (achievements == null)
    {
      Debug("No achievements to export");
      return;
    }

    Queue<KeyValuePair<string, string>> titleContents = [];

    foreach (AchievementManager.AchieveData achievementData in achievements)
    {
      string category = achievementData.category.ToString();
      string achieveXmlString = GetValidXmlString(achievementData.achievements);

      titleContents.Enqueue(new KeyValuePair<string, string>(category, achieveXmlString));
    }


    Debug($"Exporting {titleContents.Count} achievements");
    SaveContentsToFolder(titleContents, "Achievements");

  }

}
