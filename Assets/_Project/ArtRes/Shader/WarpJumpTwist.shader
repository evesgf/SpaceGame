// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with '_Object2World'

Shader "FORGE3D/Warp Jump Tunnel" {
Properties {
	_TintColorA ("Tint Color A", Color) = (0.5,0.5,0.5,0.5)
	_TintColorB ("Tint Color B", Color) = (0.5,0.5,0.5,0.5)
	_Mult ("Color strength", float) = 1
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	_Twist ("Twist", Float) = 0.0
	_EdgeFadePow ("Edge Fade Power", Float) = 0.0
	_EdgeFadeMult ("Edge Fade Mult", Float) = 0.0
	_Alpha ("Alpha cutoff", Range(0,1)) = 1
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend One One
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Mode Off }
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _TintColorA, _TintColorB;
			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			float _Twist, _Mult;
			float _Alpha;
			float _EdgeFadePow, _EdgeFadeMult;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD1;
				#endif
				float3 normalDir : TEXCOORD2;
				float4 posWorld : TEXCOORD3;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize(mul(unity_ObjectToWorld, float4(v.normal.xyz,0)).xyz);
				return o;
			}

		
			
			float4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				


			   i.texcoord.xy -=0.5;
           	
           		float dist = abs(length(i.texcoord));

	            float s = sin ( _Twist * dist);
	            float c = cos ( _Twist * dist);
	           
	            float2x2 rotationMatrix = float2x2( c, -s, s, c);
	            rotationMatrix *=0.5;
	            rotationMatrix +=0.5;
	            rotationMatrix = rotationMatrix * 2-1;
	            i.texcoord.xy = mul ( i.texcoord.xy, rotationMatrix );
	            i.texcoord.xy += 0.5;

	            float4 final = tex2D(_MainTex, i.texcoord);
				final = lerp(_TintColorA * _TintColorA.a * _Mult, _TintColorB * _TintColorB.a * _Mult, final * _Mult) * final;

				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float f = pow(abs(dot(viewDir, i.normalDir)), _EdgeFadePow) * _EdgeFadeMult;

				

				return float4(f* final.xyz * i.color.a * _Alpha, 1);
			}
			ENDCG 
		}
	}	
}
}
