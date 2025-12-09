Shader "Custom/URP/ParkourFloor"
{
    Properties
    {
        _BaseMap ("Albedo", 2D) = "white" {}
        _BaseColor ("Tint Color", Color) = (1,1,1,1)
        _Tile ("Texture Tiling", Float) = 1

        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5

        _RimColor ("Rim Color", Color) = (0.4,0.4,0.4,1)
        _RimPower ("Rim Power", Range(0.5,8)) = 3
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

            // Vertex input
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            // Vertex -> Fragment data
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            // Texture + sampler
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            // Material parameters
            float4 _BaseColor;
            float _Tile;
            float _Metallic;
            float _Smoothness;
            float4 _RimColor;
            float _RimPower;

            // Vertex shader
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));

                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = normalize(_WorldSpaceCameraPos - worldPos);

                OUT.uv = IN.uv * _Tile;

                return OUT;
            }

            // Fragment shader
            float4 frag (Varyings IN) : SV_Target
            {
                // Sample texture correctly in URP
                float4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // Main directional light
                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(IN.normalWS, mainLight.direction));
                float3 diffuse = albedo.rgb * mainLight.color * NdotL;

                // Simple specular
                float3 reflectDir = reflect(-mainLight.direction, IN.normalWS);
                float specAmount = pow(saturate(dot(reflectDir, IN.viewDirWS)), (1 - _Smoothness) * 100);
                float3 specular = specAmount * _Metallic * mainLight.color;

                // Rim light
                float rim = 1 - saturate(dot(normalize(IN.viewDirWS), normalize(IN.normalWS)));
                rim = pow(rim, _RimPower);
                float3 rimLight = _RimColor.rgb * rim;

                float3 finalColor = diffuse + specular + rimLight;

                return float4(finalColor, 1);
            }

            ENDHLSL
        }
    }
}
