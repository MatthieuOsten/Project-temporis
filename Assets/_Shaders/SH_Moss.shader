Shader "Custom/SH_Moss" {
    Properties{
        _MainTex("Main texture", 2D) = "white" {}
        _MossTex("Moss texture", 2D) = "gray" {}
        _Direction("Direction", Vector) = (0, 1, 0)
        _Amount("Amount", Range(0, 1)) = 1
    }
        SubShader{
            Tags { "RenderType" = "Opaque" "NoD3D11TextureFormatCheck" = "true" "NoFallback" = "true" }

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct v2f {
                    float4 pos : SV_POSITION;
                    float3 normal : NORMAL;
                    float2 uv_Main : TEXCOORD0;
                    float2 uv_Moss : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _MossTex;
                float4 _MossTex_ST;

                v2f vert(appdata_full v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv_Main = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.uv_Moss = TRANSFORM_TEX(v.texcoord, _MossTex);
                    o.normal = mul(unity_ObjectToWorld, v.normal);
                    return o;
                }

                float3 _Direction;
                fixed _Amount;



                fixed4 frag(v2f i) : COLOR {
                    fixed val = dot(normalize(i.normal), _Direction);

                    if (val < 1 - _Amount)
                        val = 0;

                    fixed4 tex1 = tex2Dlod(_MainTex, float4(i.uv_Main, 0, 0));
                    fixed4 tex2 = tex2Dlod(_MossTex, float4(i.uv_Moss, 0, 0));
                    return tex1 * (1 - smoothstep(1 - _Amount, 1, val)) + tex2 * smoothstep(1 - _Amount, 1, val);

                }

                ENDCG
            }
        }
            FallBack "Diffuse"
}
