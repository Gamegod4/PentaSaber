using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// See https://github.com/pardeike/Harmony/wiki for a full reference on Harmony.
/// </summary>
namespace PentaSaber.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorManager), nameof(ColorManager.ColorForSaberType),
        new Type[] {
        typeof(SaberType)})]
    public class ColorManager_ColorForSaberType
    {
        static void Postfix(ColorManager __instance, ref SaberType type, ref Color __result)
        {
            PentaSaberController? controller = PentaSaberController.Instance;
            if (controller == null)
                return;
            PentaNoteType noteType = controller.GetCurrentSaberType(type);
            __result = Plugin.Config.GetColor(noteType);
        }
    }
}