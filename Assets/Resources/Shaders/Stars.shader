Shader "Unlit/Stars"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "Queue"="Transparent"
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            static const float pi = 3.1415926535897;

            float rand2(float2 uv) {
                return frac(sin(dot(uv, float2(13.337, 61.998))) * 48675.75647);
            }

            float2 rotate(float2 uv, float a) {
                return float2(uv.y * cos(a) + uv.x * sin(a), uv.x * cos(a) - uv.y * sin(a));
            }

            float2 rand2x2(float2 uv) {
                return float2(rand2(uv), rand2(-uv));
            }

            float3 rand2x3(float2 uv) {
                return float3(rand2(uv), rand2(-uv), rand2(float2(-uv.x - 5., uv.y + 1.)));
            }

            float perl(float2 uv, float t) {
                float2 id = floor(uv);
                float2 loc = frac(uv);
                float2 sloc = smoothstep(0., 1., loc);
                return lerp(
                    lerp(
                        dot(loc, rotate(float2(1., 1.), rand2(id) * (pi * 2. + t))),
                        dot(loc - float2(1., 0.), rotate(float2(1., 1.), rand2(id + float2(1., 0.)) * (pi * 2. + t))),
                        sloc.x
                    ),
                    lerp(
                        dot(loc - float2(0., 1.), rotate(float2(1., 1.), rand2(id + float2(0., 1.)) * (pi * 2. + t))),
                        dot(loc - float2(1., 1.), rotate(float2(1., 1.), rand2(id + float2(1., 1.)) * (pi * 2. + t))),
                        sloc.x
                    ),
                    sloc.y
                );
            }

            float fperl(float2 uv, float t, float iter) {
                float o = 0., k = 0., p = 1.;
                for (float i = 0.; i < iter; i++) {
                    o += perl(uv * p, t * p) / p;
                    k += 1. / p;
                    p *= 2.;
                }
                return o / k;
            }

            float vor(float2 uv) {
                float2 id = floor(uv);
                float2 loc = frac(uv);
                float o = 100.;
                for (float x = -1.; x <= 1.; x++) {
                    for (float y = -1.; y <= 1.; y++) {
                        o = min(o, distance(sin(2.5 * pi * rand2x2(id + float2(x, y))) * 0.8 + 0.2, loc - float2(x, y)));
                    }
                }
                return o;
            }

            float3 vorid3(float2 uv) {
                float2 id = floor(uv);
                float2 loc = frac(uv);
                float o = 1000.;
                float3 ou = float3(0, 0, 0);
                for (float x = -1.; x <= 1.; x++) {
                    for (float y = -1.; y <= 1.; y++) {
                        float d = distance(sin(2.5 * pi * rand2x2(id + float2(x, y))) * 0.8 + 0.2, loc - float2(x, y));
                        if (o > d) {
                            o = d;
                            ou = rand2x3(id + float2(x, y));
                        }
                    }
                }
                return ou;
            }

            float3 star(float2 uv) {
                float val = vor(uv * 3.);
                val = 0.01 / val;
                val = pow(val, 1.7);
                float3 col = float3(val, val, val) * (vorid3(uv * 3.));
                return col * fperl(uv / 2., 0., 2.);
            }

            float3 fstar(float2 uv, float iter, float t) {
                float3 o = float3(0, 0, 0);
                float p = 1.;
                for (float i = 0.; i < iter; i++) {
                    o += star(rotate(uv + float2(t, 0.) / p, i) * p);
                    p *= 1.5;
                }
                return o;
            }

            float fnebula(float2 uv, float iter, float t) {
                float o = 0., p = 1.;
                for (float i = 0.; i < iter; i++) {
                    o += fperl(rotate(uv + float2(t, 0.) / p, i) * p / 2., 0., 6.);
                    p *= 1.5;
                }
                return o;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert UV from [0,1] to centered coordinates like Shadertoy
                float2 fragCoord = i.uv * _ScreenParams.xy;
                float2 uv = (2.0 * fragCoord - _ScreenParams.xy) / min(_ScreenParams.x, _ScreenParams.y);
                
                // Use Unity's _Time.y which provides time in seconds
                float time = _Time.y;
                
                float3 col = fstar(uv, 5., time / 5.);
                col *= 10.;
                col = pow(col, float3(1, 1, 1));
                
                // Color mixing - equivalent to the original commented line
                // col = col.r * vec3(1, 0.45, 0.4) + col.g * vec3(0.4, 0.4, 1) + col.b * vec3(1);
                col = col.r * float3(1, 0.45, 0.4) + col.g * float3(0.4, 0.4, 1) + col.b * float3(1, 1, 1);
                col = clamp(float3(0, 0, 0.03) + col, float3(0, 0, 0), float3(1, 1, 1));
                
                float n = fnebula(uv, 7., time / 5.);
                n = n * 0.4;
                n = clamp(n, 0., 1.);
                n = 1. - n;
                n = 0.5 / n;
                n = n - 0.5;
                n = max(0., n); // Clamp negative values to 0
                float3 vnb = n * float3(0.7, 0.1, 1);
                vnb = clamp(vnb, float3(0, 0, 0), float3(1, 1, 1));
                
                return fixed4((vnb + col) * 0.5, 1);
            }
            ENDCG
        }
    }
    FallBack Off
}