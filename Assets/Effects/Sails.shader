Shader "Custom/Sails"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Specular ("Specular", Color) = (0,0,0,0)

        _WindSpeed("Wind Speed", float) = 1
        _WindStrenght("Wind Strenght", float) = 1
        _WindTurbulence("Wind Turbulence", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardSpecular vertex:vert fullforwardshadows nofog  

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half4 _Specular;
        fixed4 _Color;
        half _WindSpeed;
        half _WindStrenght;
        half _WindTurbulence;

          void vert (inout appdata_full v) 
        {
            float turbulence = length(v.vertex.xyz) * _WindTurbulence;
            v.vertex.z += sin(_Time.z *_WindSpeed + turbulence) * _WindStrenght * v.color.r;
        }

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            o.Specular = _Specular;
            o.Smoothness = 0;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
