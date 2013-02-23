float4x4 matWorldViewProj;
float4x4 matWorldViewInverseTranspose;
float4x4 matWorld;
float3 eyePosition;
float3 lightPosition;
// This is the texture that SpriteBatch will try to set before drawing 
// Our sampler for the texture, which is just going to be pretty simple

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
	float2 Texcoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : POSITION0;
	float3 ToLightT : COLOR0;
	float3 ToEyeT : COLOR1;
	float2 Texcoord : TEXCOORD0;
};

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};

texture2D NormalMap;
sampler2D NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};


VertexShaderOutput main_VS(VertexShaderInput input)
{
    VertexShaderOutput output;

	float3 posW = mul(input.Position, matWorld).xyz;
    float3 normalW = normalize(mul(float4(input.Normal, 0.0), matWorldViewInverseTranspose).xyz);
	float3 tangentW = normalize(mul(float4(input.Tangent, 0.0), matWorldViewInverseTranspose).xyz);

	float3 bitangentW = normalize(cross(tangentW, normalW));

	output.Texcoord = input.Texcoord;

	float3 toEyeW = eyePosition.xyz - posW;
	float3 toLightW = lightPosition - posW;
	
	output.ScreenPosition = mul(input.Position, matWorldViewProj);

	output.ToLightT.x = dot(tangentW, toLightW);
	output.ToLightT.y = dot(bitangentW, toLightW);
	output.ToLightT.z = dot(normalW, toLightW);
   
	output.ToEyeT.x = dot(tangentW, toEyeW);
	output.ToEyeT.y = dot(bitangentW, toEyeW);
	output.ToEyeT.z = dot(normalW, toEyeW);

	//output.ToEyeT = normalW / 2 + 1; 

    return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR0
{
	float3 N = (2.0 * tex2D(NormalMapSampler, input.Texcoord) - 1).xyz;
	float3 L = normalize(input.ToLightT);
	float3 E = normalize(input.ToEyeT);
	float3 R = -reflect(L, N);

	float Kd = max(dot(L, N), 0.0);
	float Ks = pow(max(dot(E, R), 0.0), 20);

	float4 diffuse = Kd * tex2D( ColorMapSampler, input.Texcoord);
	float4 specular = Ks * float4(1,1,1,1);


    return diffuse + specular;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
