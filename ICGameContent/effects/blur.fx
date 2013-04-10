float4x4 matWorldViewProj;
float blurDistance;
// This is the texture that SpriteBatch will try to set before drawing 
// Our sampler for the texture, which is just going to be pretty simple

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : POSITION;
	float2 Texcoord : TEXCOORD0;	
};

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
	AddressU = Clamp;
	AddressV = Clamp;
};


VertexShaderOutput main_VS(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	output.ScreenPosition = mul(input.Position, matWorldViewProj);
	output.Texcoord = input.Texcoord;
	return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR
{

	float4 color;

	color  = tex2D( ColorMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y + blurDistance));
	color += tex2D( ColorMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y - blurDistance));
	color += tex2D( ColorMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y - blurDistance));
	color += tex2D( ColorMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y + blurDistance));
	color = color / 4; 

	return color;

}

float4 depth_PS(VertexShaderOutput input) : COLOR
{
	float d = tex2D(ColorMapSampler, input.Texcoord).x;

	
	return float4(d, d, d, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 depth_PS();
	}
}
