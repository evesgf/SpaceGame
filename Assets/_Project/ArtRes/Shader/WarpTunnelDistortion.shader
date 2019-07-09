// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:0,x:34414,y:32442,varname:node_0,prsc:2|alpha-1054-OUT,refract-4426-OUT;n:type:ShaderForge.SFN_ComponentMask,id:16,x:33949,y:32491,varname:node_16,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-25-RGB;n:type:ShaderForge.SFN_Tex2d,id:25,x:33730,y:32491,ptovrint:False,ptlb:Refraction,ptin:_Refraction,varname:_Refraction,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:499d5da8a7e43fe4c9545b6277a0d554,ntxv:3,isnm:True|UVIN-6460-OUT;n:type:ShaderForge.SFN_Slider,id:1054,x:33949,y:32722,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:_Opacity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:4426,x:34188,y:32491,varname:node_4426,prsc:2|A-16-OUT,B-3667-OUT;n:type:ShaderForge.SFN_Slider,id:3667,x:33807,y:32364,ptovrint:False,ptlb:Refraction Factor,ptin:_RefractionFactor,varname:_RefractionFactor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:2;n:type:ShaderForge.SFN_TexCoord,id:221,x:33563,y:32913,varname:node_221,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:7819,x:33249,y:32795,varname:node_7819,prsc:2;n:type:ShaderForge.SFN_Slider,id:9619,x:33074,y:33024,ptovrint:False,ptlb:U_TileAnimFactor,ptin:_U_TileAnimFactor,varname:_U_TileAnimFactor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Slider,id:8508,x:33074,y:33167,ptovrint:False,ptlb:V_TileAnimFactor,ptin:_V_TileAnimFactor,varname:_V_TileAnimFactor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:4226,x:33478,y:32812,varname:node_4226,prsc:2|A-9619-OUT,B-7819-T;n:type:ShaderForge.SFN_Multiply,id:989,x:33488,y:33027,varname:node_989,prsc:2|A-8508-OUT,B-7819-T;n:type:ShaderForge.SFN_Add,id:6671,x:33733,y:32885,varname:node_6671,prsc:2|A-4226-OUT,B-221-U;n:type:ShaderForge.SFN_Add,id:7067,x:33733,y:33017,varname:node_7067,prsc:2|A-221-V,B-989-OUT;n:type:ShaderForge.SFN_Append,id:6460,x:33966,y:32939,varname:node_6460,prsc:2|A-6671-OUT,B-7067-OUT;proporder:25-1054-3667-9619-8508;pass:END;sub:END;*/

Shader "FORGE3D/WarpTunnelDistortion" {
    Properties {
        _Refraction ("Refraction", 2D) = "" {}
        _Opacity ("Opacity", Range(0, 1)) = 0
        _RefractionFactor ("Refraction Factor", Range(0, 2)) = 0
        _U_TileAnimFactor ("U_TileAnimFactor", Range(-5, 5)) = 0
        _V_TileAnimFactor ("V_TileAnimFactor", Range(-5, 5)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles xbox360 ps3 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Refraction; uniform float4 _Refraction_ST;
            uniform float _Opacity;
            uniform float _RefractionFactor;
            uniform float _U_TileAnimFactor;
            uniform float _V_TileAnimFactor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 node_7819 = _Time + _TimeEditor;
                float2 node_6460 = float2(((_U_TileAnimFactor*node_7819.g)+i.uv0.r),(i.uv0.g+(_V_TileAnimFactor*node_7819.g)));
                float3 _Refraction_var = UnpackNormal(tex2D(_Refraction,TRANSFORM_TEX(node_6460, _Refraction)));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_Refraction_var.rgb.rg*_RefractionFactor);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                return fixed4(lerp(sceneColor.rgb, finalColor,_Opacity),1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
   // CustomEditor "ShaderForgeMaterialInspector"
}
