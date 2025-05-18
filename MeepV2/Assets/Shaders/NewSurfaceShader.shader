Shader "Custom/FlashlightBlindingURP"
{
    Properties
    {
        _LightColor("Light Color", Color) = (1, 1, 1, 1)
        _Intensity("Light Intensity", Float) = 10
        _AngleFalloff("Angle Falloff", Float) = 5
        _BloomThreshold("Bloom Threshold", Float) = 0.8
        _Alpha("Alpha Transparency", Range(0, 1)) = 0.5
        _GradientTex("Gradient Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            TEXTURE2D(_GradientTex);
            SAMPLER(sampler_GradientTex);
            half4 _LightColor;
            float _Intensity;
            float _AngleFalloff;
            float _BloomThreshold;
            float _Alpha;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.worldPos);
                float dotProduct = saturate(dot(viewDir, IN.worldNormal));

                float falloff = pow(dotProduct, _AngleFalloff);
                float bloom = smoothstep(_BloomThreshold, 1.0, falloff) * _Intensity;

                half gradient = SAMPLE_TEXTURE2D(_GradientTex, sampler_GradientTex, IN.uv).r;
                half finalAlpha = saturate(gradient * _Alpha + bloom);

                half3 color = _LightColor.rgb * (0.5 + bloom);

                return half4(color, finalAlpha);
            }
            ENDHLSL
        }
    }
}
