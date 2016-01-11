Shader "Custom/FancyGrid" {
	Properties{
		_Color("Color", Color) = (0.2, 0.2, 0.2 ,1)
		_GridColor("Grid Color", Color) = (0.4,0.4,0.4,1)
		_GridColorLit("Grid Color (Lit)", Color) = (0.55, 0.55, 0.55, 1)
		_LightSpeed("Light Speed", Float) = 1
		_LightDistance("Light Distance", Float) = 1
		_LightSpread("Light Spread", Float) = 0.2
		_GridLineThickness ("Grid Line Thickness", Float) = 0.05
		_GridSize ("Grid Size", Float) = 0.5
		_CoordinateSpace ("Coordinate Space (1=XY, 2=YZ, 3=XZ)", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Unlit noforwardadd

		fixed4 _Color;
		fixed4 _GridColor;
		fixed4 _GridColorLit;
		float _GridLineThickness;
		float _GridSize;
		float _LightSpeed;
		float _LightDistance;
		float _LightSpread;
		float _CoordinateSpace;
		
		struct Input {
			float3 worldPos;
		};

		float fade(float t) {
			return t * t * t * (t * (t * 6 - 15) + 10);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// We fmod twice, because we want to add _GridSize if the coordinate is negative
			float3 coord = fmod(fmod(IN.worldPos, _GridSize) + _GridSize, _GridSize);

			float2 gridPos = coord.xz;
			if (_CoordinateSpace <= 1)
				gridPos = coord.xy;
			else if (_CoordinateSpace <= 2)
				gridPos = coord.yz;

			float2 p = ceil( saturate(_GridLineThickness - gridPos) );
			float gridalpha = saturate(p.x+p.y);

			float csp = fmod(fmod(IN.worldPos.z + _Time.y * _LightSpeed, _LightDistance) + _LightDistance, _LightDistance);
			float zalpha = saturate((_LightSpread - csp) / _LightSpread) + saturate((_LightSpread - (_LightDistance - csp)) / _LightSpread);

			o.Albedo = lerp(_Color, lerp(_GridColor, _GridColorLit, fade(zalpha)), gridalpha);
		}

		half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten) {
			return half4(s.Albedo,1);
		}

		half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			return half4(s.Albedo, 1);
		}

		half4 LightingUnlit_PrePass(SurfaceOutput s, half4 light) {
			return half4(s.Albedo, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
