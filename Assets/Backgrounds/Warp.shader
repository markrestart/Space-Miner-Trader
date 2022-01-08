Shader "Unlit/Warp"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SubTex("Sub Texture", 2D) = "blue" {}
        _HighTex("Highlight Texture", 2D) = "red" {}
        _BackTex("Background Texture", 2D) = "black" {}

        _BackValue("Background float", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _SubTex;
            float4 _SubTex_ST;

            sampler2D _HighTex;
            float4 _HighTex_ST;

            sampler2D _BackTex;
            float4 _BackTex_ST;

            float _BackValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = lerp(lerp(lerp(tex2D(_MainTex, i.uv),tex2D(_SubTex, i.uv),_SinTime.z),tex2D(_HighTex, i.uv),_CosTime.y),tex2D(_BackTex, i.uv),_BackValue);

                return col;
            }
            ENDCG
        }
    }
}
