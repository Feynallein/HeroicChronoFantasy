using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager> {
    [SerializeField] LayerMask _ExempleLayer;

    public int SnakeLayer { get { return 6; } }

    // Return true if the collided layer is within selected layer mask
    public bool CollidedLayerIsSelectedLayer(LayerMask selectedLayer, LayerMask collidedLayer) {
        return (selectedLayer & (1 << collidedLayer)) > 0;
    }

    public bool CollidedWithExempleLayer(LayerMask collidedLayer) {
        return CollidedLayerIsSelectedLayer(_ExempleLayer, collidedLayer);
    }
}
