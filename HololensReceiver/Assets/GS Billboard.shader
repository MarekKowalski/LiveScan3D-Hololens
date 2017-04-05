//This is a modified version of a shader published by smb02dunnal on the Unity forums:
//https://forum.unity3d.com/threads/billboard-geometry-shader.169415/

Shader "Custom/GS Billboard"
{
	Properties
	{
		_PointSize("PointSize", Range(0, 0.1)) = 0.01
	}

	SubShader
	{
	Pass
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma target 5.0
#pragma vertex VS_Main
#pragma fragment FS_Main
#pragma geometry GS_Main
#include "UnityCG.cginc" 

		// **************************************************************
		// Data structures												*
		// **************************************************************
		struct GS_INPUT
		{
			float4	pos		: POSITION;
			float4	col		: COLOR;
		};

		struct FS_INPUT
		{
			float4	pos		: POSITION;
			float4  col		: COLOR;
		};


		// **************************************************************
		// Vars															*
		// **************************************************************

		float _PointSize;

		// **************************************************************
		// Shader Programs												*
		// **************************************************************

		// Vertex Shader ------------------------------------------------
		GS_INPUT VS_Main(appdata_full v)
		{
			GS_INPUT output = (GS_INPUT)0;

			output.pos = mul(unity_ObjectToWorld, v.vertex);
			output.col = v.color;

			return output;
		}

		// Geometry Shader -----------------------------------------------------
		[maxvertexcount(4)]
		void GS_Main(point GS_INPUT p[1], inout TriangleStream<FS_INPUT> triStream)
		{
			float3 up = float3(0, 1, 0);
			float3 look = _WorldSpaceCameraPos - p[0].pos;
			look.y = 0;
			look = normalize(look);
			float3 right = cross(up, look);

			float halfS = 0.5f * _PointSize;

			float4 v[4];
			v[0] = float4(p[0].pos + halfS * right - halfS * up, 1.0f);
			v[1] = float4(p[0].pos + halfS * right + halfS * up, 1.0f);
			v[2] = float4(p[0].pos - halfS * right - halfS * up, 1.0f);
			v[3] = float4(p[0].pos - halfS * right + halfS * up, 1.0f);

			FS_INPUT pIn;
			pIn.pos = UnityObjectToClipPos(v[0]);
			pIn.col = p[0].col;
			triStream.Append(pIn);

			pIn.pos = UnityObjectToClipPos(v[1]);
			pIn.col = p[0].col;
			triStream.Append(pIn);

			pIn.pos = UnityObjectToClipPos(v[2]);
			pIn.col = p[0].col;
			triStream.Append(pIn);

			pIn.pos = UnityObjectToClipPos(v[3]);
			pIn.col = p[0].col;
			triStream.Append(pIn);
		}

		// Fragment Shader -----------------------------------------------
		float4 FS_Main(FS_INPUT input) : COLOR
		{
			return input.col;
		}

		ENDCG
	}
	}
}
