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
        
        public virtual bool Enabled { get; set; } = true;
        public virtual bool toggleLockEnabled { get; set; } = true;
        public virtual int leftButtonSelection { get; set; } = 0;
        public virtual int rightButtonSelection { get; set; } = 0;
        public virtual float leftButtonThreshold { get; set; } = 0.5f;
        public virtual float rightButtonThreshold { get; set; } = 0.5f;
        public virtual Color SaberA1 { get; set; } = Color.red;
        public virtual Color SaberA2 { get; set; } = Color.magenta;
        public virtual Color SaberB1 { get; set; } = Color.blue;
        public virtual Color SaberB2 { get; set; } = Color.green;
        public virtual Color Neutral { get; set; } = Color.grey;

        public bool AllowDualNeutral { get; set; } = false;
        public int NeutralBufferMin { get; set; } = 1;
        public int NeutralBufferMax { get; set; } = 3;
        public int MinMainColorDuration { get; set; } = 8;
        public int MaxMainColorDuration { get; set; } = 40;
        public int MinAltColorDuration { get; set; } = 5;
        public int MaxAltColorDuration { get; set; } = 12;

        public Color GetColor(PentaNoteType pentaNoteType)
        {
            return pentaNoteType switch
            {
                PentaNoteType.ColorA1 => SaberA1,
                PentaNoteType.ColorB1 => SaberB1,
                PentaNoteType.ColorA2 => SaberA2,
                PentaNoteType.ColorB2 => SaberB2,
                PentaNoteType.Neutral => Neutral,
                _ => Color.black
            };
        }

    }
}
