using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class GrabPassRendererFeature : ScriptableRendererFeature
{
    public RenderPassEvent PassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

    GrabPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new GrabPass()
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

public class GrabPass : ScriptableRenderPass
{
    static readonly string k_RenderTag = "GrabRTPass";
    static readonly string k_GlobalFullSceneColorTexture = "_GlobalFullSceneColorTexture";

    int id = Shader.PropertyToID("_Temp");

    RenderTargetIdentifier src, temp;
    private RenderTargetHandle destination { get; set; }

    public GrabPass()
    {
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        base.OnCameraSetup(cmd, ref renderingData);

        src = renderingData.cameraData.renderer.cameraColorTarget;

        RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
        float downSample = 2;
        desc.width = Mathf.RoundToInt(desc.width / downSample);
        desc.height = Mathf.RoundToInt(desc.height / downSample);
        cmd.GetTemporaryRT(id, desc, FilterMode.Bilinear);
        temp = new RenderTargetIdentifier(id);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
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
        //if (renderingData.cameraData.isSceneViewCamera) return;

        Blit(cmd, src, temp);
        cmd.SetGlobalTexture(k_GlobalFullSceneColorTexture, temp);
    }
}