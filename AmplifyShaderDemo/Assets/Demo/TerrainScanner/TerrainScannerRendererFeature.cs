using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainScannerRendererFeature : ScriptableRendererFeature
{
    public Material UsedMaterial;
    public RenderPassEvent PassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

    TerrainScannerPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new TerrainScannerPass(UsedMaterial)
        {
            renderPassEvent = PassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var dest = RenderTargetHandle.CameraTarget;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}

public class TerrainScannerPass : ScriptableRenderPass
{
    static readonly string k_RenderTag = "Common PostProcessing";

    private Material m_Material;

    int id = Shader.PropertyToID("_Temp");

    RenderTargetIdentifier src, temp;
    private RenderTargetHandle destination { get; set; }

    public TerrainScannerPass(Material material)
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

        Blit(cmd, src, temp, m_Material, 0);
        Blit(cmd, temp, src);
    }
}