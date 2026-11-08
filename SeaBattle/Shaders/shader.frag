#version 330 core

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;

uniform sampler2D tex;
uniform vec3 lightPos;
uniform vec3 viewPos;

void main()
{
    vec3 norm = normalize( Normal );
    vec3 lightDir = normalize( lightPos - FragPos );

    float diff = max( dot( norm, lightDir ), 0.2 );

    vec4 color = texture( tex, TexCoord );

    FragColor = vec4( color.rgb * diff, 1.0 );
}