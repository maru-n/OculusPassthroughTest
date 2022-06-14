Shader "Unlit/PassthroughDoorWall"
{
    Properties
    {
        // _Alpha ("Passthrough Alpha", Range (0, 1)) = 1.0
        _DoorWidth ("Door width", Range (0, 1)) = 0.2
        _DoorHeight ("Door height", Range (0, 1)) = 0.4
        // _MainTex ("Texture", 2D) = "white" {}
        // [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) =  0
        // [Header(DepthTest)]
        // [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
        // [Enum(UnityEngine.Rendering.BlendOp)] _BlendOpColor("Blend Color", Float) = 2 //"ReverseSubtract"
        // [Enum(UnityEngine.Rendering.BlendOp)] _BlendOpAlpha("Blend Alpha", Float) = 3 //"Min"        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off
		    ZWrite Off
			// ZTest[_ZTest]
			// BlendOp[_BlendOpColor], [_BlendOpAlpha]
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

            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            // float _Alpha;
            float _DoorWidth;
            float _DoorHeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float rect(float a, float b, float x) {
                return step(a, x) * (1.0 - step(b, x));
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                // return col;
                // float alpha = 1;
                // return float4(0, 0, 0, alpha);
                // return fixed4(0,0,0,step(0.5, i.uv.x));
                float doorPosX = (1.0 - _DoorWidth) / 2.0;
                float doorPosY = (1.0 - _DoorHeight) / 2.0;
                float isDoor = rect(doorPosX, 1-doorPosX, i.uv.x) * rect(doorPosY, 1.0-doorPosY, i.uv.y);
                float alpha = (facing > 0 ) ? 1.0 - isDoor : isDoor;
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
