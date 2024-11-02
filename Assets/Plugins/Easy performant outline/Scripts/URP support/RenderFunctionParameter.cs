#if URP_OUTLINE && UNITY_6000_0_OR_NEWER
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace EPOOutline
{
    public class RenderFunctionParameter
    {
        public TextureHandle RenderTarget;
        public TextureHandle DepthTarget;
        
        public RenderFunctionParameter()
        {
            
        }
    }
}
#endif