Shader "Unlit/Passthrough"
{
    Properties
    {
        _Alpha ("Passthrough Alpha", Range (0, 1)) = 1.0
        // _MainTex ("Texture", 2D) = "white" {}
        
        // [Header(DepthTest)]
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) =  0
        // [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOpColor("Blend Color", Float) = 2 //"ReverseSubtract"
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOpAlpha("Blend Alpha", Float) = 3 //"Min"        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull [_Cull]
            Cull Off
		    ZWrite Off
			// ZTest[_ZTest]
			BlendOp[_BlendOpColor], [_BlendOpAlpha]
            BlendOp RevSub
            Blend Zero One, One One

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

            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                return fixed4(0, 0, 0, _Alpha);
            }
            ENDCG
        }
    }
}
