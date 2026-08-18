using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using HarmonyLib;
using UnityEngine;

using static GeneralExporter.FileOperations;
using static GeneralExporter.LogTools;
using static GeneralExporter.XmlTools;

namespace GeneralExporter;

[HarmonyPatch(typeof(QuestManager), "AddBuiltinQuests")]
internal static class QuestExporter
{
  private static bool _exportedQuests = false;
  internal static bool AreQuestsExported { get => _exportedQuests; }

  [HarmonyPrefix]
  static void Prefix(QuestManager __instance, XmlSerializer serial)
  {
    if (_exportedQuests)
    {
      return;
    }
    _exportedQuests = true;

    TextAsset[]? quests = __instance.availableQuests;

    if (quests == null)
    {
      Debug("No quests to export");
      return;
    }

    Queue<KeyValuePair<string, string>> titleContents = [];

    foreach (TextAsset asset in quests)
    {
      string questXml = GetValidXmlString(asset);
      string questTitle = GetCacheableRootAttributeValue(questXml, "title") ?? "Untitled Quest";
      string questNpc = GetCacheableRootAttributeValue(questXml, "npc") ?? "Unknown Npc";

      string fileName = $"{questTitle} @ {questNpc}";


      titleContents.Enqueue(new KeyValuePair<string, string>(fileName, questXml));
    }


    Debug($"Exporting {titleContents.Count} quests");
    SaveContentsToFolder(titleContents, "Quests");

  }

}
