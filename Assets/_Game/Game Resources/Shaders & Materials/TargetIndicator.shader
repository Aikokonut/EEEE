Shader "Custom/target Indicator"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Range(0, 100)) = 50
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" "LightMode" = "SRPDefaultUnlit"}
        LOD 100
        //ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                uniform float4 _MainTex_ST;
                float _Size;
            CBUFFER_END

            Varyings vert (Attributes attr)
            {
                Varyings output;

                attr.positionOS.z = 1;
                attr.positionOS.w = 1;
                attr.positionOS.x *= _ScreenParams.y / _ScreenParams.x;

                attr.positionOS.x *= _Size / 100;
                attr.positionOS.y *= _Size / 100;

                float4 centerCS = TransformObjectToHClip(float4(0,0,0,1));
                centerCS.xyz /= centerCS.w;
                centerCS.w = 0;

                output.positionCS = centerCS + attr.positionOS;
                output.positionCS.z = 0.1;
                output.positionCS.w = 1;

                output.uv = TRANSFORM_TEX(attr.uv, _MainTex);
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return col;
            }
            ENDHLSL
        }
    }
}