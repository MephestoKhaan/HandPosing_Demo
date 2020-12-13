// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

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
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching"="True"}
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
                float desiredAngle = atan2(viewDir.z, viewDir.x);
                float currentAngle = atan2(v.normal.z, v.normal.x);
                float angle = (currentAngle-desiredAngle);

                float3x3 rotMatrix;
                float cosinus = cos(angle);
                float sinus = sin(angle);
                rotMatrix[0].xyz = float3(cosinus, 0, sinus);
                rotMatrix[1].xyz = float3(0, 1, 0);
                rotMatrix[2].xyz = float3(-sinus, 0, cosinus);
                float4 newPos = float4(mul(rotMatrix, v.vertex.xyz), 1);

                o.vertex = UnityObjectToClipPos(newPos);
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
