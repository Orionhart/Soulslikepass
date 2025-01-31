Shader "Unlit/ice crack test"
{
    Properties
    {
        _CrackColor("Crack Color", Color) = (0, 0, 0, 1)
        _BaseColor("Base Color", Color) = (0.8, 0.8, 1.0, 1.0)
        _CrackScale("Crack Scale", Float) = 50.0
        _CrackDepth("Crack Depth", Float) = 0.05
        _Threshold("Crack Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            float4 _BaseColor;
            float4 _CrackColor;
            float _CrackScale;
            float _CrackDepth;
            float _Threshold;

            float hash(float2 p)
            {
                p = frac(p * float2(123.34, 234.56));
                return frac(sin(dot(p, float2(12.34, 45.67))) * 43758.5453);
            }

            float worleyNoise(float2 p, float scale)
            {
                p *= scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float minDist = 1.0;

                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 neighbor = float2(x, y);
                        float2 randomOffset = hash(ip = neighbor);
                        float2 samplePoint = neighbor + randomOffset;
                        float dist = length(fp - samplePoint);
                        minDist = min(minDist, dist);
                    }
                }

                return minDist;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float2 uv = v.uv;

                float cracks = 1.0 - worleyNoise(uv, _CrackScale);
                float mask = step(_Threshold, cracks);
                v.vertex.y -= mask * _CrackDepth;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float cracks = 1.0 - worleyNoise(i.uv, _CrackScale);

                float mask = step(_Threshold, cracks);

                float3 color = lerp(_BaseColor.rgb, _CrackColor.rgb, mask);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
