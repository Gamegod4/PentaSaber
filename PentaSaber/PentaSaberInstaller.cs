using PentaSaber.ColorManagers;
using PentaSaber.InputControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace PentaSaber
{
    internal class PentaSaberInstaller : Installer
    {
        public override void InstallBindings()
        {
            if(Plugin.Config.Enabled || Plugin.Config.SeptaEnabled || Plugin.Config.maulMode)
            {
                Plugin.Log?.Debug("Binding dependencies");
                Container.Bind<IInputController>().To<StandardInputController>().AsTransient();
                Container.Bind<IPentaColorManager>().To<StandardColorManager>().AsTransient();
                Container.BindInterfacesAndSelfTo<PentaSaberController>().AsSingle();
            }
        }
    }
}
