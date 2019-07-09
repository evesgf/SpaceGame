// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9152,x:33016,y:32739,varname:node_9152,prsc:2|emission-1309-OUT,alpha-7557-OUT;n:type:ShaderForge.SFN_Tex2d,id:5491,x:32492,y:32833,ptovrint:False,ptlb:color,ptin:_color,varname:_color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cdd32215bded8cf4b85adbb5a1d434c5,ntxv:0,isnm:False|UVIN-1595-OUT;n:type:ShaderForge.SFN_TexCoord,id:1024,x:31493,y:32819,varname:node_1024,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:1003,x:31555,y:32986,varname:node_1003,prsc:2;n:type:ShaderForge.SFN_Add,id:2295,x:31924,y:32813,varname:node_2295,prsc:2|A-1024-U,B-690-OUT;n:type:ShaderForge.SFN_Multiply,id:690,x:31764,y:33008,varname:node_690,prsc:2|A-1003-T,B-5692-OUT;n:type:ShaderForge.SFN_Slider,id:5692,x:31388,y:33216,ptovrint:False,ptlb:U_TileAnimFactor,ptin:_U_TileAnimFactor,varname:_U_TileAnimFactor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Append,id:1595,x:32139,y:32843,varname:node_1595,prsc:2|A-2295-OUT,B-2323-OUT;n:type:ShaderForge.SFN_Slider,id:7664,x:31388,y:33361,ptovrint:False,ptlb:V_TileAnimFactor,ptin:_V_TileAnimFactor,varname:_V_TileAnimFactor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:4700,x:31878,y:33269,varname:node_4700,prsc:2|A-1003-T,B-7664-OUT;n:type:ShaderForge.SFN_Add,id:2323,x:31978,y:32921,varname:node_2323,prsc:2|A-1024-V,B-4700-OUT;n:type:ShaderForge.SFN_Slider,id:7557,x:32059,y:33157,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:_Opacity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:6523,x:32516,y:32615,ptovrint:False,ptlb:node_6523,ptin:_node_6523,varname:_node_6523,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1309,x:32735,y:32790,varname:node_1309,prsc:2|A-6523-RGB,B-5491-RGB;proporder:5491-5692-7664-7557-6523;pass:END;sub:END;*/

Shader "FORGE3D/WarpTunnel" {
    Properties {
        _color ("color", 2D) = "white" {}
        _U_TileAnimFactor ("U_TileAnimFactor", Range(-5, 5)) = 0
        _V_TileAnimFactor ("V_TileAnimFactor", Range(-5, 5)) = 0
        _Opacity ("Opacity", Range(0, 1)) = 0
        _node_6523 ("node_6523", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _color; uniform float4 _color_ST;
            uniform float _U_TileAnimFactor;
            uniform float _V_TileAnimFactor;
            uniform float _Opacity;
            uniform float4 _node_6523;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_1003 = _Time + _TimeEditor;
                float2 node_1595 = float2((i.uv0.r+(node_1003.g*_U_TileAnimFactor)),(i.uv0.g+(node_1003.g*_V_TileAnimFactor)));
                float4 _color_var = tex2D(_color,TRANSFORM_TEX(node_1595, _color));
                float3 emissive = (_node_6523.rgb*_color_var.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,_Opacity);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
