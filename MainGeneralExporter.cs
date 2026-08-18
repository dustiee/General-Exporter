using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;

using static GeneralExporter.LogTools;
namespace GeneralExporter;


[BepInPlugin("dev.dustie.generalexporter", "General Exporter", "1.0.0")]
[BepInDependency("dev.dustie.recipeloader", BepInDependency.DependencyFlags.SoftDependency)]
public class GeneralExporter : BaseUnityPlugin
{
  internal static ManualLogSource? Log;
  internal static readonly bool RecipeLoaderIncluded = Chainloader.PluginInfos.ContainsKey("dev.dustie.recipeloader");

  internal static bool[] cacheUsersDone = [QuestExporter.AreQuestsExported];

  private void Awake()
  {
    Log = Logger;

    if (RecipeLoaderIncluded)
    {
      Print("Recipe Loader is present, recipes will be tailored to the loader.");
    }
    else
    {
      Print("Recipe Framework isn't present, recipes will be raw.");
    }

    Harmony harmony = new("dev.dustie.generalexporter");
    harmony.PatchAll();
  }

  private void Update()
  {
    if (XmlTools.CacheContainsItems && cacheUsersDone.All(user => user == true))
    {
      XmlTools.ClearCache();
    }
  }

}
