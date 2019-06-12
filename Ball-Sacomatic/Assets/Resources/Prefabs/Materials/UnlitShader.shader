Shader "Unlit/UnlitShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex MyVert
			#pragma fragment MyFrag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4x4 MyXformMat;  // own transform matrix
			fixed4   MyColor;

			v2f MyVert(appdata v)
			{
				v2f o;
				o.vertex = mul(MyXformMat, v.vertex);  // use our own transform matrix!
													   // MUST apply before camrea!

				o.vertex = mul(UNITY_MATRIX_VP, o.vertex);   // camera transform only 
				o.uv = v.uv;
				return o;
			}

			fixed4 MyFrag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col += MyColor;
				return col;
			}
			ENDCG
		}
	}
}