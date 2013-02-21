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

    return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR0
{
    return float4(1,1,1,1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
