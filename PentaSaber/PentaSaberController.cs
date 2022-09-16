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
        private readonly SiraSaberFactory _siraSaberFactory;

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
                //Plugin.Log.Error($"SaberA is {value} | {SaberAType}");
            }
        }

        private PentaNoteType _saberCType = PentaNoteType.ColorA2;
        public PentaNoteType SaberCType
        {
            get => _saberCType;
            private set
            {
                if (_saberCType == value)
                    return;
                _saberCType = value;
                if (SaberC == null)
                {
                    Plugin.Log.Error($"SaberC is null");
                    return;
                }
                _modelManager.SetColor(SaberC.Saber, Plugin.Config.GetColor(value));
                //Plugin.Log.Error($"SaberA is {value} | {SaberAType}");
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
                //Plugin.Log.Error($"SaberB is {value} | {SaberBType}");
            }
        }

        private PentaNoteType _saberDType = PentaNoteType.ColorB2;

        public PentaNoteType SaberDType
        {
            get => _saberDType;
            private set
            {
                if (_saberDType == value)
                    return;
                _saberDType = value;
                if (SaberD == null)
                {
                    Plugin.Log.Error($"SaberD is null");
                    return;
                }
                _modelManager.SetColor(SaberD.Saber, Plugin.Config.GetColor(value));
                //Plugin.Log.Error($"SaberB is {value} | {SaberBType}");
            }
        }

        public Saber? SaberA => _saberManager.leftSaber;
        public SiraSaber? SaberC;

        public Saber? SaberB => _saberManager.rightSaber;
        public SiraSaber? SaberD;

        public SiraSaber createNewSiraSaber(SaberType givenType, Color givenColor)
        {
            SiraSaber tempSaber = _siraSaberFactory.Spawn(givenType);
            if (givenType == SaberType.SaberA) { tempSaber.transform.SetParent(_saberManager.leftSaber.transform); }
            else { tempSaber.transform.SetParent(_saberManager.rightSaber.transform); }
            tempSaber.SetColor(givenColor);
            return tempSaber;
        }

        public PentaSaberController(IPentaColorManager colorManager, IInputController inputController,
            SaberModelManager modelManager, SaberManager saberManager, SiraSaberFactory siraSaberFactory)
        {
            Plugin.Log.Debug($"PentaSaberController constructed.");
            _colorManager = colorManager;
            _inputController = inputController;
            _modelManager = modelManager;
            _saberManager = saberManager;
            _siraSaberFactory = siraSaberFactory;
            HarmonyManager.ApplyDefaultPatches();
        }

        int SaberAID = 0;
        int SaberBID = 0;
        int SaberCID = 0;
        int SaberDID = 0;
        public void Initialize()
        {
            Instance = this;
            if (PluginConfig.Instance.Enabled)//disable double enable if it is somehow still set at launch
            {
                if (PluginConfig.Instance.SeptaEnabled) { PluginConfig.Instance.SeptaEnabled = false; }
            }
            if (PluginConfig.Instance.maulMode)
            {
                if (PluginConfig.Instance.SeptaEnabled) { PluginConfig.Instance.SeptaEnabled = false; }
                if (PluginConfig.Instance.Enabled)
                {
                    SaberC = createNewSiraSaber(SaberType.SaberA, Plugin.Config.GetColor(PentaNoteType.ColorA2));
                    SaberD = createNewSiraSaber(SaberType.SaberB, Plugin.Config.GetColor(PentaNoteType.ColorB2));
                }
                else
                {
                    SaberC = createNewSiraSaber(SaberType.SaberA, Plugin.Config.GetColor(PentaNoteType.ColorA1));
                    _saberCType = PentaNoteType.ColorA1;
                    SaberD = createNewSiraSaber(SaberType.SaberB, Plugin.Config.GetColor(PentaNoteType.ColorB1));
                    _saberDType = PentaNoteType.ColorB1;
                }
            }
            if (_colorManager.DisableScoreSubmission)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("PentaSaber");
                Plugin.Log.Debug("Disabling score submission.");
            }
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

        public PentaNoteType GetCurrentSaberTypeBySaber(int givenSaber)
        {
            if (givenSaber == SaberAID) return SaberAType;
            else if (givenSaber == SaberBID) return SaberBType;
            else if (givenSaber == SaberCID) return SaberCType;
            else if (givenSaber == SaberDID) return SaberDType;
            else return PentaNoteType.None;
        }

        public void Dispose()
        {
            Plugin.Log.Debug($"Disposing PentaSaberController, {_activeNotes.Count} notes remaining in dictionary.");
            _activeNotes.Clear();
            HarmonyManager.UnpatchAll();
        }

        bool saberCSet = false;
        bool saberDSet = false;

        public void Tick()
        {
            if(Instance != this)
            {
                Plugin.Log.Error($"WRONG INSTANCE");
                Instance = this;
            }

            if (SaberA != null && SaberAID == 0)
            {
                SaberAID = SaberA.GetInstanceID();
            }
            if (SaberB != null && SaberBID == 0)
            {
                SaberBID = SaberB.GetInstanceID();
            }

            if (PluginConfig.Instance.maulMode)
            {
                if (SaberD != null && SaberB != null && !saberDSet)
                {
                    SaberD.transform.position = SaberB.transform.position;
                    SaberD.transform.position = SaberD.transform.position - new Vector3(0, 0, .10f);
                    SaberD.transform.rotation = SaberB.transform.rotation;
                    SaberD.transform.rotation = new Quaternion(0, 1, 0, 0) * SaberD.transform.rotation;
                    saberDSet = true;
                }
                if (SaberC != null && SaberA != null && !saberCSet)
                {
                    SaberC.transform.position = SaberA.transform.position;
                    SaberC.transform.position = SaberC.transform.position - new Vector3(0, 0, .10f);
                    SaberC.transform.rotation = SaberA.transform.rotation;
                    SaberC.transform.rotation = new Quaternion(0, 1, 0, 0) * SaberC.transform.rotation;
                    saberCSet = true;
                }
                if (SaberC != null && SaberCID == 0)
                {
                    SaberCID = SaberC.Saber.GetInstanceID();
                }
                if (SaberD != null && SaberDID == 0)
                {
                    SaberDID = SaberD.Saber.GetInstanceID();
                }
            }

            if (!PluginConfig.Instance.maulMode)
            {
                switch (_inputController.SaberAState)
                {
                    case 0:
                        SaberAType = PentaNoteType.ColorA1;
                        break;
                    case 1:
                        SaberAType = PentaNoteType.ColorA2;
                        break;
                    case 2:
                        SaberAType = PentaNoteType.ColorA3;
                        break;
                }
                switch (_inputController.SaberBState)
                {
                    case 0:
                        SaberBType = PentaNoteType.ColorB1;
                        break;
                    case 1:
                        SaberBType = PentaNoteType.ColorB2;
                        break;
                    case 2:
                        SaberBType = PentaNoteType.ColorB3;
                        break;
                }
            }
        }
    }

    public enum PentaNoteType
    {
        None = -1,
        ColorA1 = 0,
        ColorB1 = 1,
        ColorA2 = 2,
        ColorB2 = 3,
        ColorA3 = 5,
        ColorB3 = 6,
        Neutral = 4,
        Neutral2 = 7
    }
}
