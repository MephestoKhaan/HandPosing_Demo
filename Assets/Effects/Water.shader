Shader "Custom/Water"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Bump ("Bump", 2D) = "white" {}
        _BumpDetail ("Bump detail", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Specular ("Specular", Color) = (0,0,0,0)
        _MaxDistance ("Max Distance", float) = 200

        
        _WaveSpeed ("Wave speed", VECTOR) = (0,0,0,0)
        _WaveHeight ("Wave height", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardSpecular vertex:vert noshadow nolightmap noambient  nodynlightmap nometa noforwardadd  halfasview 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #define _SPECULARHIGHLIGHTS_OFF

        sampler2D _MainTex;
        sampler2D _Bump;
        sampler2D _BumpDetail;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpDetail;
            float distanceFactor;
        };

        half4 _Specular;
        half _Smoothness;

        half _MaxDistance;
        half4 _WaveSpeed;
        half _WaveHeight;

        void vert (inout appdata_full v, out Input o) 
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            float distance = length(v.vertex.xz);
            float distanceFactor = saturate(distance/_MaxDistance);

            v.vertex.y += sin(_Time.z + distance*0.1) * distanceFactor * _WaveHeight;

            o.distanceFactor = 1-distanceFactor;
        }


        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            half2 displacement = IN.uv_MainTex;
            displacement += _Time.x * _WaveSpeed.xy;

            half2 displacementDetail = IN.uv_BumpDetail;
            displacementDetail += _Time.x * _WaveSpeed.zw; 

            fixed4 c = tex2D (_MainTex, displacement);
            o.Albedo = c.rgb;
            
            fixed3 normal = UnpackNormal(tex2D (_Bump, displacement));
            fixed3 detailNormal = UnpackNormal(tex2D (_BumpDetail, displacementDetail));
            o.Normal = BlendNormals(normal, detailNormal);
            o.Specular = _Specular;
            o.Smoothness = _Smoothness * IN.distanceFactor;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
