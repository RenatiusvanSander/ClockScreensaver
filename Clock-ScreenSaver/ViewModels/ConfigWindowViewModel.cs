using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clock_ScreenSaver.Models.LogicModel;

namespace Clock_ScreenSaver.ViewModels
{

    /// <summary>
    /// Interaction logic for ConfigWindow.
    /// </summary>
    public class ConfigWindowViewModel
    {

        private ConfigRegistryHandler configRegistryHandler;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ConfigWindowViewModel()
        {

        }

        /// <summary>
        /// Initializes all members in the view model.
        /// </summary>
        private void InitConfigWindowViewModel()
        {
            configRegistryHandler = new ConfigRegistryHandler();
        }

        /// <summary>
        /// Reads config out of the user registry.
        /// </summary>
        private void ReadConfig()
        {

        }

        /// <summary>
        /// Write the configuration into user's registry.
        /// </summary>
        private void WriteConfig()
        {

        }
    }
}
