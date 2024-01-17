using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class CommonRendererFeature : ScriptableRendererFeature
{
    public Material UsedMaterial;
    public RenderPassEvent PassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

    CommonPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CommonPass(UsedMaterial)
        {
            renderPassEvent = PassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var dest = RenderTargetHandle.CameraTarget;
        m_ScriptablePass.Setup(renderer.cameraColorTarget, dest);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}

public class CommonPass : ScriptableRenderPass
{
    static readonly string k_RenderTag = "Common PostProcessing";

    private Material m_Material;

    int id = Shader.PropertyToID("_Temp");

    RenderTargetIdentifier src, temp;
    private RenderTargetHandle destination { get; set; }

    public CommonPass(Material material)
    {
        m_Material = material;

        if (material == null)
        {
            material = CoreUtils.CreateEngineMaterial("Unlit/URPScreenTint");
        }
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        base.OnCameraSetup(cmd, ref renderingData);

        src = renderingData.cameraData.renderer.cameraColorTarget;

        RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(id, desc, FilterMode.Bilinear);
        temp = new RenderTargetIdentifier(id);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (m_Material == null)
        {
            return;
        }
        if (!renderingData.cameraData.postProcessEnabled) return;

        var cmd = CommandBufferPool.Get(k_RenderTag);

        VolumeStack volumes = VolumeManager.instance.stack;
        CommonPostVolume cpv = volumes.GetComponent<CommonPostVolume>();
        if (cpv.IsActive())
        {
            m_Material.SetFloat("_Intensity", (float)cpv.Intensity);
            m_Material.SetColor("_OverlayColor", (Color)cpv.Color);
        }

        Render(cmd, ref renderingData);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        base.OnCameraCleanup(cmd);

        cmd.ReleaseTemporaryRT(id);
    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isSceneViewCamera) return;

        //cmd.Blit(src, dest, m_Material, 0);
        //cmd.Blit(dest, src);
        Blit(cmd, src, temp, m_Material, 0);
        Blit(cmd, temp, src);
    }

    public void Setup(in RenderTargetIdentifier currentTarget, RenderTargetHandle dest)
    {
        //this.destination = dest;
        //this.src = currentTarget;
    }
}