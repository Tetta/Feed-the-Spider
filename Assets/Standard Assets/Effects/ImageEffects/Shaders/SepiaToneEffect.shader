Shader "Hidden/Sepiatone Effect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;

fixed4 frag (v2f_img i) : SV_Target
{	
	fixed4 original = tex2D(_MainTex, i.uv);
	
	// get intensity value (Y part of YIQ color space)
	//fixed Y = dot (fixed3(0.299, 0.587, 0.114), original.rgb);
	//fixed Y = dot (fixed3(0, 0, 0), original.rgb);

	// Convert to Sepia Tone by adding constant
	//fixed4 sepiaConvert = float4 (0.191, -0.054, -0.221, 0.0);
	fixed4 sepiaConvert = float4 (0, 0, 0, 0.0);
	//fixed4 output = sepiaConvert + Y;
	fixed4 output = sepiaConvert;
	//output.g = original.g * 0.4 + original.r * 1.0 ;
	//output.b = original.b * 0.6;
	//output.r = original.g * 0.6 + original.b * 0.4;
	output.g = original.b / 1.1;
	output.b = original.r / 1.1;
	output.r = original.g / 1.1;
	output.a = original.a;
	
	return output;
}
ENDCG

	}
}

Fallback off

}
