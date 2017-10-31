using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData  {
    public int CurrentLayerIndex;
    public List<int> EnableLayers = new List<int>();
}
