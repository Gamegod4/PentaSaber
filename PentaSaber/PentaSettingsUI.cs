using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BeatSaberMarkupLanguage.Attributes;

namespace PentaSaber
{
    internal class PentaSettingsUI : PersistentSingleton<PentaSettingsUI>
    {
        //#pragma warning disable IDE0052 // Remove unread private members
        [UIValue("colorChange-enabled")]
        private bool CommandEnabled
        {
            get => PluginConfig.Instance.Enabled;
            set => PluginConfig.Instance.Enabled = value;
        }

        [UIValue("toggleLock-enabled")]
        private bool toggleCommandEnabled
        {
            get => PluginConfig.Instance.toggleLockEnabled;
            set => PluginConfig.Instance.toggleLockEnabled = value;
        }

        [UIValue("toggleDual-enabled")]
        private bool toggleDualEnabled
        {
            get => PluginConfig.Instance.AllowDualNeutral;
            set => PluginConfig.Instance.AllowDualNeutral = value;
        }

        [UIValue("leftButton")]
        private int leftButtonDropdown
        {
            get => PluginConfig.Instance.leftButtonSelection;
            set => PluginConfig.Instance.leftButtonSelection = value;
        }

        [UIValue("leftButtonThresh")]
        private float leftButtonThresholdDropdown
        {
            get => PluginConfig.Instance.leftButtonThreshold;
            set => PluginConfig.Instance.leftButtonThreshold = value;
        }

        [UIValue("leftButtonChoices")]
        private readonly List<object> leftChoices = new List<object> { 0, 1, 2, 3 };

        [UIValue("rightButton")]
        private int rightButtonDropdown
        {
            get => PluginConfig.Instance.rightButtonSelection;
            set => PluginConfig.Instance.rightButtonSelection = value;
        }

        [UIValue("rightButtonThresh")]
        private float rightButtonThresholdDropdown
        {
            get => PluginConfig.Instance.rightButtonThreshold;
            set => PluginConfig.Instance.rightButtonThreshold = value;
        }

        [UIValue("rightButtonChoices")]
        private readonly List<object> rightChoices = new List<object> { 0, 1, 2, 3 };

        [UIValue("oneToNineTenthChoicesTag")]
        private readonly List<object> oneToNineTenthChoices = new List<object> { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };

        [UIValue("UIleftPrimaryColor")]
        private UnityEngine.Color leftPrimaryColor
        {
            get => PluginConfig.Instance.SaberA1;
            set => PluginConfig.Instance.SaberA1 = value;
        }

        [UIValue("UIrightPrimaryColor")]
        private UnityEngine.Color rightPrimaryColor
        {
            get => PluginConfig.Instance.SaberB1;
            set => PluginConfig.Instance.SaberB1 = value;
        }

        [UIValue("UIleftSecondaryColor")]
        private UnityEngine.Color leftSecondaryColor
        {
            get => PluginConfig.Instance.SaberA2;
            set => PluginConfig.Instance.SaberA2 = value;
        }

        [UIValue("UIrightSecondaryColor")]
        private UnityEngine.Color rightSecondaryColor
        {
            get => PluginConfig.Instance.SaberB2;
            set => PluginConfig.Instance.SaberB2 = value;
        }

        [UIValue("UIneutralColor")]
        private UnityEngine.Color neutralColor
        {
            get => PluginConfig.Instance.Neutral;
            set => PluginConfig.Instance.Neutral = value;
        }

        [UIAction("buttonsForm")]
        private string numToButton(int t)
        {
            switch (t)
            {
                case 0:
                    return "Trigger Button";
                case 1:
                    return "Grip";
                case 2:
                    return "Primary Button";
                case 3:
                    return "Secondary Button";
                default:
                    return "";
            }
        }

        [UIAction("buttonsOneToNineForm")]
        private string numToOneToNineButton(float t)
        {
            switch (t)
            {
                case 0.1f:
                    return "0.1";
                case 0.2f:
                    return "0.2";
                case 0.3f:
                    return "0.3";
                case 0.4f:
                    return "0.4";
                case 0.5f:
                    return "0.5";
                case 0.6f:
                    return "0.6";
                case 0.7f:
                    return "0.7";
                case 0.8f:
                    return "0.8";
                case 0.9f:
                    return "0.9";
                default:
                    return "";
            }
        }
    }
}
