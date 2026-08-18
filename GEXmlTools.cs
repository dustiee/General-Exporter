using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;


namespace GeneralExporter;

internal static class XmlTools
{
  public static string GetValidXmlString(TextAsset asset)
  {
    if (asset == null)
      throw new ArgumentNullException(nameof(asset));

    string input = asset.text;
    StringBuilder result = new(input.Length);

    foreach (char c in input)
    {
      if (XmlConvert.IsXmlChar(c))
        result.Append(c);
    }

    return result.ToString();
  }


  public static string? GetRootAttributeValue(string xml, string attributeName)
  {
    if (string.IsNullOrEmpty(xml))
      return null;

    try
    {
      XDocument document = XDocument.Parse(xml);
      return document.Root?.Attribute(attributeName)?.Value;
    }
    catch (XmlException)
    {
      return null;
    }
  }

  private static Dictionary<string, XElement> _xmlCache = [];
  internal static bool CacheContainsItems { get => _xmlCache.Count > 0; }

  public static string? GetCacheableRootAttributeValue(string xml, string attributeName)
  {
    if (!_xmlCache.TryGetValue(xml, out XElement? cachedRoot))
    {
      try
      {
        XDocument xDoc = XDocument.Parse(xml);
        cachedRoot = xDoc.Root;

        foreach (XElement elem in cachedRoot.Elements())
        {
          elem.Remove();
        }
        _xmlCache[xml] = cachedRoot;
      }
      catch { return null; }
    }

    return cachedRoot?.Attribute(attributeName)?.Value;
  }

  public static void ClearCache()
  {
    _xmlCache = [];
  }

}
