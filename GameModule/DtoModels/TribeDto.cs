using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record TribeDto(string Name, 
        int X, 
        int Y) : ITribe;
}
