using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using EPOOutline;
using System;
using System.Reflection;

#if URP_OUTLINE

#if UNITY_6000_0_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
#endif

using UnityEngine.Rendering.Universal;

namespace EPOOutline
{
    public class URPOutlineFeature : ScriptableRendererFeature
    {
        private class SRPOutline : ScriptableRenderPass
        {
            private static List<Outlinable> temporaryOutlinables = new List<Outlinable>();

            public ScriptableRenderer Renderer;

            public bool UseColorTargetForDepth;

            public Outliner Outliner;

    #if UNITY_6000_0_OR_NEWER
            public OutlineParameters GraphParameters = new OutlineParameters(null);
    #endif
            
            public OutlineParameters Parameters = new OutlineParameters(new BasicCommandBufferWrapper(new CommandBuffer()));

            private List<Outliner> outliners = new List<Outliner>();

    #if UNITY_6000_0_OR_NEWER
            private UnsafeCommandBufferWrapper wrapper = new UnsafeCommandBufferWrapper();
            private Dictionary<RTHandle, TextureHandle> registeredHandles = new Dictionary<RTHandle, TextureHandle>();

            private void RegisterHandle(RTHandle handle, RenderGraph graph, IUnsafeRenderGraphBuilder builder, AccessFlags flags)
            {
                var imported = graph.ImportTexture(handle);
                builder.UseTexture(imported, flags); 
                registeredHandles[handle] = imported;
            }
            
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                var resourceData = frameData.Get<UniversalResourceData>();
                var cameraData = frameData.Get<UniversalCameraData>();

                using (var builder = renderGraph.AddUnsafePass("EPO Outline", out RenderFunctionParameter passData))
                {
                    registeredHandles.Clear();
                    GraphParameters.RTHandlePool.ReleaseAll();
                    passData.RenderTarget = resourceData.cameraColor.IsValid() ? resourceData.cameraColor : resourceData.backBufferColor;
                    passData.DepthTarget = resourceData.cameraDepth.IsValid() ? resourceData.cameraDepth : resourceData.backBufferDepth;
                    
                    Outlinable.GetAllActiveOutlinables(GraphParameters.OutlinablesToRender);

                    RendererFilteringUtility.Filter(cameraData.camera, GraphParameters);

                    GraphParameters.TargetWidth = cameraData.cameraTargetDescriptor.width;
                    GraphParameters.TargetHeight = cameraData.cameraTargetDescriptor.height;
                    
                    GraphParameters.ScaledBufferWidth = cameraData.cameraTargetDescriptor.width;
                    GraphParameters.ScaledBufferHeight = cameraData.cameraTargetDescriptor.height;

                    GraphParameters.Antialiasing = cameraData.cameraTargetDescriptor.msaaSamples;

                    GraphParameters.Viewport = new Rect(0, 0, GraphParameters.TargetWidth, GraphParameters.TargetHeight);

                    Outliner.UpdateSharedParameters(GraphParameters, cameraData.camera, cameraData.isSceneViewCamera, true, false);
                    Outliner.ReplaceHandles(GraphParameters);

                    builder.UseTexture(passData.RenderTarget, AccessFlags.ReadWrite);
                    builder.UseTexture(passData.DepthTarget, AccessFlags.ReadWrite);

                    RegisterHandle(GraphParameters.Handles.Target, renderGraph, builder, AccessFlags.ReadWrite);
                    RegisterHandle(GraphParameters.Handles.InfoTarget, renderGraph, builder, AccessFlags.ReadWrite);

                    RegisterHandle(GraphParameters.Handles.PrimaryTarget, renderGraph, builder, AccessFlags.ReadWrite);
                    RegisterHandle(GraphParameters.Handles.SecondaryTarget, renderGraph, builder, AccessFlags.ReadWrite);

                    RegisterHandle(GraphParameters.Handles.PrimaryInfoBufferTarget, renderGraph, builder, AccessFlags.ReadWrite);
                    RegisterHandle(GraphParameters.Handles.SecondaryInfoBufferTarget, renderGraph, builder, AccessFlags.ReadWrite);

                    foreach (var handle in GraphParameters.TextureHandleMap)
                        RegisterHandle(handle.Value, renderGraph, builder, AccessFlags.Read);
                    
                    wrapper.SetHandleMap(registeredHandles);                        
                    wrapper.SetRenderGraph(renderGraph);
                    builder.SetRenderFunc<RenderFunctionParameter>((data, ctx) =>
                        {
                            GraphParameters.Target = data.RenderTarget;
                            GraphParameters.DepthTarget = data.DepthTarget;

                            wrapper.SetCommandBuffer(ctx.cmd);
                            GraphParameters.Buffer = wrapper;
                            
                            Setup(GraphParameters);
                        });
                }
            }
    #endif

            private FieldInfo nameId = typeof(RenderTargetIdentifier).GetField("m_NameID", BindingFlags.NonPublic | BindingFlags.Instance);

    #pragma warning disable
            private bool IsDepthTextureAvailable(ScriptableRenderer renderer)
            {
    #if UNITY_2022_1_OR_NEWER
                return renderer.cameraDepthTargetHandle.rt != null;
    #else
                return (int)nameId.GetValue(GetDepthTarget(renderer)) != -1;
    #endif
            }

            private RenderTargetIdentifier GetDepthTarget(ScriptableRenderer renderer)
            {
                return
    #if UNITY_2022_1_OR_NEWER
                    Renderer.cameraDepthTargetHandle;
    #elif UNITY_2020_2_OR_NEWER
                    Renderer.cameraDepthTarget;
    #endif
            }

