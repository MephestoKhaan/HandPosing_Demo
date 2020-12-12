Shader "Custom/Flame"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Tint ("Tint", Color) = (1,1,1,1)

        _Speed("Speed", float) = 10
        _Phase ("Phase", float) = 10
        _Amplitude ("Amplitude", float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha One
        Cull Off

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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _Tint;
            
            half _Speed;
            half _Phase;
            half _Amplitude;

            v2f vert (appdata v)
            {
                v2f o;

                float3 viewDir = ObjSpaceViewDir(v.vertex);
                viewDir.y = 0;
                viewDir = normalize(viewDir);
                float3 normal = normalize(v.normal);

                float angle =  acos(clamp(dot(viewDir, normal), -1, 1)) * 57.29578;
                float4 rotation = float4(0, sin(angle), 0, cos(angle));
                
                float m = rotation.x*rotation.x + rotation.y*rotation.y + rotation.z*rotation.z + rotation.w*rotation.w;
            v.vertex.xyz = v.vertex.xyz + 2 * cross(rotation.xyz, rotation.w * v.vertex.xyz + cross(rotation.xyz,v.vertex.xyz)) / m;


                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x += sin(_Speed*_Time.z + _Phase*i.uv.y) * i.uv.y * i.uv.y   * _Amplitude;
                fixed4 col = tex2D(_MainTex, uv) * _Tint;
                return col;
            }
            ENDCG
        }
    }
}
