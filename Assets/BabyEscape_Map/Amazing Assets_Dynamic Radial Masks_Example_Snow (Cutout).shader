Shader "Amazing Assets/Dynamic Radial Masks/Example/Snow (Cutout)" {
	Properties {
		_Color ("Base Color", Vector) = (1,1,1,1)
		_BaseTex ("Base Map", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		_BaseGlossiness ("Base Smoothness", Range(0, 1)) = 0.5
		_BaseMetallic ("Base Metallic", Range(0, 1)) = 0
		[NoScaleOffset] _BaseNormal ("Base Normal", 2D) = "bump" {}
		_SnowCover ("Snow Cover", Range(-1, 1)) = 0.5
		_SnowColor ("Snow Color", Vector) = (1,1,1,1)
		_SnowTex ("Snow Map", 2D) = "white" {}
		_SnowGlossiness ("Snow Smoothness", Range(0, 1)) = 0.5
		_SnowMetallic ("Snow Metallic", Range(0, 1)) = 0
		[NoScaleOffset] _SnowNormal ("Snow Normal", 2D) = "bump" {}
		[Toggle(_SONAR_EFFECT_ON)] _RenderSonarEffect ("Render Sonar Effect", Float) = 0
		[HDR] _SonarColor ("Sonar Color", Vector) = (0,0,0,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Transparent/Cutout/Diffuse"
}