Shader "Overlewd/Bling"
{
    Properties
    {   
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)


    _StencilComp("Stencil Comparison", Float) = 8
    _Stencil("Stencil ID", Float) = 0
    _StencilOp("Stencil Operation", Float) = 0
    _StencilWriteMask("Stencil Write Mask", Float) = 255
    _StencilReadMask("Stencil Read Mask", Float) = 255

    _ColorMask("Color Mask", Float) = 15

    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

    _Speed("Bling Speed", Float) = 2
    _Thickness("Bling Thickness", Range(0, 1)) = 0.8
}

SubShader
{
    Tags
    {
        "Queue" = "Transparent"
        "IgnoreProjector" = "True"
        "RenderType" = "Transparent"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "True"
    }

    Stencil
    {
        Ref[_Stencil]
        Comp[_StencilComp]
        Pass[_StencilOp]
        ReadMask[_StencilReadMask]
        WriteMask[_StencilWriteMask]
    }

    Cull Off
    Lighting Off
    ZWrite Off
    ZTest[unity_GUIZTestMode]
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask[_ColorMask]

    Pass
    {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 2.0
        #include "UnityCG.cginc"
        #include "UnityUI.cginc"

        #pragma multi_compile __ UNITY_UI_CLIP_RECT
        #pragma multi_compile __ UNITY_UI_ALPHACLIP
        #pragma multi_compile __ ANDROID

        struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color : COLOR;
            half2 texcoord  : TEXCOORD0;
            float4 worldPosition : TEXCOORD1;
        };

        fixed4 _Color;
        fixed4 _TextureSampleAdd;
        float4 _ClipRect;

        v2f vert(appdata_t IN)
        {
            v2f OUT;
            OUT.worldPosition = IN.vertex;
            OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

            OUT.texcoord = IN.texcoord;

    #ifdef UNITY_HALF_TEXEL_OFFSET
            OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
    #endif

            OUT.color = IN.color * _Color;
            return OUT;
        }

        sampler2D _MainTex;
        sampler2D _AlphaTex;
        float _Speed;
        float _Thickness;

        fixed4 frag(v2f IN) : SV_Target
        {
            half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
            //fixed4 color = UnityGetUIColor(IN.texcoord, _MainTex, _AlphaTex, _TextureSampleAdd) * IN.color;
            //half4 alpha = tex2D(_AlphaTex, IN.texcoord);
             

            float x = (IN.worldPosition.x + _ScreenParams[0] * 0.5) / _ScreenParams[0];
            float y = (IN.worldPosition.y + _ScreenParams[1] * 0.5) / _ScreenParams[1];
            float f = 1 + x - _SinTime[3] * _Speed;
            if (_CosTime[3] > 0)
            {
                if (y >= f && y <= f + _Thickness)
                    color /= 1 - (f - y) - _Thickness;
                if (y >= f - _Thickness && y <= f)
                    color /= 1 + (f - y) - _Thickness;
            }
             #if ANDROID
                color.a *= tex2D (_AlphaTex, IN.texcoord).r;
             #endif    
            #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
            #endif

            #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
            #endif
            return color;
        }
        ENDCG
    }
}
}