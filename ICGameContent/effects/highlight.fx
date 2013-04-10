float4x4 matWorldViewProj;
float fTimeMs;

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

	float3 vertexScaled = (1.4 + 0.3 * sin(fTimeMs / 1000)) * input.Position.xyz;
	output.ScreenPosition = mul( float4(vertexScaled, 1), matWorldViewProj );
	return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR
{
	return float4( 1.0, 0.0, 1.0, 0.3 );
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
