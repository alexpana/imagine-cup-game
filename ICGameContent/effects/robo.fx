float4x4 matWorldViewProj;
float4x4 matWorldViewInverseTranspose;
float4x4 matWorld;

// This is the texture that SpriteBatch will try to set before drawing 
// Our sampler for the texture, which is just going to be pretty simple

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL;	
};

struct VertexShaderOutput
{
	float4 ScreenPosition : POSITION0;
    float4 WorldPosition : COLOR0;
	float3 Normal : COLOR1;
};

VertexShaderOutput main_VS(VertexShaderInput input)
{
    VertexShaderOutput output;
    
	output.WorldPosition = mul(input.Position, matWorld);
	output.Normal = mul(float4(input.Normal, 0.0), matWorldViewInverseTranspose).xyz;
	output.ScreenPosition = mul(input.Position, matWorldViewProj);

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
