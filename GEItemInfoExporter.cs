using System.Collections.Generic;
using HarmonyLib;

using static GeneralExporter.FileOperations;
using static GeneralExporter.LogTools;

namespace GeneralExporter;


[HarmonyPatch(typeof(InvDatabase), "Awake")]
internal static class ItemExporter
{
  private static bool _alreadyRan = false;
  [HarmonyPostfix]
  internal static void Postfix(InvDatabase __instance)
  {
    if (_alreadyRan)
    {
      return;
    }
    _alreadyRan = true;
    int itemsGot = 0;

    InvDatabase[] databases = InvDatabase.list;
    Queue<KeyValuePair<string, string>> titleContents = [];

    foreach (InvDatabase database in databases)
    {

      List<InvBaseItem> items = database.items;
      itemsGot += items.Count;

      foreach (InvBaseItem item in items)
      {
        if (item.name == null)
        {
          continue;
        }
        InvGameItem gameItem = InvDatabase.CreateItem(item, 0, 1);
        titleContents.Enqueue(new KeyValuePair<string, string>(gameItem?.itemName ?? "Unknown item name", ItemToInfoString(item, gameItem)));
      }
    }

    SaveContentsToFolder(titleContents, "Items");
    Print($"Exported {itemsGot} items");
  }


  private static void AppendItemInfo(
      System.Text.StringBuilder sb,
      string name,
      object value)
  {
    sb.AppendLine($"{name}: {value ?? "null"}");
  }

  private static string ItemToInfoString(InvBaseItem item, InvGameItem? gameItem)
  {
    if (item == null)
    {
      return "item: null\n";
    }

    if (gameItem == null)
    {
      return "item: null\n";
    }

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("=====================");
    sb.AppendLine("[The properties below are most useful when making new recipes:] \n");
    AppendItemInfo(sb, "INTERNAL name", item.name);
    AppendItemInfo(sb, "durability", item.durability);

    sb.AppendLine("\n[All other properties:] \n");
    AppendItemInfo(sb, "itemName", gameItem.itemName);
    AppendItemInfo(sb, "itemDesc", gameItem.itemDesc);



    AppendItemInfo(sb, "id16", item.id16);
    AppendItemInfo(sb, "slot", item.slot);
    AppendItemInfo(sb, "category", item.category);
    AppendItemInfo(sb, "price", item.price);
    AppendItemInfo(sb, "color", item.color);

    sb.AppendLine("stats:");

    if (item.stats == null || item.stats.Count == 0)
    {
      sb.AppendLine("  (none)");
    }
    else
    {
      foreach (InvStat stat in item.stats)
      {
        if (stat == null)
        {
          sb.AppendLine("  null");
          continue;
        }

        AppendItemInfo(sb, "  id", stat.id);
        AppendItemInfo(sb, "  modifier", stat.modifier);
        AppendItemInfo(sb, "  amount", stat.amount);
      }
    }

    sb.AppendLine("effects:");

    if (item.effects == null || item.effects.Count == 0)
    {
      sb.AppendLine("  (none)");
    }
    else
    {
      foreach (InvEffect effect in item.effects)
      {
        if (effect == null)
        {
          sb.AppendLine("  null");
          continue;
        }

        AppendItemInfo(sb, "  id", effect.id);
        AppendItemInfo(sb, "  modifier", effect.modifier);
        AppendItemInfo(sb, "  amount", effect.amount);
        AppendItemInfo(sb, "  duration", effect.duration);
      }
    }


    AppendItemInfo(sb, "options", item.options);
    AppendItemInfo(sb, "extraOptions", item.extraOptions);


    AppendItemInfo(sb, "collectable", item.collectable);
    AppendItemInfo(sb, "consumable", item.consumable);
    AppendItemInfo(sb, "twoHanded", item.twoHanded);
    AppendItemInfo(sb, "spawner", item.spawner);
    AppendItemInfo(sb, "isRYB", item.isRYB);
    AppendItemInfo(sb, "isRYBLight", item.isRYBLight);
    AppendItemInfo(sb, "hideFromCreative", item.hideFromCreative);
    AppendItemInfo(sb, "alwaysCombine", item.alwaysCombine);
    AppendItemInfo(sb, "requestDrop", item.requestDrop);
    AppendItemInfo(sb, "pet", item.pet);


    AppendItemInfo(sb, "energy", item.energy);


    AppendItemInfo(sb, "iconName", item.iconName);
    AppendItemInfo(sb, "iconAtlas", item.iconAtlas);
    AppendItemInfo(sb, "attachment", item.attachment);
    AppendItemInfo(sb, "loot", item.loot);
    sb.AppendLine("---------------------");


    return sb.ToString();
  }

}
