Shader "Stylized/Water"
{
    Properties
    {
        _DepthDensity ("Depth", Range(0.0, 1.0)) = 0.5
        _DistanceDensity ("Distance", Range(0.0, 1.0)) = 0.1
   
        _WaveNormalMap ("Normal Map", 2D) = "bump"{}
        _WaveNormalScale ("Scale", float) = 10.0
        _WaveNormalSpeed ("Speed", float) = 1.0

        _ShallowColor ("Shallow", Color) = (0.44, 0.95, 0.36, 1.0)
        _DeepColor ("Deep", Color) =  (0.0, 0.05, 0.19, 1.0)
        _FarColor ("Far", Color) = (0.04, 0.27, 0.75, 1.0)
        
        _ReflectionContribution ("Contribution", Range(0.0, 1.0)) = 1.0

        // _FoamTexture ("Texture", 2D) = "black"{}
        // _FoamScale ("Scale", float) = 1.0
        // _FoamSpeed ("Speed", float) = 1.0
        // _FoamContribution ("Contribution", Range(0.0, 1.0)) = 1.0

        _SunSpecularColor ("Color", Color) = (1, 1, 1, 1)
        _SunSpecularExponent ("Exponent", float) = 1000


        _Wave1Direction ("Direction", Range(0, 1)) = 0
        _Wave1Amplitude ("Amplitude", float) = 1
        _Wave1Wavelength ("Wavelength", float) = 1
        _Wave1Speed ("Speed", float) = 1

        _Wave2Direction ("Direction", Range(0, 1)) = 0
        _Wave2Amplitude ("Amplitude", float) = 1
        _Wave2Wavelength ("Wavelength", float) = 1
        _Wave2Speed ("Speed", float) = 1
    }
    SubShader
    {
        LOD 100

        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        GrabPass
        {
            "_GrabTexture"
        }

        Pass
        {
            Tags
            {
                "LightMode" = "Always" 
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog  // Make fog work.
            #pragma shader_feature SHADOWS

            #include "UnityCG.cginc"
            #include "WaterUtilities.cginc"
            #include "ShadowUtilities.cginc"

            // Densities.
            float _DepthDensity;
            float _DistanceDensity;

            // Wave Normal Map.
            sampler2D _WaveNormalMap;
            float _WaveNormalScale;
            float _WaveNormalSpeed;

            // Base Color.
            float3 _ShallowColor;
            float3 _DeepColor;
            float3 _FarColor;

            // Reflections.
            float _ReflectionContribution;

            // Subsurface Scattering.
            float3 _SSSColor;

            // Foam.
            // sampler2D _FoamTexture;
            // float _FoamScale;
            float _FoamNoiseScale;
            // float _FoamSpeed;
            // float _FoamContribution;

            // Sun Specular.
    

            

            // Edge Foam.

            // Shadows.            
            sampler2D _MainDirectionalShadowMap;

            // Wave 1.
            float _Wave1Direction;
            float _Wave1Amplitude;
            float _Wave1Wavelength;
            float _Wave1Speed;

            // Wave 2.
            float _Wave2Direction;
            float _Wave2Amplitude;
            float _Wave2Wavelength;
            float _Wave2Speed;

            // Depth and color buffers.
            sampler2D _GrabTexture;
            sampler2D _CameraDepthTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPosition : TEXCOORD1;
                float4 grabPosition : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };

            // Returns the total wave height offset at the given world position,
            // based on the set wave properties.
            float GetWaveHeight(float2 worldPosition)
            {
                float2 dir1 = float2(cos(PI * _Wave1Direction), sin(PI * _Wave1Direction));
                float2 dir2 = float2(cos(PI * _Wave2Direction), sin(PI * _Wave2Direction));
                float wave1 = SimpleWave(worldPosition, dir1, _Wave1Wavelength, _Wave1Amplitude, _Wave1Speed);
                float wave2 = SimpleWave(worldPosition, dir2, _Wave2Wavelength, _Wave2Amplitude, _Wave2Speed);
                return wave1 + wave2;
            }

            // Approximates the normal of the wave at the given world position. The d
            // parameter controls the "sharpness" of the normal.
            float3x3 GetWaveTBN(float2 worldPosition, float d)
            {
                float waveHeight = GetWaveHeight(worldPosition);
                float waveHeightDX = GetWaveHeight(worldPosition - float2(d, 0));
                float waveHeightDZ = GetWaveHeight(worldPosition - float2(0, d));
                
                // Calculate the partial derivatives in the Z and X directions, which
                // are the tangent and binormal vectors respectively.
                float3 tangent = normalize(float3(0, waveHeight - waveHeightDZ, d));
                float3 binormal = normalize(float3(d, waveHeight - waveHeightDX, 0));

                // Cross the results to get the normal vector, and return the TBN matrix.
                // Note that the TBN matrix is orthogonal, i.e. TBN^-1 = TBN^T.
                // We exploit this fact to speed up the inversion process.
                float3 normal = normalize(cross(binormal, tangent));
                return transpose(float3x3(tangent, binormal, normal));
            }

            v2f vert (appdata v)
            {
                v2f o;

                o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldPosition.y += GetWaveHeight(o.worldPosition.xz);
                o.vertex = mul(UNITY_MATRIX_VP, float4(o.worldPosition, 1));

                // We don't want to offset the grab position by the wave height,
                // because we get nice refraction effects for free essentially.
                // o.grabPosition = ComputeGrabScreenPos(mul(UNITY_MATRIX_VP, float4(mul(unity_ObjectToWorld, v.vertex).xyz, 1)));
                o.grabPosition = ComputeGrabScreenPos(o.vertex);

                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calculate the view vector.
                float3 viewDirWS = normalize(i.worldPosition - _WorldSpaceCameraPos);

                // ------------------- //
                // NORMAL CALCULATIONS //
                // ------------------- //

                // Get the tangent to world matrix.
                float3x3 tangentToWorld = GetWaveTBN(i.worldPosition.xz, 0.01);

                // Sample the wave normal map and calculate the world-space normal for this fragment.
                float3 normalTS = MotionFourWayChaos(_WaveNormalMap, i.worldPosition.xz/_WaveNormalScale, _WaveNormalSpeed, true);
                float3 normalWS = mul(tangentToWorld, normalTS);

                // ------------------------------ //
                // SAMPLE DEPTH AND COLOR BUFFERS //
                // ------------------------------ //

                // Calculate the position of this fragment in screen space to
                // use as uv's in screen-space texture look ups.
                float2 screenCoord = i.grabPosition.xy / i.grabPosition.w;

                // Sample the grab-pass and depth textures based on the screen coordinate
                // to get the frag color and depth of the fragment behind this one.
                float3 fragColor = tex2D(_GrabTexture, screenCoord.xy).rgb;
                float fragDepth = tex2D(_CameraDepthTexture, screenCoord.xy).x;

                // ------------------------ //
                // DEPTH AND DISTANCE MASKS //
                // ------------------------ //

                // Calculate the distance the viewing ray travels underwater,
                // as well as the transmittance for that distance.
                float opticalDepth = abs(LinearEyeDepth(fragDepth) - LinearEyeDepth(i.vertex.z));
                float transmittance = exp(-_DepthDensity * opticalDepth);

                // Also calculate how far away the fragment is from the camera.
                float distanceMask = exp(-_DistanceDensity * length(i.worldPosition - _WorldSpaceCameraPos));

                // ----------- //
                // SHADOW MASK //
                // ----------- //

                float shadowMask = 1.0;

                // If shadows are enabled, sample the shadow map.
                #ifdef SHADOWS
                #endif

                // ---------- //
                // BASE COLOR //
                // ---------- //

                // Calculate the base color based on the transmittance and distance mask.
                float3 baseColor = fragColor * _ShallowColor;
                baseColor = lerp(_DeepColor, baseColor, transmittance * max(0.5, shadowMask));
                baseColor = lerp(_FarColor, baseColor, distanceMask);

                // --------------- //
                // REFLECTED COLOR //
                // --------------- //

                // Calculate the fresnel reflection coefficient.
                float fresnelMask = 0.0 + 1.0 * pow(1.0 + dot(normalWS, -viewDirWS), 5.0);

                // Sample the reflection cubemap to get the reflections at this fragment.
                float3 reflectedColor = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflect(viewDirWS, normalWS));
                
                // Modulate the reflected color by the fresnel coefficient, distance mask, and shadow.
                reflectedColor = reflectedColor * fresnelMask * distanceMask * shadowMask;
                reflectedColor = reflectedColor * _ReflectionContribution;

                // --------------------------- //
                // SUBSURFACE SCATTERING COLOR //
                // --------------------------- //

                // Modulate the SSS color by the wave height.
                float3 sssColor = _SSSColor * GetWaveHeight(i.worldPosition.xz);

                // Only draw it on wavetips that the sun is shining through (so it appears backlit).
                sssColor *= saturate(dot(viewDirWS, _WorldSpaceLightPos0.xyz));

                // ---------- //
                // FOAM COLOR //
                // ---------- //

                // Distort the world space uv coordinates by the normal map.
                // float2 foamUV = (i.worldPosition.xz / _FoamScale) + (_FoamNoiseScale * normalTS.xz);

                // Sample the foam texture and modulate the result by the distance mask and shadow mask.
                // float3 foamColor = MotionFourWayChaos(_FoamTexture, foamUV, _FoamSpeed, false);
                // foamColor = foamColor * distanceMask * shadowMask;
                // foamColor = foamColor * _FoamContribution;

                // ------------------ //
                // SUN SPECULAR COLOR //
                // ------------------ //

                // Reflect the viewing vector by the normal.
                float3 viewR = reflect(viewDirWS, normalWS);

                // Calculate the specular mask.
                float sunSpecularMask = saturate(dot(viewR, _WorldSpaceLightPos0));
                sunSpecularMask = sunSpecularMask * shadowMask;

                // Get the sun specular color to add into the final color later on.

                // ------------- //
                // SPARKLE COLOR //
                // ------------- //

                // Get some random sparkly normals.
                
                // Dot them to make a sparkly mask.


                // Get the sparkle specular color to add later on.

                // --------------- //
                // EDGE FOAM COLOR //
                // --------------- //

                // Calculate edge foam mask, by on clipping the optical depth.

                // ----------- //
                // FINAL COLOR //
                // ----------- //

                float3 color = baseColor + reflectedColor + sssColor;

                // Apply fog.
                UNITY_APPLY_FOG(i.fogCoord, color);

                return float4(color, 1);
            }
            ENDCG
        }
    }
}
