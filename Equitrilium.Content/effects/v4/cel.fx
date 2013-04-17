float4x4 matWorldViewProj;
float4x4 matWorldInverseTranspose;
float3 lightPosition;
float3 eyePosition;

struct VertexShaderInput
{
    float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
	float3 Tangent : TANGENT0;
	float2 Texcoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 ScreenPosition : SV_POSITION;
	float2 Texcoord : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 ToLight : TEXCOORD2;
	float3 ToEye : TEXCOORD3;
};

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};

VertexShaderOutput vertex_shader(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 posW = mul( input.Position, matWorldViewProj );

	output.ScreenPosition = posW;
	output.Texcoord = input.Texcoord;
	output.Normal = mul( float4( input.Normal, 0 ), matWorldViewProj ).xyz;
	output.ToLight = lightPosition - posW;
	output.ToEye = eyePosition - posW;

	return output;
}

float4 main_PS(VertexShaderOutput input) : SV_Target
{
	float step1 = 0.3;
	float step2 = 0.9;
	
	float3 C = normalize(input.ToEye);
	float3 L = normalize(input.ToLight);
	float3 N = normalize(input.Normal);

	// Edge detection
	float rim = 1;//dot(C, N ) > 0.001 ? 1 : 0;

	float ndotl = saturate( dot( L, normalize(input.Normal) ) / 2.0 + 0.5 );
	float Kd = (step( step2, ndotl ) * 0.33) + (step( step1, ndotl ) * 0.33) + 0.3;

	// uncomment for lambertian reflectance
	Kd = ndotl;

	Kd = Kd * rim;

	float3 color = float3( 0.6, 0.3, 0.2 );

	// BLUE PRINT
	// -----------------------------------------------------------------------------
	float gridLineSIze = 0.02;
	float gridSize = 0.2;
	float gridLineX = step( fmod( input.Texcoord.x, gridSize ), gridLineSIze );
	float gridLineY = step( fmod( input.Texcoord.y / 2, gridSize), gridLineSIze );
	float grid = clamp( gridLineX + gridLineY, 0, 1 );
	float3 gridColor = float3( 0.9, 0.9, 0.9 );
	//color = grid * gridColor + ( 1 - grid ) * color;

	// TEXTURE
	// -----------------------------------------------------------------------------
	//color = tex2D( ColorMapSampler, input.Texcoord );


	return float4( color * Kd, 1.0 );
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 vertex_shader();
		PixelShader = compile ps_4_0 main_PS();
	}
}