            private RenderTargetIdentifier GetColorTarget(ScriptableRenderer renderer)
            {
    #if UNITY_2022_1_OR_NEWER
                return renderer.cameraColorTargetHandle;
    #else
                return renderer.cameraColorTarget;
    #endif
            }
    #pragma warning restore

            private void Setup(OutlineParameters parameters)
            {
                if (Outliner.RenderingStrategy == OutlineRenderingStrategy.Default)
                {
                    OutlineEffect.SetupOutline(parameters);
                    parameters.BlitMesh = null;
                    parameters.MeshPool.ReleaseAllMeshes();
                }
                else
                {
                    temporaryOutlinables.Clear();
                    temporaryOutlinables.AddRange(parameters.OutlinablesToRender);

                    parameters.OutlinablesToRender.Clear();
                    parameters.OutlinablesToRender.Add(null);

                    foreach (var outlinable in temporaryOutlinables)
                    {
                        parameters.OutlinablesToRender[0] = outlinable;
                        OutlineEffect.SetupOutline(parameters);
                        parameters.BlitMesh = null;
                    }

                    parameters.MeshPool.ReleaseAllMeshes();
                }
            }
            
    #pragma warning disable
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var underlyingBuffer = (Parameters.Buffer as IUnderlyingBufferProvider).UnderlyingBuffer;
                underlyingBuffer.Clear();
                var outlineEffect = Outliner;
                if (outlineEffect == null || !outlineEffect.enabled)
                    return;

                Outlinable.GetAllActiveOutlinables(Parameters.OutlinablesToRender);

                Outliner.UpdateSharedParameters(Parameters, renderingData.cameraData.camera, renderingData.cameraData.isSceneViewCamera, false, false);

                RendererFilteringUtility.Filter(renderingData.cameraData.camera, Parameters);

                Parameters.TargetWidth = renderingData.cameraData.cameraTargetDescriptor.width;
                Parameters.TargetHeight = renderingData.cameraData.cameraTargetDescriptor.height;

                Parameters.Viewport = new Rect(0, 0, Parameters.TargetWidth, Parameters.TargetHeight);
                var scaledSize = Parameters.ScaledSize;
                Parameters.ScaledBufferWidth = scaledSize.ScaledWidth;
                Parameters.ScaledBufferHeight = scaledSize.ScaledHeight;

                Parameters.Antialiasing = renderingData.cameraData.cameraTargetDescriptor.msaaSamples;

                Parameters.Target = OutlineEffect.HandleSystem.Alloc(RenderTargetUtility.ComposeTarget(Parameters, GetColorTarget(Renderer)));
                Parameters.DepthTarget = OutlineEffect.HandleSystem.Alloc(RenderTargetUtility.ComposeTarget(Parameters, !IsDepthTextureAvailable(Renderer) ? GetColorTarget(Renderer) :
                        GetDepthTarget(Renderer)));
                
                Outliner.ReplaceHandles(Parameters);
                
                Setup(Parameters);
                
                context.ExecuteCommandBuffer(underlyingBuffer);
            }
    #pragma warning restore
        }

        private class Pool
        {
            private Stack<SRPOutline> outlines = new Stack<SRPOutline>();

            private List<SRPOutline> createdOutlines = new List<SRPOutline>();

            public SRPOutline Get()
            {
                if (outlines.Count == 0)
                {
                    outlines.Push(new SRPOutline());
                    createdOutlines.Add(outlines.Peek());
                }

                return outlines.Pop();
            }

            public void ReleaseAll()
            {
                outlines.Clear();
                foreach (var outline in createdOutlines)
                    outlines.Push(outline);
            }
        }

        private GameObject lastSelectedCamera;

        private Pool outlinePool = new Pool();

        private List<Outliner> outliners = new List<Outliner>();

        private bool GetOutlinersToRenderWith(RenderingData renderingData, List<Outliner> outliners)
        {
            outliners.Clear();
            var camera = renderingData.cameraData.camera.gameObject;
            camera.GetComponents(outliners);
            if (outliners.Count == 0)
            {
    #if UNITY_EDITOR
                if (renderingData.cameraData.isSceneViewCamera)
                {
                    var foundObject = Array.Find(
                        Array.ConvertAll(UnityEditor.Selection.gameObjects, x => x.GetComponent<Outliner>()),
                        x => x != null);

                    camera = foundObject?.gameObject ?? lastSelectedCamera;

                    if (camera == null)
                        return false;
                    
                    camera.GetComponents(outliners);
                }
                else
                    return false;
    #else
                    return false;
    #endif
            }

            var hasOutliners = outliners.Count > 0;
            if (hasOutliners)
                lastSelectedCamera = camera;

            return hasOutliners;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!GetOutlinersToRenderWith(renderingData, outliners))
                return;

            var additionalCameraData = renderingData.cameraData.camera.GetUniversalAdditionalCameraData();
            var activeStackCount = 0;
            if (additionalCameraData != null)
            {
                var stack = additionalCameraData.renderType == CameraRenderType.Overlay ? null : additionalCameraData.cameraStack;
                if (stack != null)
                {
                    foreach (var camera in stack)
                    {
                        if (camera != null && camera.isActiveAndEnabled)
                            activeStackCount++;
                    }
                }
            }

            foreach (var outliner in outliners)
            {
                var outline = outlinePool.Get();

                outline.Outliner = outliner;

                outline.Renderer = renderer;

                outline.renderPassEvent = outliner.RenderStage == RenderStage.AfterTransparents ? RenderPassEvent.AfterRenderingTransparents : RenderPassEvent.AfterRenderingOpaques;

                renderer.EnqueuePass(outline);
            }

            outlinePool.ReleaseAll();
        }

        public override void Create()
        {
        }
    }
}
#endif