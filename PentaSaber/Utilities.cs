using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IPA.Utilities;
namespace PentaSaber
{
    internal static class Utilities
    {
        public static FieldAccessor<SaberModelContainer, SaberModelController>.Accessor getSaberModelController =
            FieldAccessor<SaberModelContainer, SaberModelController>.GetAccessor("_saberModelControllerPrefab");
        public static void PrintComponents(this GameObject obj)
        {
            Plugin.Log.Info($"Components: {obj.name}");
            foreach (var item in obj.GetComponentsInChildren<MonoBehaviour>())
            {
                Plugin.Log.Info($"   {item.name} | {item.GetType().Name}");
                foreach(var child in item.GetComponentsInChildren<MonoBehaviour>())
                {
                    Plugin.Log.Info($"      {child.name} | {child.GetType().Name}");
                }
            }
        }
    }
}
