using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BeatSaberMarkupLanguage.Attributes;

namespace PentaSaber
{
    internal class PentaSettingsUI : BeatSaberMarkupLanguage.Components.NotifiableSingleton<PentaSettingsUI>
    {
        //#pragma warning disable IDE0052 // Remove unread private members
        [UIValue("colorChange-enabled")]
        private bool CommandEnabled
        {
            get => PluginConfig.Instance.Enabled;
            set
            {
                PluginConfig.Instance.Enabled = value;
                if (value)
                {
                    PluginConfig.Instance.SeptaEnabled = false;
                    NotifyPropertyChanged(nameof(CommandEnabled2));
                }
            }
        }
        [UIValue("colorChange2-enabled")]
        private bool CommandEnabled2
        {
            get => PluginConfig.Instance.SeptaEnabled;
            set
            {
                PluginConfig.Instance.SeptaEnabled = value;
                if (value)
                {
                    PluginConfig.Instance.Enabled = false;
                    NotifyPropertyChanged(nameof(CommandEnabled));
                }
            }
        }

        [UIValue("toggleLock-enabled")]
        private bool toggleCommandEnabled
        {
            get => PluginConfig.Instance.toggleLockEnabled;
            set => PluginConfig.Instance.toggleLockEnabled = value;
        }

        [UIValue("trueRandomSepta-enabled")]
        private bool toggleTrueRandomSeptaEnabled
        {
            get => PluginConfig.Instance.trueRandomSepta;
            set => PluginConfig.Instance.trueRandomSepta = value;
        }

        [UIValue("leftButton")]
        private int leftButtonDropdown
        {
            get => PluginConfig.Instance.leftSecondaryButtonSelection;
            set
            {
                PluginConfig.Instance.leftSecondaryButtonSelection = value;
                if (PluginConfig.Instance.leftTertiaryButtonSelection == value)
                {
                    PluginConfig.Instance.leftTertiaryButtonSelection = 4;
                    NotifyPropertyChanged(nameof(leftButtonDropdown2));
                }
            }
        }

        [UIValue("leftButtonThresh")]
        private float leftButtonThresholdDropdown
        {
            get => PluginConfig.Instance.leftSecondaryButtonThreshold;
            set => PluginConfig.Instance.leftSecondaryButtonThreshold = value;
        }

        [UIValue("leftButton2")]
        private int leftButtonDropdown2
        {
            get => PluginConfig.Instance.leftTertiaryButtonSelection;
            set
            {
                PluginConfig.Instance.leftTertiaryButtonSelection = value;
                if (PluginConfig.Instance.leftSecondaryButtonSelection == value)
                {
                    PluginConfig.Instance.leftSecondaryButtonSelection = 4;
                    NotifyPropertyChanged(nameof(leftButtonDropdown));
                }
            }
        }

        [UIValue("leftButtonThresh2")]
        private float leftButtonThresholdDropdown2
        {
            get => PluginConfig.Instance.leftTertiaryButtonThreshold;
            set => PluginConfig.Instance.leftTertiaryButtonThreshold = value;
        }

        [UIValue("leftButtonChoices")]
        private readonly List<object> leftChoices = new List<object> { 0, 1, 2, 3, 4 };

        [UIValue("rightButton")]
        private int rightButtonDropdown
        {
            get => PluginConfig.Instance.rightSecondaryButtonSelection;
            set
            {
                PluginConfig.Instance.rightSecondaryButtonSelection = value;
                if (PluginConfig.Instance.rightTertiaryButtonSelection == value)
                {
                    PluginConfig.Instance.rightTertiaryButtonSelection = 4;
                    NotifyPropertyChanged(nameof(rightButtonDropdown2));
                }
            }
        }

        [UIValue("rightButtonThresh")]
        private float rightButtonThresholdDropdown
        {
            get => PluginConfig.Instance.rightSecondaryButtonThreshold;
            set => PluginConfig.Instance.rightSecondaryButtonThreshold = value;
        }

        [UIValue("rightButton2")]
        private int rightButtonDropdown2
        {
            get => PluginConfig.Instance.rightTertiaryButtonSelection;
            set
            {
                PluginConfig.Instance.rightTertiaryButtonSelection = value;
                if (PluginConfig.Instance.rightSecondaryButtonSelection == value)
                {
                    PluginConfig.Instance.rightSecondaryButtonSelection = 4;
                    NotifyPropertyChanged(nameof(rightButtonDropdown));
                }
            }
        }

        [UIValue("rightButtonThresh2")]
        private float rightButtonThresholdDropdown2
        {
            get => PluginConfig.Instance.rightTertiaryButtonThreshold;
            set => PluginConfig.Instance.rightTertiaryButtonThreshold = value;
        }

        [UIValue("rightButtonChoices")]
        private readonly List<object> rightChoices = new List<object> { 0, 1, 2, 3, 4 };

        [UIValue("transitionButton")]
        private float transitionButtonDropdown
        {
            get => PluginConfig.Instance.transitionBlockerLength;
            set => PluginConfig.Instance.transitionBlockerLength = value;
        }

        [UIValue("oneToNineTenthChoicesTag")]
        private readonly List<object> oneToNineTenthChoices = new List<object> { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };

        [UIValue("P0ToP1TenthChoicesTag")]
        private readonly List<object> P0ToP1TenthChoices = new List<object> { 0.0f, 0.01f, 0.02f, 0.03f, 0.04f, 0.05f, 0.06f, 0.07f, 0.08f, 0.09f, 0.1f };

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

        [UIValue("UIleftTertiaryColor")]
        private UnityEngine.Color leftTertiaryColor
        {
            get => PluginConfig.Instance.SaberA3;
            set => PluginConfig.Instance.SaberA3 = value;
        }

        [UIValue("UIrightTertiaryColor")]
        private UnityEngine.Color rightTertiaryColor
        {
            get => PluginConfig.Instance.SaberB3;
            set => PluginConfig.Instance.SaberB3 = value;
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
                case 4:
                    return "Not Set";
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

        [UIAction("buttonsP0toP1Form")]
        private string numToP0ToP1Button(float t)
        {
            switch (t)
            {
                case 0.0f:
                    return "0.0";
                case 0.01f:
                    return "0.01";
                case 0.02f:
                    return "0.02";
                case 0.03f:
                    return "0.03";
                case 0.04f:
                    return "0.04";
                case 0.05f:
                    return "0.05";
                case 0.06f:
                    return "0.06";
                case 0.07f:
                    return "0.07";
                case 0.08f:
                    return "0.08";
                case 0.09f:
                    return "0.09";
                case 1.0f:
                    return "1.0";
                default:
                    return "";
            }
        }
    }
}
