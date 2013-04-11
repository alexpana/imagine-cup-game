float4x4 matWorldViewProj;
float fTimeMs;
float3 f3Color;

struct VertexShaderInput
{
    float4 Position : POSITION;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : POSITION;
};

VertexShaderOutput main_VS(VertexShaderInput input) 
{
	VertexShaderOutput output;

	float3 vertexScaled = (1.25 + 0.12 * sin(fTimeMs / 200)) * input.Position.xyz + float3(1, 1, 1);
	output.ScreenPosition = mul( float4(vertexScaled, 1), matWorldViewProj );
	return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR
{
	return float4( f3Color, 0.2 );
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
