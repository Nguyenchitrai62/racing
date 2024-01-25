Shader "Unlit/Ramp" {
	Properties {
		_ArrowTex ("Arrow", 2D) = "white" {}
		_DotTex ("Dot", 2D) = "white" {}
		_ColorTex ("Color", 2D) = "white" {}
		_Brightness ("Brightness", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}