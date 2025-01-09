Shader "Custom/CubeFaceShader"
{
    Properties
    {
        _FrontTex ("Front Texture", 2D) = "white" {}
        _BackTex ("Back Texture", 2D) = "white" {}
        _TopTex ("Top Texture", 2D) = "white" {}
        _BottomTex ("Bottom Texture", 2D) = "white" {}
        _LeftTex ("Left Texture", 2D) = "white" {}
        _RightTex ("Right Texture", 2D) = "white" {}
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
            #include "UnityCG.cginc"

            sampler2D _FrontTex;
            sampler2D _BackTex;
            sampler2D _TopTex;
            sampler2D _BottomTex;
            sampler2D _LeftTex;
            sampler2D _RightTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 각 면의 Normal 벡터를 dot 함수로 구분
                if (dot(i.normal, float3(0, 0, 1)) > 0.9) // 앞면
                    return tex2D(_FrontTex, i.uv);
                else if (dot(i.normal, float3(0, 0, -1)) > 0.9) // 뒷면
                    return tex2D(_BackTex, i.uv);
                else if (dot(i.normal, float3(0, 1, 0)) > 0.9) // 윗면
                    return tex2D(_TopTex, i.uv);
                else if (dot(i.normal, float3(0, -1, 0)) > 0.9) // 아랫면
                    return tex2D(_BottomTex, i.uv);
                else if (dot(i.normal, float3(-1, 0, 0)) > 0.9) // 왼쪽면
                    return tex2D(_LeftTex, i.uv);
                else if (dot(i.normal, float3(1, 0, 0)) > 0.9) // 오른쪽면
                    return tex2D(_RightTex, i.uv);

                return fixed4(1, 1, 1, 1); // 기본값 흰색
            }
            ENDCG
        }
    }
}
