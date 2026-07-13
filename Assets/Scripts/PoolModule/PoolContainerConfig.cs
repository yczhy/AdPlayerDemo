using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    [CreateAssetMenu(fileName = "PoolContainerConfig", menuName = "Duskvern/PoolContainerConfig")]
    public class PoolContainerConfig : IConfigBase
    {
        [SerializeField] private List<PoolContainerParam> poolContainerParams = new ();

        public List<PoolContainerParam> PoolContainerParams { get => poolContainerParams; }
    }

}
