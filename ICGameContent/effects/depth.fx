float4x4 matWorldViewProj;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : POSITION;
	float Distance : TEXCOORD0;
};


VertexShaderOutput main_VS(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	output.ScreenPosition = mul(input.Position, matWorldViewProj);	
	output.Distance = 1 - output.ScreenPosition.z / output.ScreenPosition.w;
	return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR
{
	return float4(input.Distance,0,0,1);
}

technique Technique1
{
	pass Pass1
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
