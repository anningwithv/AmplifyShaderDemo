Shader "Unlit/ShaderStudy"
{
    Properties
    {
        _FogRadius ("Fog Radius" , Range(1, 20)) = 12
        _FogBlend ("Fog Blend", Range(0.01, 1)) = 0.35
        _FogColor ("Fog Color", Color) = (0.2, 0.2, 0.2, 0.6)
    }
    SubShader
    {
        Tags {"RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" "UniversalMaterialType"="Unlit" }
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _FogRadius;
            float _FogBlend;
            float4 _FogColor;
            int _UnitPos_Length = 0;
            float2 _UnitPos[3000];

            struct appdata
            {
                float4 vertex : POSITION;
            };

            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos: TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                // o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                // o.worldNormal = v.normal;
                return o;
            }

            // float invLerp(float from, float to, float value){
            //     return (value - from) / (to - from);
            // }

            // float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value){
            //     float rel = invLerp(origFrom, origTo, value);
            //     return lerp(targetFrom, targetTo, rel);
            // }

            float getAlphaLerp(float2 worldPos, float2 uPos){
                float threshold = _FogRadius * (1 - _FogBlend);
                float lerpRatio = _FogRadius - threshold;
                float alphaLerp = 1;

                float cDist = distance(worldPos, uPos);

                if (cDist > _FogRadius){
                    alphaLerp = 1;
                }
                else if (cDist >= threshold){
                    alphaLerp = saturate((cDist - threshold) / lerpRatio);
                }
                else {
                    alphaLerp = 0;
                }

                return alphaLerp;
            }

            half4 frag (v2f i) : SV_Target
            {
                float alphaLerp = 1;
                if (_UnitPos_Length > 0){
                    for (int x = 0; x < _UnitPos_Length; x++){
                        alphaLerp = min(alphaLerp, getAlphaLerp(float2(i.worldPos.x, i.worldPos.z), _UnitPos[x]));
                    }
                }

                return half4(_FogColor.xyz, _FogColor.w * alphaLerp);
            }
            ENDCG
        }
    }
}
