Shader "Custom/ProjectorShader" {
 Properties {
  [HDR]_Color ("Color", Color) = (1,1,1,1)
  _MainTex ("Albedo (RGB)", 2D) = "white" {}
 }

 SubShader {
  Tags { "Queue" = "Transparent" }
  LOD 200

  Blend SrcAlpha OneMinusSrcAlpha // Cập nhật phần Blend để hỗ trợ độ trong suốt

  CGPROGRAM
  #pragma surface surf Lambert vertex:vert noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa noforwardadd nolppv noshadowmask
  #pragma target 3.0

  float4x4 unity_Projector;
  sampler2D _MainTex;
  fixed4 _Color;

  struct Input {
   float2 uv_MainTex;
   float4 posProj : TEXCOORD0;
  };

  void vert(inout appdata_full v, out Input o) {
   UNITY_INITIALIZE_OUTPUT(Input, o);
   o.posProj = mul(unity_Projector, v.vertex);
  }

  void surf (Input IN, inout SurfaceOutput o) {
   // Kiểm tra xem pixel có nằm trong vùng chiếu sáng của projector không
   float2 projUV = IN.posProj.xy / IN.posProj.w;
   bool isOutside = projUV.x < 0.0 || projUV.x > 1.0 || projUV.y < 0.0 || projUV.y > 1.0;

   fixed4 projColor = isOutside ? _Color : tex2Dproj(_MainTex, UNITY_PROJ_COORD(IN.posProj)) * _Color;

   // Nếu projector không chiếu trúng, giữ nguyên màu sắc
   o.Albedo = projColor.rgb;
   o.Alpha = isOutside ? 1.0 : projColor.a; // Giữ nguyên độ trong suốt nếu nằm ngoài vùng chiếu
  }
  ENDCG
 }
 FallBack "Diffuse"
}