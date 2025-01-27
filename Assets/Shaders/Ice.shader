Shader "Unlit/Ice"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.4, 0.8, 1.0, 1.0)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.9
        _FresnelColor ("Fresnel Color", Color) = (0.7, 0.9, 1.0, 1.0)
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 3.0
        _CrackDepth("Crack Depth", Range(0, 1)) = 0.7
        _CrackScale("Crack Scale", Float) = 50.0
        _CrackIntensity("Crack Intensity", Float) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
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
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normalWS : NORMAL;
                float3 viewDirWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            float3 _BaseColor;
            float _Smoothness;
            float4 _FresnelColor;
            float _FresnelPower;
            float _CrackDepth;
            float _CrackScale = 1.0;
            float _CrackIntensity = 0.5;

            float hash(float2 p) 
            {
                p = frac(p * float2(123.56, 456.78));
                return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
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
                        float2 randomOffset = hash(ip + neighbor); 
                        float dist = length(fp - (neighbor + randomOffset));
                        minDist = min(minDist, dist); 
                    }
                }

                return minDist;
            }

            float raymarchCracks(float2 uv, float3 viewDir, float depth)
            {
                float totalDepth = 0.0;
                const int steps = 16;
                float stepSize = depth / steps;

                for (int i = 0; i < steps; i++)
                {
                    float2 offsetUV = uv + viewDir.xy * totalDepth;
                    float noise = worleyNoise(offsetUV, _CrackScale);
                    if (noise < 0.1) 
                    {
                        return 1.0;
                    }
                    totalDepth += stepSize;
                }

                return 0.0; 
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                //normal to world space
                o.normalWS = UnityObjectToWorldNormal(v.normalOS);

                //calculate world position using unity_ObjectToWorld 
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float3 cameraPos = _WorldSpaceCameraPos;
                o.viewDirWS = normalize(cameraPos - worldPos);
                o.uv = v.vertex.xy * 10;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //normalize vectors
                float3 normal = normalize(i.normalWS);
                float3 viewDir = normalize(i.viewDirWS);

                //fresnel effect
                float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), _FresnelPower);
                float3 fresnelColor = _FresnelColor.rgb * fresnel;

                //base color and smooth shading
                float3 baseColor = _BaseColor;

                float cracks = raymarchCracks(i.uv, viewDir, _CrackDepth);
                float3 crackColor = cracks * _CrackIntensity;

                //combine base color and fresnel
                float3 col = baseColor * (1.0 - cracks) + crackColor + fresnelColor;

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
