using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeatmapSaveData;

namespace PentaSaber.ColorManagers
{
    internal class StandardColorManager : IPentaColorManager
    {
        private class NoteTypeTracker
        {
            private const int maxIndex = 3;
            public bool allowDualNeutral { get; } = false;
            public int neutralBufferMin { get; } = 3;
            public int neutralBufferMax { get; } = 3;
            public int minColorDuration { get; } = 5;
            public int maxColorDuration { get; } = 20;
            public readonly ColorType ColorType;
            public NoteTypeTracker(ColorType colorType)
            {
                ColorType = colorType;
                CurrentDuration = UnityEngine.Random.Range(minColorDuration, maxColorDuration + 1);
                ResetNeutralBuffer();
                CurrentType = colorType == ColorType.ColorA ? PentaNoteType.ColorA1 : PentaNoteType.ColorB1;
                if (NeutralBuffer > 0)
                {
                    NextType = PentaNoteType.Neutral;
                }
                else
                {
                    NextType = colorType == ColorType.ColorA ? PentaNoteType.ColorA2 : PentaNoteType.ColorB2;
                }
                allowDualNeutral = Plugin.Config.AllowDualNeutral;
                neutralBufferMin = Plugin.Config.NeutralBufferMin;
                neutralBufferMax = Plugin.Config.NeutralBufferMax;
                minColorDuration = Plugin.Config.MinColorDuration;
                maxColorDuration = Plugin.Config.MaxColorDuration;
            }
            private int _current;
            public int NeutralBuffer;
            public PentaNoteType CurrentType { get; private set; }
            public PentaNoteType NextType { get; private set; }
            public int Current
            {
                get => _current;
                private set
                {
                    if (_current == value)
                        return;
                    if (value > maxIndex)
                        value = 0;
                    _current = value;
                    CurrentCount = 0;
                    Next = value + 1;
                    if (Next > maxIndex)
                        Next = 0;
                    CurrentType = GetNoteTypeForIndex(ColorType, Current, NeutralBuffer > 0);
                    NextType = GetNoteTypeForIndex(ColorType, Next, NeutralBuffer > 0);
                }
            }
            public int Next { get; private set; }
            public int CurrentCount { get; private set; }
            public int CurrentDuration { get; set; }
            public int LastNeutral { get; private set; }
            public bool Increment(bool blockTransition)
            {
                Plugin.Log.Debug($"{ColorType} : {CurrentType} : {CurrentCount}/{CurrentDuration}{(blockTransition ? " (transition blocked)" : "")}");
                CurrentCount++;
                bool previousNeutral = CurrentType == PentaNoteType.Neutral;
                bool transitioned = false;
                if (LastNeutral > 0)
                    LastNeutral++;
                if (CurrentCount >= CurrentDuration && !blockTransition)
                {
                    Current++;
                    transitioned = true;
                }
                if (transitioned)
                {
                    if (CurrentType != PentaNoteType.Neutral)
                    {
                        if (previousNeutral)
                            LastNeutral = 1;
                        CurrentDuration = UnityEngine.Random.Range(minColorDuration, maxColorDuration + 1);
                        ResetNeutralBuffer();
                    }
                    else
                    {
                        LastNeutral = 0;
                        CurrentDuration = NeutralBuffer;
                    }
                }
                return transitioned;
            }

            private void ResetNeutralBuffer()
            {
                if (neutralBufferMax >= neutralBufferMin && neutralBufferMax > 0)
                {
                    NeutralBuffer = UnityEngine.Random.Range(neutralBufferMin, neutralBufferMax);
                }
                else
                {
                    NeutralBuffer = 0;
                }
            }

        }

        public string Id => "Default";

        public string Name => "Default";

        public string? Description => "The default color manager.";

        private readonly NoteTypeTracker trackerA = new NoteTypeTracker(ColorType.ColorA);
        private readonly NoteTypeTracker trackerB = new NoteTypeTracker(ColorType.ColorB);

        public StandardColorManager()
        {
        }

        public PentaNoteType GetNextNoteType(GameNoteController note)
        {
            ColorType currentType = note.noteData.colorType;
            NoteTypeTracker noteTracker = currentType == ColorType.ColorA ? trackerA : trackerB;
            NoteTypeTracker otherTracker = currentType == ColorType.ColorA ? trackerB : trackerA;
            bool blockTransition = !noteTracker.allowDualNeutral
                                    && otherTracker.CurrentType == PentaNoteType.Neutral
                                    && noteTracker.NextType == PentaNoteType.Neutral
                                    && otherTracker.LastNeutral < 5;
            if (blockTransition)
                Plugin.Log.Debug($"Blocking transition for {currentType}");
            bool transitioned = noteTracker.Increment(blockTransition);


            return noteTracker.CurrentType;
        }

        private static PentaNoteType GetNoteTypeForIndex(ColorType colorType, int index, bool allowNeutral)
        {
            PentaNoteType retVal = (colorType, index, allowNeutral) switch
            {
                (ColorType.ColorA, 0, _) => PentaNoteType.ColorA1,
                (ColorType.ColorA, 1, true) => PentaNoteType.Neutral,
                (ColorType.ColorA, 1, false) => PentaNoteType.ColorA1,
                (ColorType.ColorA, 2, _) => PentaNoteType.ColorA2,
                (ColorType.ColorA, 3, true) => PentaNoteType.Neutral,
                (ColorType.ColorA, 3, false) => PentaNoteType.ColorA2,

                (ColorType.ColorB, 0, _) => PentaNoteType.ColorB1,
                (ColorType.ColorB, 1, true) => PentaNoteType.Neutral,
                (ColorType.ColorB, 1, false) => PentaNoteType.ColorB1,
                (ColorType.ColorB, 2, _) => PentaNoteType.ColorB2,
                (ColorType.ColorB, 3, true) => PentaNoteType.Neutral,
                (ColorType.ColorB, 3, false) => PentaNoteType.ColorB2,

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
