using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Package.Internal
{
    public class LayerUtilities
    {
        public void LayersInMask(LayerMask layerMask, List<int> layers)
        {
            if (layers == null)
                layers = new List<int>(16);
            
            var mask = layerMask.value;
            for (int i = 0; i < 8 * sizeof(int); mask >>= 1, i++)
                if ((mask & 1) == 1)
                    layers.Add(i);
        }
    }
}