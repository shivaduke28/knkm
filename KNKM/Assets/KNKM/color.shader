Shader "Unlit/color"
{
    Properties
    {
        _Speed("Speed", Range(0,10)) = 1
        _Emission("Emission", Range(0,100)) = 1
        _Color("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
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
            float _Speed;
            float _Emission;
            float4 _Color;
            float4 _Color2;

            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.b, c.g, K.w, K.z), float4(c.g, c.b, K.x, K.y), step(c.b, c.g));
                float4 q = lerp(float4(p.x, p.y, p.w, c.r), float4(c.r, p.y, p.z, p.x), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(float3(c.x, c.x, c.x) + K.xyz) * 6.0 - K.www);
                return c.z * lerp(float3(K.x, K.x, K.x), clamp(p - float3(K.x,K.x,K.x), 0.0, 1.0), c.y);
            }

            fixed3 col1(float2 uv)
            {
                fixed3 col;
                col.rg = uv;
                col.b = 0;

                float3 hsv = rgb2hsv(col.rgb);
                hsv.r *= 0.5;
                hsv.r += _Time.y * _Speed;
                return hsv2rgb(hsv);
            }

            fixed3 col2(float2 uv)
            {
                return float3(1, 0, 1) * step(0.9, frac(sin(_Time.y * 3.141592 * _Speed)));
            }



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col;
                //col.rgb = col2(i.uv);
                //col.rgb *= _Emission;

                //col.a = 1;
                //return col;
                col.rgb = 0;
                float2 uv = i.uv;
                uv.x = frac(uv.x + _Time.y * _Speed);
                col.rgb += _Color.rgb *  step(0.5, uv.x);
                col.rgb += _Color2.rgb * step(uv.x, 0.5) ;
                col.rgb *= _Emission;
                col.a = 1;
                return col;

                col.rg = i.uv;
                col.b = 0;

                float3 hsv = rgb2hsv(col.rgb);
                hsv.r *= 0.5;
                hsv.r += _Time.y * _Speed;
                col.rgb = hsv2rgb(hsv);


                col.a = 1;
                return col;
            }

            ENDCG
        }
    }
}
