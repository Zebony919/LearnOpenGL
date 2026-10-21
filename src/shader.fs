#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform float mixFactor;

void main()
{
    FragColor = vec3(objectColor * objectColor, 1.0);
}