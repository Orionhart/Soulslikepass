// This is a simple triplanar shader for Unity compatible with Unity 2023.2.15f1.
// It uses Shader Graph-like HLSL to project textures in world space without UV mapping.

Shader "Custom/TriplanarShader"
{
    Properties
    {
        _GroundTex ("Ground Texture", 2D) = "white" {}
        _WallTex ("Wall Texture", 2D) = "white" {}
        _Scale ("Texture Scale", Float) = 1.0
        _BlendThreshold ("Wall Blend Threshold", Float) = 0.5
        _BlendSmoothing ("Blend Smoothing", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _GroundTex;
            sampler2D _WallTex;
            float _Scale;
            float _BlendThreshold;
            float _BlendSmoothing;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Absolute world position for triplanar mapping
                float3 worldPos = i.worldPos * _Scale;

                // Normalized world normal for blending weights
                float3 worldNormal = abs(i.worldNormal);

                // Compute blending weights for each axis
                float3 blendWeights = worldNormal / (worldNormal.x + worldNormal.y + worldNormal.z);

                // Sample the ground texture in each axis projection
                float4 groundTexX = tex2D(_GroundTex, worldPos.yz);
                float4 groundTexY = tex2D(_GroundTex, worldPos.zx);
                float4 groundTexZ = tex2D(_GroundTex, worldPos.xy);

                // Sample the wall texture in each axis projection
                float4 wallTexX = tex2D(_WallTex, worldPos.yz);
                float4 wallTexY = tex2D(_WallTex, worldPos.zx);
                float4 wallTexZ = tex2D(_WallTex, worldPos.xy);

                // Combine the textures using the blend weights
                float4 groundColor = groundTexX * blendWeights.x + groundTexY * blendWeights.y + groundTexZ * blendWeights.z;
                float4 wallColor = wallTexX * blendWeights.x + wallTexY * blendWeights.y + wallTexZ * blendWeights.z;

                // Determine the blend factor based on the slope
                float slope = dot(i.worldNormal, float3(0, 1, 0));
                float wallBlend = saturate((abs(slope) - _BlendThreshold) / max(_BlendSmoothing, 0.0001));

                // Lerp between ground and wall textures
                return lerp(groundColor, wallColor, wallBlend);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
