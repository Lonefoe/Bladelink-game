Shader "Universal Render Pipeline/Custom/Foggy"
{
    Properties
    {
        _MainTex("Warm Texture", 2D) = "black" { }

        _01FogDepthVerticals("Linear Depth 01 And Verticals(RO)", Vector) = (0.0, 0.0, 0.0, 0.0)
        _FogTexture("Fog Ramp Texture (RO)", 2D) = "black" { }

        _Color("Tint", Color) = (1, 1, 1, 1)

        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }

        HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL

            SubShader
        {
            Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
            "RenderType" = "Transparent"
        }

            Cull Off
        Lighting Off
        Blend One OneMinusSrcAlpha
        ZWrite Off

            Pass
            {
                Tags { "LightMode" = "Universal2D" }
                HLSLPROGRAM
                #pragma prefer_hlslcc gles
                #pragma vertex CombinedShapeLightVertex
                #pragma fragment CombinedShapeLightFragment
               #pragma surface surf Lambert vertex:vert nofog keepalpha
                #pragma target 3.0
                 #pragma multi_compile _PIXELSNAP_ON

            //This is necessary to deactivate fog effects if they are not used.
    #pragma multi_compile _FOG_OFF _FOG_ON

    sampler2D _MainTex;
    fixed4 _Color;

    struct Input
    {
        float2 uv_MainTex;
        #if defined( _FOG_ON )					
            float worldPosY;
        #endif
        fixed4 color;
    };

    //Just normal stuff
    void vert(inout appdata_full v, out Input o)
    {
        #if defined( PIXELSNAP_ON )
            v.vertex = UnityPixelSnap(v_.vertex);
        #endif

        UNITY_INITIALIZE_OUTPUT(Input, o);
        o.color = v.color * _Color;

        #if defined( _FOG_ON )	
            o.worldPosY = mul(unity_ObjectToWorld, v.vertex).y;
        #endif
    }

       //         #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

#if defined( _FOG_ON )
    /****************************************************************************/
    /**********************************FOG***************************************/
    /****************************************************************************/

    fixed4 _01FogDepthVerticals;
    sampler2D _FogTexture;

    float4 FogProperties(float world_pos_y_, float tex_columns_)
    {
        float fog_bottom_column = tex_columns_;
        float fog_top_column = tex_columns_ + 0.25;
        float fog_depth = abs(_01FogDepthVerticals.x);

        //If the depth is negative, we are using front fog.
        fog_bottom_column += step(0.f, -_01FogDepthVerticals.x) * 0.5f;
        fog_top_column += step(0.f, -_01FogDepthVerticals.x) * 0.5f;

        //Calculate positions depending on depth, bottom and top.
        float2 fog_position_bottom = float2(fog_bottom_column, 1 - fog_depth);
        float4 bottom_FogTexture = tex2D(_FogTexture, fog_position_bottom);

        float2 fog_position_top = float2(fog_top_column, 1 - fog_depth);
        float4 top_FogTexture = tex2D(_FogTexture, fog_position_top);

        //Vertical position
        float vertical_pos = clamp((world_pos_y_ - _01FogDepthVerticals.y) / (_01FogDepthVerticals.z - _01FogDepthVerticals.y), 0, 1);

        //Blend bottom and top.
        return lerp(bottom_FogTexture, top_FogTexture, vertical_pos);
    }

    fixed4 CalculateFog(float world_pos_y_)
    {
        //Calculate the color, alpha is the amount of fog.
        float4 fog_tex = FogProperties(world_pos_y_, 0.125);
        float factor = fog_tex.a;
        return lerp(0, fog_tex, factor);
    }
#endif

    /****************************************************************************/
    /****************************************************************************/
    /****************************************************************************/


    void surf(Input i, inout SurfaceOutput o)
    {
        fixed4 color = tex2D(_MainTex, i.uv_MainTex) * i.color;

#if defined( _FOG_ON )					
        fixed4 fog_color = CalculateFog(i.worldPosY);

        //We have to remove the actual color of the object, the more fog it has, because we will increase emission.
        color.rgb *= (1 - fog_color.a);

        //We add the fog as emission. 
        //Since fog is not affected by any type of lights we need emission to become the color of the object.
        o.Emission = fog_color * color.a;
#endif						

        o.Albedo = color.rgb * color.a;
        o.Alpha = color.a;
    }

                ENDHLSL
            }

        }

            Fallback "Sprites/Default"
}
