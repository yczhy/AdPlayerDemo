using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duskvern
{
    public class Panelconfig
    {
        
    }

    [CreateAssetMenu(fileName = "UIConfig", menuName = "Duskvern//UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [ShowInInspector]
        private Dictionary<string, Panelconfig> panelconfig = new ();

        public Panelconfig GetPanelConfig(string panelname)
        {
            if (panelconfig.ContainsKey(panelname))
            {
                return panelconfig[panelname];
            }
            Logger.LogUI("Panelconfig not found");
            return null;
        }


    }

}
