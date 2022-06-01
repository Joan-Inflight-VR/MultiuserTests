Shader "VRToolkit/GradientAlwaysInFront"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color1("First Color", Color) = (1,1,1,1)
        _Color2("Second Color", Color) = (1,1,1,1)
        _Alpha("Alpha", range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags
         {
             "Queue" = "Transparent"
             "IgnoreProjector" = "True"
             "RenderType" = "Transparent"
             "PreviewType" = "Plane"
         }

         Lighting Off
         Cull Off
         ZTest Off
         ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf NoLighting alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        float4 _Color1;
        float4 _Color2;
        float _Alpha;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            fixed4 gradient = lerp(_Color1, _Color2, IN.uv_MainTex.x);

            o.Albedo = c.rgb * gradient;
            o.Alpha = c.a  * _Alpha;
        }

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
