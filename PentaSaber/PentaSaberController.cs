using PentaSaber.HarmonyPatches.Manager;
using SiraUtil.Sabers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace PentaSaber
{
    public class PentaSaberController : IInitializable, ITickable, IDisposable
    {
        public static PentaSaberController? Instance { get; private set; }

        private readonly Dictionary<GameNoteController, PentaNoteType> _activeNotes
            = new Dictionary<GameNoteController, PentaNoteType>();

        private readonly IPentaColorManager _colorManager;
        private readonly IInputController _inputController;
        private readonly SaberManager _saberManager;
        private readonly SaberModelManager _modelManager;

        private PentaNoteType _saberAType = PentaNoteType.ColorA1;
        public PentaNoteType SaberAType
        {
            get => _saberAType;
            private set
            {
                if (_saberAType == value)
                    return;
                _saberAType = value;
                if(SaberA == null)
                {
                    Plugin.Log.Error($"SaberA is null");
                    return;
                }
                _modelManager.SetColor(SaberA, Plugin.Config.GetColor(value));
                Plugin.Log.Error($"SaberA is {value} | {SaberAType}");
            }
        }

        private PentaNoteType _saberBType = PentaNoteType.ColorB1;

        public PentaNoteType SaberBType
        {
            get => _saberBType;
            private set
            {
                if (_saberBType == value)
                    return;
                _saberBType = value;
                if (SaberB == null)
                {
                    Plugin.Log.Error($"SaberB is null");
                    return;
                }
                _modelManager.SetColor(SaberB, Plugin.Config.GetColor(value));

                Plugin.Log.Error($"SaberB is {value} | {SaberBType}");
            }
        }

        public Saber? SaberA => _saberManager.leftSaber;

        public Saber? SaberB => _saberManager.rightSaber;

        public PentaSaberController(IPentaColorManager colorManager, IInputController inputController,
            SaberModelManager modelManager, SaberManager saberManager)
        {
            Plugin.Log.Debug($"PentaSaberController constructed.");
            _colorManager = colorManager;
            _inputController = inputController;
            _modelManager = modelManager;
            _saberManager = saberManager;
            HarmonyManager.ApplyDefaultPatches();
        }
        public void Initialize()
        {
            Instance = this;
            Plugin.Log.Debug($"PentaSaberController Initialized.");
        }

        public PentaNoteType SetNoteType(GameNoteController note)
        {
            PentaNoteType noteType = _colorManager.GetNextNoteType(note);
            _activeNotes[note] = noteType;
            return noteType;
        }

        public bool TryGetNoteType(GameNoteController note, out PentaNoteType noteType)
        {
            if(!_activeNotes.TryGetValue(note, out noteType))
                return false;
            return true;
        }

        public void RemoveNoteType(GameNoteController note)
        {
            _activeNotes.Remove(note);
        }

        public PentaNoteType GetCurrentSaberType(SaberType saberType)
        {
            return (saberType) switch
            {
                SaberType.SaberA => SaberAType,
                SaberType.SaberB => SaberBType,
                _ => PentaNoteType.None
            };
        }

        public void Dispose()
        {
            Plugin.Log.Debug($"Disposing PentaSaberController, {_activeNotes.Count} notes remaining in dictionary.");
            _activeNotes.Clear();
            HarmonyManager.UnpatchAll();
        }

        public void Tick()
        {
            if(Instance != this)
            {
                Plugin.Log.Error($"WRONG INSTANCE ROOOOOOOOAAAAAAR");
                Instance = this;
            }
            SaberAType = _inputController.SaberAToggled ? PentaNoteType.ColorA2 : PentaNoteType.ColorA1;
            SaberBType = _inputController.SaberBToggled ? PentaNoteType.ColorB2 : PentaNoteType.ColorB1;
        }
    }

    public enum PentaNoteType
    {
        None = -1,
        ColorA1 = 0,
        ColorB1 = 1,
        ColorA2 = 2,
        ColorB2 = 3,
        Neutral = 4
    }
}
