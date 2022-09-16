using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Zenject;

/// <summary>
/// See https://github.com/pardeike/Harmony/wiki for a full reference on Harmony.
/// </summary>
namespace PentaSaber.HarmonyPatches
{
    /// <summary>
    /// This patches ClassToPatch.MethodToPatch(Parameter1Type arg1, Parameter2Type arg2)
    /// </summary>
    [HarmonyPatch(typeof(GameNoteController), nameof(GameNoteController.HandleCut),
        new Type[] { // List the Types of the method's parameters.
        typeof(Saber),
        typeof(Vector3),
        typeof(Quaternion),
        typeof(Vector3),
        typeof(bool)})]
    public class GameNoteController_HandleCut
    {
        private static readonly MethodInfo _getBasicCutInfoMethod = typeof(NoteBasicCutInfoHelper)
            .GetMethod(nameof(NoteBasicCutInfoHelper.GetBasicCutInfo));
        private static readonly MethodInfo _cutIsGoodMethod = SymbolExtensions.GetMethodInfo(() => CutIsGood(null!, null!, false));

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (_getBasicCutInfoMethod == null)
            {
                Plugin.Log.Error($"_getBasicCutInfo is null");
            }
            bool finished = false;
            bool readyToInsert = false;
            //Plugin.Log.Critical($"Starting Transpile");
            foreach (var code in instructions)
            {
                //Plugin.Log.Info($"   {code}");
                if (!finished)
                {
                    if (!readyToInsert)
                    {
                        if(code.opcode == OpCodes.Call && code.Calls(_getBasicCutInfoMethod))
                        {
                            //Plugin.Log.Critical($"ReadyToInsert");
                            readyToInsert = true;
                        }
                    }
                    else
                    {
                        //Plugin.Log.Critical($"Inserting OpCodes");
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // Load GameNoteController (this)
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // Load Argument 1 (Saber)
                        yield return new CodeInstruction(OpCodes.Ldloc_3); // Load flag3 (saberTypeOk)
                        yield return new CodeInstruction(OpCodes.Call, _cutIsGoodMethod);
                        yield return new CodeInstruction(OpCodes.Stloc_3); // Store result in flag3 (saberTypeOk)
                        finished = true;
                    }
                }
                yield return code;
            }
        }

        static bool CutIsGood(GameNoteController gameNote, Saber saber, bool saberTypeOk)
        {
            //Plugin.Log.Debug($"Executing CutIsGood: {gameNote?.GetInstanceID()} | {saber?.name} | {saber?.saberType} | {saberTypeOk}");
            PentaSaberController? controller = PentaSaberController.Instance;
            if (gameNote == null)
            {
                Plugin.Log.Warn($"GameNoteController is null.");
                return saberTypeOk;
            }
            if (saber == null)
            {
                Plugin.Log.Warn($"Saber is null.");
                return saberTypeOk;
            }
            if (controller == null)
            {
                Plugin.Log.Warn($"PentaSaberController is null.");
                return saberTypeOk;
            }
            if (controller.TryGetNoteType(gameNote, out var pentaNoteType))
            {
                PentaNoteType pentaSaberType = controller.GetCurrentSaberTypeBySaber(saber.GetInstanceID());
                bool isGood = pentaNoteType == pentaSaberType
                    || pentaNoteType == PentaNoteType.Neutral || pentaNoteType == PentaNoteType.Neutral2;
                //Plugin.Log.Info($"Cutting note '{gameNote.GetInstanceID()}' ({pentaNoteType}) with saber '{pentaSaberType}' aka '{saber}':'{saber.GetInstanceID()}' - Good: {isGood}");
                return isGood;
            }
            else
            {
                Plugin.Log.Warn($"GameNoteController not in dictionary.");
                return saberTypeOk;
            }
        }
    }
}
