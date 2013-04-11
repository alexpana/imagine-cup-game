float4x4 matWorldViewProj;
float distance;
float dof_range;
float no_dof_range;
float near;
float far;
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

texture2D DepthMap;
sampler2D DepthMapSampler = sampler_state
{
   Texture = <DepthMap>;
   MinFilter = linear;
   MagFilter = linear;
   MipFilter = linear;  
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
	float4 blurColor;

	blurColor  = tex2D( ColorMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y + blurDistance));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y - blurDistance));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y - blurDistance));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y + blurDistance));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x, input.Texcoord.y + blurDistance));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x, input.Texcoord.y - blurDistance));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y));
	blurColor += tex2D( ColorMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y));



	blurColor = blurColor / 8; 


	float4 color = tex2D(ColorMapSampler, input.Texcoord);
	float depth = tex2D(DepthMapSampler, input.Texcoord).x;

	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y + blurDistance)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y - blurDistance)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y + blurDistance)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y - blurDistance)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x + blurDistance, input.Texcoord.y)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x, input.Texcoord.y + blurDistance)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x - blurDistance, input.Texcoord.y)));
	depth  = min(depth, tex2D( DepthMapSampler, float2(input.Texcoord.x, input.Texcoord.y - blurDistance)));


	float dd = abs ( depth * ( far + near ) - distance );

	float blurFactor = saturate(  ( dd - no_dof_range ) / dof_range );

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
