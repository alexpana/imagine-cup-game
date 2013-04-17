float4x4 matWorldViewProj;

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : SV_POSITION;
	float4 Distance		  : TEXCOORD0;
};


VertexShaderOutput main_VS(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 posS = mul(float4(input.Position.xyz, 1.0), matWorldViewProj);
	output.ScreenPosition = posS;
	output.Distance = posS;
	return output;
}

float4 main_PS(VertexShaderOutput input) : SV_Target
{

	float d = input.Distance.z/input.Distance.w;
	float near = 1;
	float far = 10000;

	float Zback = (- near * far) / ( (near - far) * d + far );
	float Znorm = Zback / (-far - near);

	return float4(Znorm,0,0,0);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 main_VS();
		PixelShader = compile ps_4_0 main_PS();
	}
}
