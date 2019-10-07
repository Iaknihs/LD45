// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Rainbow shader with lots of adjustable properties!

Shader "custom/Rainbow" {
	Properties{
		_Color("Color", Color) = (0.8, 0.8, 0.8, 1)
		_isActive("isActive", Float) = 0.0
		_Saturation("Saturation", Range(0.0, 1.0)) = 0.8
		_Luminosity("Luminosity", Range(0.0, 1.0)) = 0.5
		_Spread("Spread", Range(0.5, 10.0)) = 3.8
		_Speed("Speed", Range(-10.0, 10.0)) = 2.4
		_TimeOffset("TimeOffset", Range(0.0, 6.28318531)) = 0.0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			//#include "Shared/ShaderTools.cginc"

			fixed4 _Color;
			fixed _isActive;
			fixed _Saturation;
			fixed _Luminosity;
			half _Spread;
			half _Speed;
			half _TimeOffset;

			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord0 : TEXCOORD0;
			};

			struct fragmentInput {
				float4 position : SV_POSITION;
				float4 texcoord0 : TEXCOORD0;
				fixed3 localPosition : TEXCOORD1;
			};

			fixed hueToRGB(float v1, float v2, float vH) {
				if (vH < 0.0) vH += 1.0;
				if (vH > 1.0) vH -= 1.0;
				if ((6.0 * vH) < 1.0) return (v1 + (v2 - v1) * 6.0 * vH);
				if ((2.0 * vH) < 1.0) return (v2);
				if ((3.0 * vH) < 2.0) return (v1 + (v2 - v1) * ((2.0 / 3.0) - vH) * 6.0);
				return v1;
			}

			inline fixed4 HSLtoRGB(fixed4 hsl) {
				fixed4 rgb = fixed4(0.0, 0.0, 0.0, hsl.w);

				if (hsl.y == 0) {
					rgb.xyz = hsl.zzz;
				}
				else {
					float v1;
					float v2;

					if (hsl.z < 0.5) v2 = hsl.z * (1 + hsl.y);
					else v2 = (hsl.z + hsl.y) - (hsl.y * hsl.z);

					v1 = 2.0 * hsl.z - v2;

					rgb.x = hueToRGB(v1, v2, hsl.x + (1.0 / 3.0));
					rgb.y = hueToRGB(v1, v2, hsl.x);
					rgb.z = hueToRGB(v1, v2, hsl.x - (1.0 / 3.0));
				}

				return rgb;
			}

			fragmentInput vert(vertexInput i) {
				fragmentInput o;
				o.position = UnityObjectToClipPos(i.vertex);
				o.texcoord0 = i.texcoord0;
				o.localPosition = i.vertex.xyz; + fixed3(0.5, 0.5, 0.5);
				return o;
			}

			fixed4 frag(fragmentInput i) : SV_TARGET {

				if (_isActive < 0.5) { 
					return _Color;
				}

				half time = _Time.y;
				//fixed cosine = cos(timeWithOffset);
				//fixed hue = (lPos.x * sine + lPos.y * cosine) / 2.0;
				fixed hue = fmod(-i.position.y/100 + time, 1.0);
				fixed4 hsl = fixed4(hue, _Saturation, _Luminosity, 1.0);
				return HSLtoRGB(hsl);
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}