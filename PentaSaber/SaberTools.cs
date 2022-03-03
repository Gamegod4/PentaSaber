using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IPA.Utilities;

namespace PentaSaber
{
    public static class SaberTools
    {

        public static FieldAccessor<SaberModelController, SaberTrail>.Accessor accessSaberTrail
            = FieldAccessor<SaberModelController, SaberTrail>.GetAccessor("_saberTrail");
        public static FieldAccessor<SaberTrail, Color>.Accessor accessSaberTrailColor
            = FieldAccessor<SaberTrail, Color>.GetAccessor("_color");
        public static FieldAccessor<SaberModelController, SetSaberGlowColor[]>.Accessor accessSaberGlow
            = FieldAccessor<SaberModelController, SetSaberGlowColor[]>.GetAccessor("_setSaberGlowColors");
        public static FieldAccessor<SaberModelController, SetSaberFakeGlowColor[]>.Accessor accessSaberFakeGlow
            = FieldAccessor<SaberModelController, SetSaberFakeGlowColor[]>.GetAccessor("_setSaberFakeGlowColors");
        public static FieldAccessor<SaberModelController, TubeBloomPrePassLight>.Accessor accessSaberLight
            = FieldAccessor<SaberModelController, TubeBloomPrePassLight>.GetAccessor("_saberLight");

        public static void SetSaberColor(this SaberModelController saberModel, Color color)
        {
            SaberTrail trail = accessSaberTrail(ref saberModel);
            accessSaberTrailColor(ref trail) = color;
            SetSaberGlowColor[] glowColors = accessSaberGlow(ref saberModel);
            for(int i = 0; i < glowColors.Length; i++)
            {
                glowColors[i].SetColors();
            }
            SetSaberFakeGlowColor[] fakeGlowColors = accessSaberFakeGlow(ref saberModel);
            for (int i = 0; i < fakeGlowColors.Length; i++)
            {
                fakeGlowColors[i].SetColors();
            }
            TubeBloomPrePassLight? saberLight = accessSaberLight(ref saberModel);
            if(saberLight != null)
                saberLight.color = color;
        }
    }
}
