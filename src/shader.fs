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

struct DirLight {
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform DirLight dirLight;
uniform DirLight sunLight;

struct PointLight {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float constant;
    float linear;
    float quadratic;
};
uniform PointLight sourceLight;
#define N_POINT_LIGHTS 4
uniform PointLight pointLights[N_POINT_LIGHTS];

struct FlashLight {
    vec3 position;
    vec3 direction;
    float radius;
    float outerRadius;
};
uniform FlashLight flashLight;

uniform vec3 camPos;
uniform float toggleFlash;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shineness);

    vec3 texColor = texture(material.diffuse, TexCoords).rgb;
    vec3 ambient = light.ambient * texColor;
    vec3 diffuse = light.diffuse * diff * texColor;
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;

    return ambient + diffuse + specular;
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shineness);

    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    vec3 texColor = texture(material.diffuse, TexCoords).rgb;
    vec3 ambient = light.ambient * texColor;
    vec3 diffuse = light.diffuse * diff * texColor;
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;

    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    return ambient + diffuse + specular;
}

vec3 CalcFlashLight(FlashLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shineness);

    vec3 lightToFragment = normalize(fragPos - light.position);
    float theta = dot(lightToFragment, normalize(light.direction));
    float epsilon = light.radius - light.outerRadius;
    float intensity = clamp((theta - light.outerRadius) / epsilon, 0.0, 1.0);

    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (1.0 + 0.09 * distance + 0.032 * (distance * distance));

    vec3 texColor = texture(material.diffuse, TexCoords).rgb;
    vec3 ambient = vec3(0.05) * texColor * intensity;
    vec3 diffuse = vec3(1.0) * diff * texColor * intensity;
    vec3 specular = vec3(1.0) * spec * texture(material.specular, TexCoords).rgb * intensity;

    return ambient + diffuse * attenuation + specular * attenuation;
}

void main()
{
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(camPos - FragPos);
    vec3 result = vec3(0.0);

    result += CalcDirLight(sunLight, norm, viewDir);
    result += CalcPointLight(sourceLight, norm, FragPos, viewDir);

    for (int i = 0; i < N_POINT_LIGHTS; i++) {
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
    }

    if (toggleFlash > 0.5) {
        result += CalcFlashLight(flashLight, norm, FragPos, viewDir);
    }

    FragColor = vec4(result, 1.0);
}