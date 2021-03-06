float4x4 matWorldViewProj;
float4x4 matWorldInverseTranspose;
float4x4 matWorld;
float3 eyePosition;
float3 lightPosition;
float2 uvScale;
// This is the texture that SpriteBatch will try to set before drawing 
// Our sampler for the texture, which is just going to be pretty simple

struct VertexShaderInput
{
    float4 Position : SV_POSITION;
	float3 Normal : NORMAL0;
	float3 Tangent : TANGENT0;
	float2 Texcoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : SV_POSITION;
	float3 Texcoord : TEXCOORD0;
	float3 ToLightT : TEXCOORD1;
	float3 ToEyeT : TEXCOORD2;
	
};

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
	AddressU = Wrap;
    AddressV = Wrap;
	MipLODBias = -1;
};

texture2D NormalMap;
sampler2D NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
	MipLODBias = -1;
	AddressU = Wrap;
    AddressV = Wrap;
};

texture2D SpecularMap;
sampler2D SpecularMapSampler = sampler_state
{
	Texture = <SpecularMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
	AddressU = Wrap;
    AddressV = Wrap;
};


VertexShaderOutput main_VS(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

	float3 posW = mul(float4(input.Position.xyz, 1.0), matWorld).xyz;
    float3 normalW = normalize(mul(float4(input.Normal, 0.0), matWorldInverseTranspose).xyz);
	float3 tangentW = normalize(mul(float4(input.Tangent, 0.0), matWorldInverseTranspose).xyz);
	float3 bitangentW = normalize(cross(normalW, tangentW));

	output.Texcoord.xy = input.Texcoord.xy;

	float3 toEyeW = eyePosition - posW;
	float3 toLightW = lightPosition - posW;
	
	output.ScreenPosition = mul(input.Position, matWorldViewProj);

	output.ToLightT.x = dot(tangentW, toLightW);
	output.ToLightT.y = dot(bitangentW, toLightW);
	output.ToLightT.z = dot(normalW, toLightW);
   
	output.ToEyeT.x = dot(tangentW, toEyeW);
	output.ToEyeT.y = dot(bitangentW, toEyeW);
	output.ToEyeT.z = dot(normalW, toEyeW);

    return output;
}

float4 main_PS(VertexShaderOutput input) : SV_Target
{
	float3 N = (2.0 * tex2D(NormalMapSampler, input.Texcoord.xy) - 1.0).xyz;

	float3 L = normalize(input.ToLightT);
	float3 E = normalize(input.ToEyeT);
	float3 R = -reflect(L, N);

	float Kd = max(dot(L, N), 0.0);
	float Ks = pow(max(dot(E, R), 0.0), 5);
	float Ka = 0.3;


	float4 ambient = Ka * tex2D( ColorMapSampler, input.Texcoord.xy);
	float4 diffuse = Kd * tex2D( ColorMapSampler, input.Texcoord.xy);
	float4 specular = Ks * tex2D( SpecularMapSampler, input.Texcoord.xy);

	return diffuse + ambient + specular;
   
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 main_VS();
		PixelShader = compile ps_4_0 main_PS();
	}
}
