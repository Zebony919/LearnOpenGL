#version 330 core

out vec4 FragColor;

in vec3 ourPosition;
in vec3 ourColour;
in vec2 TexCoords;

uniform sampler2D texture1;
uniform sampler2D texture2;

void main()
{
    vec4 tex1 = texture(texture1, TexCoords);
    vec4 tex2 = texture(texture2, TexCoords);
    FragColor = mix(tex1, tex2, 0.2);
}