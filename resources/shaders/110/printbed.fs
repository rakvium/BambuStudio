#version 110

const vec3 back_color_dark  = vec3(0.235, 0.235, 0.235);
const vec3 back_color_light = vec3(0.365, 0.365, 0.365);

uniform sampler2D u_sampler;
uniform bool transparent_background;
uniform bool svg_source;

varying vec2 tex_coord;

vec4 svg_color(vec2 uv)
{
    // takes foreground from u_sampler
    vec4 fore_color = texture2D(u_sampler, uv);

    // calculates radial gradient
    vec3 back_color = vec3(mix(back_color_light, back_color_dark, smoothstep(0.0, 0.5, length(abs(uv) - vec2(0.5)))));

    // blends foreground with background
    return vec4(mix(back_color, fore_color.rgb, fore_color.a), transparent_background ? fore_color.a : 1.0);
}

vec4 non_svg_color(vec2 uv)
{
    // takes foreground from u_sampler
    vec4 color = texture2D(u_sampler, uv);
    return vec4(color.rgb, transparent_background ? color.a * 0.25 : color.a);
}

void main()
{
    // flip uv
    vec2 uv = vec2(tex_coord.x, 1.0 - tex_coord.y);
    gl_FragColor = svg_source ? svg_color(uv) : non_svg_color(uv);
}