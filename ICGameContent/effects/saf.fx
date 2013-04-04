float4x4 matWorldViewProj;
float3 eyePosition;
float fTime;
float fAlpha;
float2 fVel;

struct VertexShaderInput
{
    float4 Position : POSITION;
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
	AddressU = Wrap;
    AddressV = Wrap;
};


texture2D AlphaMap;
sampler2D AlphaMapSampler = sampler_state
{
	Texture = <AlphaMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
	AddressU = Wrap;
    AddressV = Wrap;
};

VertexShaderOutput main_VS(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.ScreenPosition = mul( input.Position, matWorldViewProj );
	output.Texcoord = input.Texcoord;

	return output;
}

float4 main_PS(VertexShaderOutput input) : COLOR
{
	float2 texcoord = input.Texcoord.xy + float2(fTime * fVel.x, fTime * fVel.y);


	float4 color = tex2D(ColorMapSampler, texcoord);
	float alpha = tex2D(AlphaMapSampler, input.Texcoord.xy).a;

	return float4( color.xyz, fAlpha * alpha );
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
