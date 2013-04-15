float4x4 World;
float4x4 View;
float4x4 Projection;

Texture TextureBackground;
Texture TextureSpots;
float ValRandom;
float ValTime;

sampler BackgroundSampler = sampler_state {
	texture = <TextureBackground>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU  = mirror;
	AddressV  = mirror;
};

sampler SpotsSampler = sampler_state {
	texture = <TextureSpots>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU  = mirror;
	AddressV  = mirror;
};

struct VertexShaderInput
{
    float4 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	float4 position = input.Position;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.TexCoord = input.TexCoord;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2x2 rotation = { cos(ValTime), sin(ValTime), -sin(ValTime), cos(ValTime) };
	float2 uvCoord = mul( input.TexCoord.xy, rotation );
	float4 back = tex2D( BackgroundSampler, uvCoord );
	float4 spots = tex2D( SpotsSampler, uvCoord );
	return back * 0.7 + back * 0.3 * (spots * ValTime);
}

technique HighLOD
{
    pass Diffuse
    {
        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}
