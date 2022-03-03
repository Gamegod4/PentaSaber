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
        public virtual bool Enabled { get; set; } = true;
        public virtual Color SaberA1 { get; set; } = Color.red;
        public virtual Color SaberA2 { get; set; } = Color.magenta;
        public virtual Color SaberB1 { get; set; } = Color.blue;
        public virtual Color SaberB2 { get; set; } = Color.green;
        public virtual Color Neutral { get; set; } = Color.grey;

        public bool AllowDualNeutral { get; set; } = false;
        public int NeutralBufferMin { get; set; } = 3;
        public int NeutralBufferMax { get; set; } = 3;
        public int MinColorDuration { get; set; } = 5;
        public int MaxColorDuration { get; set; } = 20;

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
