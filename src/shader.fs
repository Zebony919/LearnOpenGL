#version 330 core

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shineness;
};
uniform Material material;

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
uniform Light sourceLight;

struct FlashLight {
    vec3 position;
    vec3 direction;
    float radius;
};
uniform FlashLight flashLight;

uniform vec3 objectColor;
uniform vec3 lightPos;
uniform vec3 camPos;

uniform float mixFactor;
uniform float toggleFlash;

void main()
{
    float theta = dot(normalize(camPos - FragPos), normalize(-flashLight.direction));

    float distance = length(sourceLight.position - FragPos);
    float attenuation = 1.0 / (sourceLight.constant + sourceLight.linear * distance + 
                        sourceLight.quadratic * (distance * distance));

    vec3 ambient = sourceLight.ambient * vec3(texture(material.diffuse, TexCoords));

    vec3 norm = normalize(Normal);
    vec3 sourceLightDir = normalize(sourceLight.position - FragPos);
    float diff = max(dot(norm, sourceLightDir), 0.0);
    vec3 diffuse = (diff * vec3(texture(material.diffuse, TexCoords))) * sourceLight.diffuse;

    vec3 viewDir = normalize(camPos - FragPos);
    vec3 reflectDir = reflect(-sourceLightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shineness);
    vec3 specular = (spec * vec3(texture(material.specular, TexCoords))) * sourceLight.specular;

    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    if ((theta > flashLight.radius) && (toggleFlash == 1.0f)) {
        ambient = min(ambient * 1.7, 1.0);
        diffuse = min(diffuse * 1.7, 1.0);
    };

    vec3 result = (ambient + diffuse + specular);
    FragColor = vec4(result, 1.0);
}