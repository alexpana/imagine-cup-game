float4x4 matWorldViewProj;

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float3 Tangent : TANGENT0;
	float2 Texcoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : POSITION;
	float3 Texcoord : TEXCOORD0;
	
};

VertexShaderOutput main_VS(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

	output.Texcoord.xy = input.Texcoord;
	output.ScreenPosition = mul(input.Position, matWorldViewProj);

    return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR
{
	return tex2D(ColorMapSampler, input.Texcoord.xy);
   
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
