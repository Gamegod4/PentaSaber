using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// See https://github.com/pardeike/Harmony/wiki for a full reference on Harmony.
/// </summary>
namespace PentaSaber.HarmonyPatches
{
    /// <summary>
    /// This patches ClassToPatch.MethodToPatch(Parameter1Type arg1, Parameter2Type arg2)
    /// </summary>
    [HarmonyPatch(typeof(GameNoteController), "NoteDidStartDissolving",
        new Type[] { })]
    public class GameNoteController_NoteDidStartDissolving
    {
        /// <summary>
        /// This code is run after the original code in MethodToPatch is run.
        /// </summary>
        static void Postfix(GameNoteController __instance)
        {
            PentaSaberController.Instance?.RemoveNoteType(__instance);
        }
    }
}