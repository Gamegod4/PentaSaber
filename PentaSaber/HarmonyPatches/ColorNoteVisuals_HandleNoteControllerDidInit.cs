using HarmonyLib;
using IPA.Utilities;
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
    /// <summary>
    /// This patches ClassToPatch.MethodToPatch(Parameter1Type arg1, Parameter2Type arg2)
    /// </summary>
    [HarmonyPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit),
        new Type[] { // List the Types of the method's parameters.
        typeof(NoteControllerBase) })]
    public class ColorNoteVisuals_NoteController_HandleNoteControllerDidInit
    {
        /// <summary>
        /// This code is run before the original code in MethodToPatch is run.
        /// </summary>
        static void Postfix(ColorNoteVisuals __instance, ref NoteControllerBase noteController, MaterialPropertyBlockController[] ____materialPropertyBlockControllers, ref Color ____noteColor)
        {
            if (!(noteController is GameNoteController gnc))
                return;
            var pc = PentaSaberController.Instance;
            if (pc == null)
                return;
            PentaNoteType pentaNoteType = pc.SetNoteType(gnc);
            ____noteColor = Plugin.Config.GetColor(pentaNoteType);
            //Plugin.Log.Info($"Setting note '{gnc.GetInstanceID()}' to {pentaNoteType}");
            foreach (var m in ____materialPropertyBlockControllers)
            {
                m.materialPropertyBlock.SetColor(Shader.PropertyToID("_Color"), ____noteColor);
                m.ApplyChanges();
            }
        }
    }

    [HarmonyPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit),
        new Type[] { // List the Types of the method's parameters.
        typeof(BurstSliderGameNoteController) })]
    public class ColorNoteVisuals_BurstSlider_HandleNoteControllerDidInit
    {
        static void Postfix(ColorNoteVisuals __instance, ref BurstSliderGameNoteController noteController, MaterialPropertyBlockController[] ____materialPropertyBlockControllers, ref Color ____noteColor)
        {
            if (!(noteController is BurstSliderGameNoteController bnc))
                return;
            var pc = PentaSaberController.Instance;
            if (pc == null)
                return;
            PentaNoteType pentaNoteType = pc.SetNoteType(bnc);
            ____noteColor = Plugin.Config.GetColor(pentaNoteType);
            //Plugin.Log.Info($"Setting note '{bnc.GetInstanceID()}' to {pentaNoteType}");
            foreach (var m in ____materialPropertyBlockControllers)
            {
                m.materialPropertyBlock.SetColor(Shader.PropertyToID("_Color"), ____noteColor);
                m.ApplyChanges();
            }
        }
    }
}
