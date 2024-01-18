using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("TerrainScannerVolume", typeof(UniversalRenderPipeline))]
public class TerrainScannerVolume : VolumeComponent, IPostProcessComponent
{
    public FloatParameter Intensity = new FloatParameter(1);
    public ColorParameter Color = new ColorParameter(UnityEngine.Color.white);
    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return true;
    }
}
