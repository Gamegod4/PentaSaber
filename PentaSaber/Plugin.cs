using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using PentaSaber.HarmonyPatches.Manager;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace PentaSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; } = null!;
        internal static PluginConfig Config { get; private set; } = null!;
        /// <summary>
        /// Use to send log messages through BSIPA.
        /// </summary>
        internal static IPALogger Log { get; private set; } = null!;

        [Init]
        public Plugin(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            Config = conf.Generated<PluginConfig>();

            PluginConfig.Instance = Config;
            if (PluginConfig.Instance.Enabled || PluginConfig.Instance.maulMode)//preventing double and maul enables
            {
                if (PluginConfig.Instance.SeptaEnabled) { PluginConfig.Instance.SeptaEnabled = false; }
            }
            if (PluginConfig.Instance.leftSecondaryButtonSelection == PluginConfig.Instance.leftTertiaryButtonSelection) { PluginConfig.Instance.leftTertiaryButtonSelection = 4; }
            if (PluginConfig.Instance.rightSecondaryButtonSelection == PluginConfig.Instance.rightTertiaryButtonSelection) { PluginConfig.Instance.rightTertiaryButtonSelection = 4; }
            zenjector.Install<PentaSaberInstaller>(Location.StandardPlayer);
            zenjector.Install<PentaSaberInstaller>(Location.MultiPlayer);

            zenjector.Install(Location.Menu, x => x.BindInterfacesTo<PentaSettingsUI>().AsSingle());//build mod menu
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Plugin.Log.Info("OnApplicationStart");
        }

        [OnExit]
        public void OnApplicationQuit()
        {

        }

    }
}
