namespace ProjectNomad.Shared.DTOs.ServerToWasm;

public record WorldParametersDto(
    int AverageTemperature, 
    bool IsWind, 
    bool IsRain, 
    bool IsSnow, 
    bool IsBlizzard);
