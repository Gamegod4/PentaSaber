using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PentaSaber
{
    public class PluginConfig
    {
        private static PluginConfig? _instance;

        public static PluginConfig Instance
        {
            get => _instance ?? throw new System.InvalidOperationException("PentaConfig instance not yet created.");
            set => _instance = value;
        }

        public virtual bool Enabled { get; set; } = false;
        public virtual bool SeptaEnabled { get; set; } = false;
        public virtual bool toggleLockEnabled { get; set; } = false;
        public virtual bool trueRandomSepta { get; set; } = true;
        public virtual bool maulMode { get; set; } = false;
        public virtual bool maulFlipBack { get; set; } = false;
        public virtual bool singleColorNeutral { get; set; } = true;
        public virtual bool neutralOnlyMode { get; set; } = false;
        public virtual int leftSecondaryButtonSelection { get; set; } = 0;
        public virtual float leftSecondaryButtonThreshold { get; set; } = 0.5f;
        public virtual int leftTertiaryButtonSelection { get; set; } = 0;
        public virtual float leftTertiaryButtonThreshold { get; set; } = 0.5f;
        public virtual int rightSecondaryButtonSelection { get; set; } = 0;
        public virtual float rightSecondaryButtonThreshold { get; set; } = 0.5f;
        public virtual int rightTertiaryButtonSelection { get; set; } = 0;
        public virtual float rightTertiaryButtonThreshold { get; set; } = 0.5f;
        public virtual float transitionBlockerLength { get; set; } = 0.05f;
        public virtual int minDuration { get; set; } = 10;
        public virtual int maxDuration { get; set; } = 20;
        public virtual int neutralBufferNumber { get; set; } = 3;
        public virtual Color SaberA1 { get; set; } = colorPresets.Red;
        public virtual Color SaberA2 { get; set; } = colorPresets.Violet;
        public virtual Color SaberA3 { get; set; } = colorPresets.Yellow;
        public virtual Color SaberB1 { get; set; } = colorPresets.Blue;
        public virtual Color SaberB2 { get; set; } = colorPresets.Green;
        public virtual Color SaberB3 { get; set; } = colorPresets.Cyan;
        public virtual Color NeutralLeft { get; set; } = colorPresets.Gray;
        public virtual Color NeutralRight { get; set; } = colorPresets.Gray;

        public Color GetColor(PentaNoteType pentaNoteType)
        {
            return pentaNoteType switch
            {
                PentaNoteType.ColorA1 => SaberA1,
                PentaNoteType.ColorB1 => SaberB1,
                PentaNoteType.ColorA2 => SaberA2,
                PentaNoteType.ColorB2 => SaberB2,
                PentaNoteType.ColorA3 => SaberA3,
                PentaNoteType.ColorB3 => SaberB3,
                PentaNoteType.Neutral => NeutralLeft,
                PentaNoteType.Neutral2 => NeutralRight,
                _ => Color.black
            };
        }
    }

    public class colorPresets
    {
        static public Color Red = new Color(1, 0, 0, 1);
        static public Color Orange = new Color(1, .6445f, 0, 1);
        static public Color Yellow = new Color(1, 1, 0, 1);
        static public Color Green = new Color(0, 1, 0, 1);
        static public Color Blue = new Color(0, 0, 1, 1);
        static public Color Indigo = new Color(.2930f, 0, .5078f, 1);
        static public Color Violet = new Color(.9297f, .5078f, .9297f, 1);
        static public Color Brown = new Color(.6445f, .1641f, .1641f, 1);
        static public Color Cyan = new Color(0, 1, 1, 1);
        static public Color Black = new Color(0, 0, 0, 1);
        static public Color White = new Color(1, 1, 1, 1);
        static public Color Gray = new Color(.5f, .5f, .5f, 1);
    }
}
