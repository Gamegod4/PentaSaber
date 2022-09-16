using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaSaber.ColorManagers
{
    internal class StandardColorManager : IPentaColorManager
    {
        private class NoteTypeTracker
        {
            private readonly Random random;
            private const int maxIndex = 3;
            public int neutralBufferNumber { get; } = 0;
            public int minDuration { get; } = 5;
            public int maxDuration { get; } = 10;
            public float transitionBlockerLength { get; } = 0.05f;
            public int currentNeutralCount = 0;
            public bool timeTrigger = false;
            public bool firstRun = true;
            public float timeTracker = 0.0f;
            public float baseTimeNum = 0.5f;
            public readonly ColorType ColorType;
            public int RandomInt(int min, int max)
            {
                return random.Next(min, max);
            }
            public NoteTypeTracker(ColorType colorType)
            {
                ColorType = colorType;
                random = new Random((int)DateTime.Now.Ticks + ((int)colorType * 13));
                CurrentType = colorType == ColorType.ColorA ? PentaNoteType.ColorA1 : PentaNoteType.ColorB1;
                neutralBufferNumber = PluginConfig.Instance.neutralBufferNumber;
                minDuration = PluginConfig.Instance.minDuration;
                maxDuration = PluginConfig.Instance.maxDuration + 1;//counter random maxout
                transitionBlockerLength = PluginConfig.Instance.transitionBlockerLength;

                if (minDuration <= 0) { minDuration = 1; }
                if (maxDuration < minDuration) { maxDuration = minDuration + 1; }//counter random maxout
                if (neutralBufferNumber < 0) { neutralBufferNumber = 0; }
            }
            public PentaNoteType CurrentType { get; private set; }
            
            private int colorNum = 1;
            public void triggerColor()
            {
                int tempInt = 0;
                if (PluginConfig.Instance.SeptaEnabled)
                {
                    if (PluginConfig.Instance.trueRandomSepta)
                    {
                        if (colorNum == 1)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 1, false);
                            tempInt = RandomInt(1, 3);
                            if (tempInt == 1) { colorNum = 3; }
                            else { colorNum = 5; }
                        }
                        else if (colorNum == 3)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 3, false);
                            tempInt = RandomInt(1, 3);
                            if (tempInt == 1) { colorNum = 1; }
                            else { colorNum = 5; }
                        }
                        else if (colorNum == 5)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 5, false);
                            tempInt = RandomInt(1, 3);
                            if (tempInt == 1) { colorNum = 1; }
                            else { colorNum = 3; }
                        }
                    }
                    else
                    {
                        if (colorNum == 1)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 1, false);
                            tempInt = RandomInt(1, 3);
                            if (tempInt == 1) { colorNum = 3; }
                            else { colorNum = 5; }
                        }
                        else if (colorNum == 3)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 3, false);
                            colorNum = 1;
                        }
                        else if (colorNum == 5)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 5, false);
                            colorNum = 1;
                        }
                    }
                }
                else//penta
                {
                    if (PluginConfig.Instance.Enabled)
                    {
                        if (colorNum == 1)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 1, false);
                            colorNum = 3;
                        }
                        else if (colorNum == 3)
                        {
                            CurrentType = GetNoteTypeForIndex(ColorType, 3, false);
                            colorNum = 1;
                        }
                    }
                }
                
            }

            public void triggerNeutral(bool state) { CurrentType = GetNoteTypeForIndex(ColorType, colorNum, state); }

            public void advanceTimeTracker(GameNoteController note) { timeTracker = note.noteData.time + (baseTimeNum * RandomInt(minDuration, maxDuration)); }
        }

        public string Id => "Default";
        public string Name => "Default";
        public string? Description => "The default color manager.";
        public bool DisableScoreSubmission => true;
        private readonly NoteTypeTracker trackerA = new NoteTypeTracker(ColorType.ColorA);
        private readonly NoteTypeTracker trackerB = new NoteTypeTracker(ColorType.ColorB);

        public StandardColorManager()
        {
        }

        private readonly Random randomExt;
        public int RandomExtInt(int min, int max)
        {
            return randomExt.Next(min, max);
        }

        public PentaNoteType GetNextNoteType(GameNoteController note)
        {
            ColorType currentType = note.noteData.colorType;
            NoteTypeTracker noteTracker = currentType == ColorType.ColorA ? trackerA : trackerB;

            if (!PluginConfig.Instance.neutralOnlyMode)
            {
                if (noteTracker.timeTracker == 0) { noteTracker.advanceTimeTracker(note); }
                else if (note.noteData.time >= noteTracker.timeTracker) { noteTracker.timeTrigger = true; }

                if (noteTracker.CurrentType != PentaNoteType.Neutral && noteTracker.CurrentType != PentaNoteType.Neutral2)
                {
                    if ((noteTracker.timeTrigger && note.noteData.timeToPrevColorNote > noteTracker.transitionBlockerLength))
                    {
                        noteTracker.triggerColor();
                        if (noteTracker.neutralBufferNumber == 0) { noteTracker.timeTrigger = false; noteTracker.advanceTimeTracker(note); }
                        if (noteTracker.neutralBufferNumber > 0) { noteTracker.triggerNeutral(true); noteTracker.currentNeutralCount++; }
                    }
                }
                else
                {
                    if (noteTracker.currentNeutralCount < noteTracker.neutralBufferNumber) { noteTracker.currentNeutralCount++; }
                    else
                    {
                        if (noteTracker.neutralBufferNumber > 0) { noteTracker.currentNeutralCount = 0; noteTracker.triggerNeutral(false); }
                        noteTracker.timeTrigger = false;
                        noteTracker.advanceTimeTracker(note);
                    }
                }
            }
            else
            {
                if (PluginConfig.Instance.Enabled || PluginConfig.Instance.SeptaEnabled)//counters neutral during base
                {
                    if (noteTracker.firstRun)
                    {
                        noteTracker.triggerNeutral(true);
                        noteTracker.firstRun = false;
                    }
                }
            }

            return noteTracker.CurrentType;
        }

        private static PentaNoteType GetNoteTypeForIndex(ColorType colorType, int index, bool allowNeutral)
        {
            PentaNoteType neutralSwitchingNoteType = PentaNoteType.Neutral;
            if (!PluginConfig.Instance.singleColorNeutral)
            {
                neutralSwitchingNoteType = PentaNoteType.Neutral2;
            }

            PentaNoteType retVal = PentaNoteType.None;
            retVal = (colorType, index, allowNeutral) switch
            {
                (ColorType.ColorA, 0, _) => PentaNoteType.ColorA1,
                (ColorType.ColorA, 1, true) => PentaNoteType.Neutral,
                (ColorType.ColorA, 1, false) => PentaNoteType.ColorA1,
                (ColorType.ColorA, 2, _) => PentaNoteType.ColorA2,
                (ColorType.ColorA, 3, true) => PentaNoteType.Neutral,
                (ColorType.ColorA, 3, false) => PentaNoteType.ColorA2,
                (ColorType.ColorA, 4, _) => PentaNoteType.ColorA3,
                (ColorType.ColorA, 5, true) => PentaNoteType.Neutral,
                (ColorType.ColorA, 5, false) => PentaNoteType.ColorA3,

                (ColorType.ColorB, 0, _) => PentaNoteType.ColorB1,
                (ColorType.ColorB, 1, true) => neutralSwitchingNoteType,
                (ColorType.ColorB, 1, false) => PentaNoteType.ColorB1,
                (ColorType.ColorB, 2, _) => PentaNoteType.ColorB2,
                (ColorType.ColorB, 3, true) => neutralSwitchingNoteType,
                (ColorType.ColorB, 3, false) => PentaNoteType.ColorB2,
                (ColorType.ColorB, 4, _) => PentaNoteType.ColorB3,
                (ColorType.ColorB, 5, true) => neutralSwitchingNoteType,
                (ColorType.ColorB, 5, false) => PentaNoteType.ColorB3,

                _ => PentaNoteType.None
            };
            if (retVal == PentaNoteType.None)
            {
                Plugin.Log.Error($"Returning PentaNoteType.None: {colorType} | {index} | {allowNeutral}");
            }
            return retVal;
        }
    }
}
