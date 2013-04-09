float4x4 matWorldViewProj;
float distance;
float range;
float near;
float far;
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


texture2D BlurredColorMap;
sampler2D BlurredColorMapSampler = sampler_state
{
	Texture = <BlurredColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
	AddressU = Clamp;
    AddressV = Clamp;
};


texture DepthMap;
sampler DepthMapSampler = sampler_state
{
   Texture = <DepthMap>;
   MinFilter = Linear;
   MagFilter = Linear;
   MipFilter = Linear;  
   AddressU  = Clamp;
   AddressV  = Clamp;
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
	float4 color = tex2D(ColorMapSampler, input.Texcoord);
	float4 blurColor = tex2D(BlurredColorMapSampler, input.Texcoord);		
	float depth = 1 - tex2D(DepthMapSampler, input.Texcoord).r;

	float linearZ = ( -near * far ) / ( depth - far ); //linearize the depth	
	float blurFactor = saturate( abs ( linearZ - distance ) / range );

	return lerp(color, blurColor, blurFactor);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 main_VS();
		PixelShader = compile ps_2_0 main_PS();
	}
}
