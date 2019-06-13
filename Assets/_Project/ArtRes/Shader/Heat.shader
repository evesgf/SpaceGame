// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FORGE3D/Heat" {	
Properties {
	_TintColor("Tint", Color) = (1.0, 1.0, 1.0, 1.0)
	_Dist("Distortion ammount", Float) = 10.0		
	_Normal("Distortion map", 2D) = "" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

Category {
	
	Tags { "Queue"="Transparent+1" "RenderType"="Transparent" }
	Cull Back Lighting Off ZWrite Off 
	Fog { Mode off }
	ZTest LEqual

	SubShader {

		GrabPass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
		}

		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			 Blend SrcAlpha OneMinusSrcAlpha
			 AlphaTest Greater 0

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma multi_compile_particles
		#include "UnityCG.cginc"
				
		float4 _TintColor;
		float _Dist;

		sampler2D _Normal;

		sampler2D _CameraDepthTexture;
		float _InvFade;

		sampler2D _GrabTexture;
		float4 _GrabTexture_TexelSize;

		struct appdata_t {
			float4 vertex : POSITION;
			fixed4 color  : COLOR;
			float2 texcoord: TEXCOORD0;
		};

		struct v2f {
			float4 vertex : POSITION;
			fixed4 color  : COLOR;
			float4 uvgrab : TEXCOORD0;			
			float2 uvmain : TEXCOORD1;
			float4 projPos : TEXCOORD2;
		};

		
		float4 _Normal_ST;

		v2f vert (appdata_t v) 
		{
			v2f o;

			o.vertex = UnityObjectToClipPos(v.vertex);
			
			o.projPos = ComputeScreenPos (o.vertex);
			COMPUTE_EYEDEPTH(o.projPos.z);


			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif

			o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
			o.uvgrab.zw = o.vertex.zw;

			o.uvmain = TRANSFORM_TEX( v.texcoord, _Normal );
			
			o.color = v.color;

			return o;
		}

		float4 frag(v2f i) : COLOR
		{
			float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
			float partZ = i.projPos.z;
			float fade = saturate (_InvFade * (sceneZ-partZ));
			i.color.a *= fade;
			
			float4 packedTex = tex2D(_Normal, i.uvmain);

			float local1 = packedTex.z * 2.4;
			float2 local2 = packedTex.rg * 2.25;

			packedTex.rg = local1 * local2;

			half2 bum = UnpackNormal(packedTex).rg; 
			float2 offset = bum * _Dist * _GrabTexture_TexelSize.xy;
			i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
	
			half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));			
			
			return float4(col.rgb, _TintColor.a * packedTex.a * i.color.a);			
		}
		ENDCG
	}
}
}
}