// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable

Shader "Custom/StandardSkinnedLightmap"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Bump("Bump", 2D) = "bump" {}

        _Metallic("Metallic", Range(0,1)) = 0
        _Smoothness("Smoothness", Range(0,1)) = 0

        _LightmapCoordinates("Lightmap Coordinates", VECTOR) = (0,0,0,0)


        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard  nolightmap
        #pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
        #pragma shader_feature_local _GLOSSYREFLECTIONS_OFF
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Bump;
        float4 _LightmapCoordinates;
        half _Smoothness;
        half _Metallic;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.uv_MainTex.xy * _LightmapCoordinates.xy + _LightmapCoordinates.zw));

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * pow(lightmap,0.5);
            o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_MainTex));
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
