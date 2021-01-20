
void GetMainLightInformation_float(out float3 Position, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
	Position = float3(0.5, 1.5, 0.5);
	Direction = float3(-0.5, 0.5, -0.5);
	Color = float3(1, 1, 1);
	Attenuation = 0.4;
#else
	Light light = GetMainLight();

	Position = _MainLightPosition;
	Direction = light.direction;
	Color = light.color;
	Attenuation = light.distanceAttenuation;
#endif
}