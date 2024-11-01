using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
    public static class XRUtility
    {
        public static bool IsXRActive =>
            UnityEngine.XR.XRSettings.enabled &&
            UnityEngine.XR.XRSettings.isDeviceActive;

        public static RenderTextureDescriptor VRRenderTextureDescriptor => UnityEngine.XR.XRSettings.eyeTextureDesc;

        public static bool IsUsingVR(OutlineParameters parameters)
        {
            return IsXRActive &&
                   !parameters.IsEditorCamera &&
                   parameters.EyeMask != StereoTargetEyeMask.None;
        }
    }
    
    public static class RenderTargetUtility
    {
        private static RenderTextureFormat? hdrFormat = null;

        public static int GetDepthSliceForEye(StereoTargetEyeMask mask)
        {
            switch (mask)
            {
                case StereoTargetEyeMask.Both:
                    return -1;
                case StereoTargetEyeMask.None:
                case StereoTargetEyeMask.Left:
                    return 0;
                case StereoTargetEyeMask.Right:
                    return 1;
                default:
                    throw new ArgumentException("Unknown mode");
            }
        }

        public struct RenderTextureInfo
        {
            public readonly RenderTextureDescriptor Descriptor;

            public RenderTextureInfo(RenderTextureDescriptor descriptor)
            {
                Descriptor = descriptor;
            }
        }

        public static RenderTargetIdentifier ComposeTarget(OutlineParameters parameters, RenderTargetIdentifier target)
        {
            return new RenderTargetIdentifier(target, 0, CubemapFace.Unknown, GetDepthSliceForEye(parameters.EyeMask));
        }
        
        public static RenderTextureInfo GetTargetInfo(OutlineParameters parameters, int width, int height)
        {
            var rtFormat = parameters.UseHDR ? GetHDRFormat() : RenderTextureFormat.ARGB32;

            if (XRUtility.IsUsingVR(parameters))
            {
                var descriptor = UnityEngine.XR.XRSettings.eyeTextureDesc;
                descriptor.colorFormat = rtFormat;
                descriptor.width = width;
                descriptor.height = height;
                descriptor.depthBufferBits = 0;
                descriptor.msaaSamples = Mathf.Max(parameters.Antialiasing, 1);

                var eyesCount = parameters.EyeMask == StereoTargetEyeMask.Both ? VRTextureUsage.TwoEyes : VRTextureUsage.OneEye;
                descriptor.vrUsage = eyesCount;

                return new RenderTextureInfo(descriptor);
            }
            else
            {
                var descriptor = new RenderTextureDescriptor(width, height, rtFormat, 0);
                descriptor.dimension = TextureDimension.Tex2D;
                descriptor.msaaSamples = Mathf.Max(parameters.Antialiasing, 1);

                return new RenderTextureInfo(descriptor);
            }
        }

        public static RTHandle GetRT(OutlineParameters parameters, int width, int height, string name)
        {
            var info = GetTargetInfo(parameters, width, height);

#if UNITY_6000_0_OR_NEWER
            var result = OutlineEffect.HandleSystem.Alloc(width, height,
                info.Descriptor.graphicsFormat,
                TextureWrapMode.Clamp, TextureWrapMode.Clamp, TextureWrapMode.Clamp,
                info.Descriptor.volumeDepth,
                FilterMode.Bilinear,
                info.Descriptor.dimension,
                info.Descriptor.enableRandomWrite,
                info.Descriptor.useMipMap,
                info.Descriptor.autoGenerateMips,
                false,
                1,
                0.0f,
                (MSAASamples)info.Descriptor.msaaSamples,
                info.Descriptor.bindMS,
                info.Descriptor.useDynamicScale,
                info.Descriptor.useDynamicScaleExplicit,
                info.Descriptor.memoryless,
                info.Descriptor.vrUsage,
                name);
#else
            var result = OutlineEffect.HandleSystem.Alloc(width, height,
                info.Descriptor.volumeDepth,
                0,
                info.Descriptor.graphicsFormat,
                FilterMode.Bilinear,
                TextureWrapMode.Clamp,
                info.Descriptor.dimension,
                info.Descriptor.enableRandomWrite,
                info.Descriptor.useMipMap,
                info.Descriptor.autoGenerateMips,
                false,
                1,
                0.0f,
                (MSAASamples)info.Descriptor.msaaSamples,
                info.Descriptor.bindMS,
                info.Descriptor.useDynamicScale,
                info.Descriptor.memoryless,
#if UNITY_2022_1_OR_NEWER
                info.Descriptor.vrUsage,
#endif
                name);
#endif
            
            return result;
        }

        private static RenderTextureFormat GetHDRFormat()
        {
            if (hdrFormat.HasValue)
                return hdrFormat.Value;

            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
                hdrFormat = RenderTextureFormat.ARGBHalf;
            else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
                hdrFormat = RenderTextureFormat.ARGBFloat;
            else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB64))
                hdrFormat = RenderTextureFormat.ARGB64;
            else
                hdrFormat = RenderTextureFormat.ARGB32;

            return hdrFormat.Value;
        }
    }
}