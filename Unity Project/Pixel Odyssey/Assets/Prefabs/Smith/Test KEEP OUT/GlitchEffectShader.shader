Shader "Custom/GlitchEffectShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        GlitchStrength("Glitch Strength", Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            sampler2D _MainTex;
            half GlitchStrength;

            struct Input
            {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                half2 glitchOffset = half2(0.02 * sin(_Time.y * 20.0), 0.02 * cos(_Time.y * 25.0));
                half4 c = tex2D(_MainTex, IN.uv_MainTex + glitchOffset * GlitchStrength);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
