// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terrain shader - snow"
{
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		_NoisePower("Noise Power", Float) = 0
		_SnowHeight("SnowHeight", Float) = 0.1
		_SnowLow("Snow Low", Float) = 0.1
		_RenderTexture("Render Texture", 2D) = "white" {}
		_TextureSnow("Texture Snow", 2D) = "white" {}
		_SnowTexture("Snow Texture", Color) = (0,0,0,0)
		_TrailColor("Trail Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv2_texcoord2;
			float2 uv_texcoord;
		};

		uniform float _SnowHeight;
		uniform float _SnowLow;
		uniform sampler2D _RenderTexture;
		uniform float4 _RenderTexture_ST;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform float _NoisePower;
		uniform sampler2D _TextureSnow;
		uniform float4 _TextureSnow_ST;
		uniform float4 _SnowTexture;
		uniform float4 _TrailColor;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_0 = (3.0).xxxx;
			return temp_cast_0;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv2_RenderTexture = v.texcoord1 * _RenderTexture_ST.xy + _RenderTexture_ST.zw;
			float4 tex2DNode26 = tex2Dlod( _RenderTexture, float4( uv2_RenderTexture, 0, 0.0) );
			float3 lerpResult27 = lerp( ( ase_vertexNormal * _SnowHeight ) , ( ase_vertexNormal * _SnowLow ) , tex2DNode26.r);
			v.vertex.xyz += lerpResult27;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_Noise = i.uv2_texcoord2 * _Noise_ST.xy + _Noise_ST.zw;
			float temp_output_18_0 = (_NoisePower + (tex2D( _Noise, uv_Noise ).r - 0.0) * (1.0 - _NoisePower) / (1.0 - 0.0));
			float2 uv_TextureSnow = i.uv_texcoord * _TextureSnow_ST.xy + _TextureSnow_ST.zw;
			float4 temp_output_38_0 = ( temp_output_18_0 * tex2D( _TextureSnow, uv_TextureSnow ) );
			float2 uv2_RenderTexture = i.uv2_texcoord2 * _RenderTexture_ST.xy + _RenderTexture_ST.zw;
			float4 tex2DNode26 = tex2D( _RenderTexture, uv2_RenderTexture );
			float4 lerpResult39 = lerp( ( temp_output_38_0 * _SnowTexture ) , ( temp_output_38_0 * _TrailColor ) , tex2DNode26.r);
			o.Albedo = lerpResult39.rgb;
			float3 temp_cast_1 = (0.0).xxx;
			o.Specular = temp_cast_1;
			o.Smoothness = 0.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
379;73;1148;553;3736.985;1081.736;4.749372;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2390.828,-451.4134;Float;False;1;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1914.264,-300.9529;Float;False;Property;_NoisePower;Noise Power;2;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2046.926,-496.1135;Float;True;Property;_Noise;Noise;1;0;Create;True;0;0;False;0;None;33d524e1d46b7fd49b630b2d9d8234fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;18;-1581.596,-491.94;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.8;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-1612.305,193.551;Float;True;Property;_TextureSnow;Texture Snow;7;0;Create;True;0;0;False;0;None;aafe815103fb7e5498f830c49c926079;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;37;-1214.67,-44.53255;Float;False;Property;_SnowTexture;Snow Texture;8;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;-1235.195,496.8201;Float;False;Property;_TrailColor;Trail Color;9;0;Create;True;0;0;False;0;0,0,0,0;0.8018868,0.7514629,0.7224546,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-1640.536,1240.83;Float;False;Property;_SnowHeight;SnowHeight;4;0;Create;True;0;0;False;0;0.1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;21;-1650.802,1068.902;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-1634.881,982.5452;Float;False;Property;_SnowLow;Snow Low;5;0;Create;True;0;0;False;0;0.1;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1209.488,147.5533;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1303.107,1144.388;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-915.5084,325.5855;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;-1870.93,732.3867;Float;True;Property;_RenderTexture;Render Texture;6;0;Create;True;0;0;False;0;None;cf07de32d80447941ba062f7c566a42c;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-920.0497,156.2353;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1304.114,1034.725;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1087.334,-619.3678;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;27;-1028.668,1166.037;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;20;-1091.675,-912.7086;Float;False;Property;_Colormultiply;Color multiply;3;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;188.108,360.5958;Float;False;Constant;_Float2;Float 2;11;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;39;-653.8676,268.5198;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1516.838,-723.5157;Float;True;Property;_Color;Color;0;0;Create;True;0;0;False;0;e388cde0d0b4f9046a5a9bdc7702e7d3;e388cde0d0b4f9046a5a9bdc7702e7d3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-827.7737,-665.7089;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;224.345,113.3448;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1807.839,-700.5158;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;224.345,189.3447;Float;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;425.1552,35.64176;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;Terrain shader - snow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;1;5;0
WireConnection;18;0;4;1
WireConnection;18;3;13;0
WireConnection;38;0;18;0
WireConnection;38;1;34;0
WireConnection;24;0;21;0
WireConnection;24;1;25;0
WireConnection;41;0;38;0
WireConnection;41;1;40;0
WireConnection;35;0;38;0
WireConnection;35;1;37;0
WireConnection;29;0;21;0
WireConnection;29;1;28;0
WireConnection;3;0;18;0
WireConnection;3;1;1;0
WireConnection;27;0;24;0
WireConnection;27;1;29;0
WireConnection;27;2;26;1
WireConnection;39;0;35;0
WireConnection;39;1;41;0
WireConnection;39;2;26;1
WireConnection;1;1;2;0
WireConnection;19;0;20;0
WireConnection;19;1;3;0
WireConnection;0;0;39;0
WireConnection;0;3;6;0
WireConnection;0;4;7;0
WireConnection;0;11;27;0
WireConnection;0;14;42;0
ASEEND*/
//CHKSM=AAC583CF7B0238F374064F4226501063B52158BD