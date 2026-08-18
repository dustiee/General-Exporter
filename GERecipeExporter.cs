using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using HarmonyLib;
using UnityEngine;
using static RecipeLoader.API;

using static GeneralExporter.FileOperations;
using static GeneralExporter.LogTools;
using static GeneralExporter.XmlTools;



namespace GeneralExporter;

[HarmonyPatch(typeof(RecipeManager), nameof(RecipeManager.AddBuiltinRecipes))]
[HarmonyPriority(Priority.First)]
internal static class RecipeExporter
{
  [HarmonyPrefix]
  internal static void Prefix(RecipeManager __instance)
  {
    Print("Trying to get recipes.");
    Queue<KeyValuePair<string, string>> titleContents = [];

    if (GeneralExporter.RecipeLoaderIncluded)
    {
      RecipeExporter_DepRecipeLoader.ExportRecipes(__instance, titleContents);
    }
    else
    {
      foreach (RecipeManager.CategorizedRecipes cRecipes in __instance.categorizedRecipes)
      {
        foreach (TextAsset tRecipe in cRecipes.recipes)
        {
          string recipeXmlString = GetValidXmlString(tRecipe);
          string recipeName = GetRootAttributeValue(recipeXmlString, "name") ?? "invalidName";

          titleContents.Enqueue(
            new KeyValuePair<string, string>(recipeName, recipeXmlString)
          );
        }
      }
    }

    Print($"Got {titleContents.Count} from this recipe manager.");
    SaveContentsToFolder(titleContents, "Recipes");
  }

}

// Contain all uses of Recipe Loader here such that RecipeExporter doesn't reference Recipe Loader.
// (It would break otherwise if Recipe Loader wasn't available)
internal static class RecipeExporter_DepRecipeLoader
{
  internal static void ExportRecipes(
    RecipeManager recipeManager,
    Queue<KeyValuePair<string, string>> titleContents)
  {
    StationType? station = InferStation(recipeManager.categorizedRecipes);

    if (station == null)
    {
      Warn("Got null station");
      return;
    }

    foreach (RecipeManager.CategorizedRecipes cRecipes in recipeManager.categorizedRecipes)
    {
      foreach (TextAsset tRecipe in cRecipes.recipes)
      {
        string recipeXmlString = GetValidXmlString(tRecipe);
        string recipeName =
          GetRootAttributeValue(recipeXmlString, "name") ?? "invalidName";

        string? recipeContent =
          TextAssetRecipeToRecipeLoaderRepresentation(
            station.Value,
            cRecipes.category,
            tRecipe
          );

        if (recipeContent == null)
        {
          Warn("Got no recipe content");
          continue;
        }

        titleContents.Enqueue(
          new KeyValuePair<string, string>(recipeName, recipeContent)
        );
      }
    }
  }
}
