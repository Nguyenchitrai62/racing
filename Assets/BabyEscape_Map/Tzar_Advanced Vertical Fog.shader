Shader "Tzar/Advanced Vertical Fog" {
	Properties {
		[Header(MAIN)] [Header(Texture and Color)] [Space] _MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,0.5)
		[PowerSlider(1.0)] _IntersectionThresholdMax ("Fog Intensity", Range(0, 2)) = 0.5
		[Space] _Alpha ("Alpha", Range(-1, 1)) = 1
		[Toggle] _Invert ("Invert Color", Float) = 0
		[Space] [Header(Cookie)] [Toggle] _UseCookie ("Enable Cookie", Float) = 0
		_Cookie ("Cookie", 2D) = "white" {}
		_CookieStrength ("Cookie Alpha", Range(0, 1)) = 1
		[Space] [Header(Movement)] [Toggle] _Rotation ("Rotate", Float) = 0
		_RotationSpeed ("Rotation Speed", Range(-1, 1)) = 0
		_OriginX ("Origin X", Range(-2, 2)) = 0
		_OriginY ("Origin Y", Range(-2, 2)) = 0
		[Space(30)] [Header(DISTORTION)] [Header(Distortion Texture)] _DistortTex ("Texture", 2D) = "white" {}
		[Space] [Toggle] _UseMainDistort ("Override Texture - Use Main Texture", Float) = 0
		_MainDistortAmount ("Main Texture Distortion Amount", Range(0, 1)) = 1
		[Space] [Toggle] _DistortCookie ("Override Texture - Use Cookie Texture", Float) = 0
		_DistortCookieAmount ("Cookie Distortion Amount", Range(0, 1)) = 1
		[Space] [Toggle] _MainDistort ("Distort Main Texture", Float) = 0
		[Header(Distortion Values)] _Magnitude ("Magnitude", Range(0, 10)) = 0
		[PowerSlider(2.0)] _DistortSpeed ("Speed", Range(0, 2)) = 0
		_Period ("Period", Range(-3, 3)) = 1
		_Offset ("Period Offset", Range(0, 15)) = 0
		[Space] [Header(Distortion Movement)] [Toggle] _DistortionRotation ("Rotate", Float) = 0
		_DistortRotationSpeed ("Rotation Speed", Range(-2, 2)) = 0
		[Space] [Toggle] _Translate ("Move", Float) = 0
		_SpeedX ("X Speed", Range(-0.5, 0.5)) = 0
		_SpeedY ("Y Speed", Range(-0.5, 0.5)) = 0
		[Header(Debug)] [Toggle] _TestDistortion ("Show Distortion Texture", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}