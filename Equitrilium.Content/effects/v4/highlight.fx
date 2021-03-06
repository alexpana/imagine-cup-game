float4x4 matWorldViewProj;
float fTimeMs;
float3 f3Color;

struct VertexShaderInput
{
    float4 Position : SV_POSITION;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : SV_POSITION;
};

VertexShaderOutput main_VS(VertexShaderInput input) 
{
	VertexShaderOutput output;

	float3 vertexScaled = (1.25 + 0.12 * sin(fTimeMs / 200)) * input.Position.xyz;
	output.ScreenPosition = mul( float4(vertexScaled, 1), matWorldViewProj );
	return output;
}

float4 main_PS(VertexShaderOutput input) : SV_Target
{
	return float4( f3Color, 0.2 );
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 main_VS();
		PixelShader = compile ps_4_0 main_PS();
	}
}
