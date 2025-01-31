Shader "Unlit/Ice"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.4, 0.8, 1.0, 1.0)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.9
        _FresnelColor ("Fresnel Color", Color) = (0.7, 0.9, 1.0, 1.0)
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 3.0
        _CrackScale("Crack Scale", Float) = 50.0
        _Reflectivity("Reflectivity", Range(0, 1)) = 0.5
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
            };

            float3 _BaseColor;
            float _Smoothness;
            float4 _FresnelColor;
            float _FresnelPower;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Transform normal to world space
                o.normalWS = UnityObjectToWorldNormal(v.normalOS);

                // Calculate world position using unity_ObjectToWorld matrix
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Use built-in _WorldSpaceCameraPos to get camera position
                float3 cameraPos = _WorldSpaceCameraPos;

                // Calculate view direction
                o.viewDirWS = normalize(cameraPos - worldPos);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Normalize vectors
                float3 normal = normalize(i.normalWS);
                float3 viewDir = normalize(i.viewDirWS);

                // Fresnel effect
                float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), _FresnelPower);
                float3 fresnelColor = _FresnelColor.rgb * fresnel;

                // Base color and smooth shading
                float3 baseColor = _BaseColor;

                // Combine base color and fresnel
                float3 col = baseColor + fresnelColor;

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
