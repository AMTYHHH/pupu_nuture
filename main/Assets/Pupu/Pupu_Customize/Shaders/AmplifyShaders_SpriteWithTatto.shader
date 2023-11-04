// Made with Amplify Shader Editor v1.9.1.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaders/SpriteWithTatto"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_SpriteRect("SpriteRect", Vector) = (0,0,0,0)
		_TattoTex("TattoTex", 2D) = "white" {}
		[Toggle]_TattoMapMode("TattoMapMode", Float) = 0
		[Toggle]_FlipX("FlipX", Float) = 0
		_TattoColor("TattoColor", Color) = (1,1,1,1)

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _TattoTex;
			float4 _MainTex_TexelSize;
			uniform float4 _SpriteRect;
			uniform float _FlipX;
			uniform float4 _TattoColor;
			uniform sampler2D TATTO_MAP_TEX;
			uniform float _TattoMapMode;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord1 = screenPos;
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord7 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 originalColor78 = ( IN.color * tex2D( _MainTex, texCoord7 ) );
				float2 texCoord30 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult32 = (float2(_MainTex_TexelSize.z , _MainTex_TexelSize.w));
				float2 appendResult35 = (float2(_SpriteRect.x , _SpriteRect.y));
				float2 appendResult37 = (float2(_SpriteRect.z , _SpriteRect.w));
				float2 break96 = ( ( ( texCoord30 * appendResult32 ) - appendResult35 ) / appendResult37 );
				float lerpResult99 = lerp( break96.x , ( 1.0 - break96.x ) , _FlipX);
				float2 appendResult98 = (float2(lerpResult99 , break96.y));
				float2 rectUV39 = appendResult98;
				float4 temp_output_101_0 = ( tex2D( _TattoTex, rectUV39 ) * _TattoColor );
				float4 lerpResult106 = lerp( float4( 1,1,1,1 ) , temp_output_101_0 , (temp_output_101_0).a);
				float4 tatto86 = lerpResult106;
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult16 = (float2(ase_screenPosNorm.x , ase_screenPosNorm.y));
				float4 tattoMap81 = tex2D( TATTO_MAP_TEX, appendResult16 );
				float4 lerpResult74 = lerp( tatto86 , tattoMap81 , _TattoMapMode);
				float4 break23 = ( originalColor78 * lerpResult74 );
				float4 appendResult22 = (float4(break23.r , break23.g , break23.b , (originalColor78).a));
				
				fixed4 c = appendResult22;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19109
Node;AmplifyShaderEditor.CommentaryNode;84;-3278.005,-390.3956;Inherit;False;1327.012;648.0723;Comment;8;86;101;100;85;46;48;106;107;TattoTex;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;80;-3295.802,379.4671;Inherit;False;1140.902;442.8296;Comment;5;73;16;41;72;81;TattoMap;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexelSizeNode;29;-1058.045,-761.3286;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;32;-876.2008,-689.1679;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;33;-522.4053,-800.3852;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-545.4053,-709.3852;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;36;-362.4051,-704.3852;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;37;-541.4053,-607.3854;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-693.3953,-887.4118;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;34;-710.4053,-716.3852;Inherit;False;Property;_SpriteRect;SpriteRect;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;22;-172.5474,-154.4607;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;616.8707,-169.5723;Float;False;True;-1;2;ASEMaterialInspector;0;10;AmplifyShaders/SpriteWithTatto;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.ComponentMaskNode;58;-459.6812,-486.0366;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1237.451,-397.2607;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-931.7022,-480.1718;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;6;-1114.013,-566.9864;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-796.5784,-486.1611;Inherit;False;originalColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;23;-391.0996,-152.3612;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-533.082,-152.7558;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;73;-2740.288,486.5246;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2976.887,638.9658;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;48;-2983.238,-236.7712;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;41;-3245.802,610.2967;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1503.76,-369.3583;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;85;-3232.172,-127.058;Inherit;False;39;rectUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-965.5801,-888.1678;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;96;-225.1959,-702.0309;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;98;267.8041,-697.0309;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;440.4806,-702.9726;Inherit;False;rectUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;99;94.80408,-804.0309;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-49.24017,-710.6432;Inherit;False;Property;_FlipX;FlipX;4;1;[Toggle];Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;97;-62.19592,-778.0309;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;4;-1486.41,-525.9796;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-2434.359,486.6865;Inherit;False;tattoMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-973.5493,111.5519;Inherit;False;Property;_TattoMapMode;TattoMapMode;3;1;[Toggle];Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;74;-764.2578,-10.01585;Inherit;False;3;0;COLOR;1,1,1,1;False;1;COLOR;1,1,1,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-975.2692,20.47952;Inherit;False;81;tattoMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-1014.373,-67.5372;Inherit;False;86;tatto;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;46;-3228.005,-340.3956;Inherit;True;Property;_TattoTex;TattoTex;1;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;72;-3066.511,429.4671;Inherit;True;Global;TATTO_MAP_TEX;TATTO_MAP_TEX;2;0;Create;True;0;0;0;False;0;False;None;;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;86;-2227.291,-108.0562;Inherit;False;tatto;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;106;-2457.301,-182.9772;Inherit;False;3;0;COLOR;1,1,1,1;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-2671.226,-81.407;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;100;-2903.226,11.59305;Inherit;False;Property;_TattoColor;TattoColor;5;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;107;-2517.301,23.02283;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
WireConnection;29;0;4;0
WireConnection;32;0;29;3
WireConnection;32;1;29;4
WireConnection;33;0;31;0
WireConnection;33;1;35;0
WireConnection;35;0;34;1
WireConnection;35;1;34;2
WireConnection;36;0;33;0
WireConnection;36;1;37;0
WireConnection;37;0;34;3
WireConnection;37;1;34;4
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;22;0;23;0
WireConnection;22;1;23;1
WireConnection;22;2;23;2
WireConnection;22;3;58;0
WireConnection;1;0;22;0
WireConnection;58;0;78;0
WireConnection;2;0;4;0
WireConnection;2;1;7;0
WireConnection;57;0;6;0
WireConnection;57;1;2;0
WireConnection;78;0;57;0
WireConnection;23;0;79;0
WireConnection;79;0;78;0
WireConnection;79;1;74;0
WireConnection;73;0;72;0
WireConnection;73;1;16;0
WireConnection;16;0;41;1
WireConnection;16;1;41;2
WireConnection;48;0;46;0
WireConnection;48;1;85;0
WireConnection;96;0;36;0
WireConnection;98;0;99;0
WireConnection;98;1;96;1
WireConnection;39;0;98;0
WireConnection;99;0;96;0
WireConnection;99;1;97;0
WireConnection;99;2;95;0
WireConnection;97;0;96;0
WireConnection;81;0;73;0
WireConnection;74;0;87;0
WireConnection;74;1;83;0
WireConnection;74;2;76;0
WireConnection;86;0;106;0
WireConnection;106;1;101;0
WireConnection;106;2;107;0
WireConnection;101;0;48;0
WireConnection;101;1;100;0
WireConnection;107;0;101;0
ASEEND*/
//CHKSM=97F55365C58B9F183DA773FBC653880EE1D7DEFF