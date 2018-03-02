#version 330

in vec2 f_texcoord;
out vec4 outputColor;

uniform sampler2D maintexture;

void main()
{
	vec2 flipped_texcoord = vec2(f_texcoord.x, 1.0 - f_texcoord.y);
    vec4 texel = texture(maintexture, flipped_texcoord);
	if (texel.a < 0.5)
	{
		discard;
	}
	outputColor = texel;
}