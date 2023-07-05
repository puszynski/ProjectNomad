using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record TribeDto(int Id,
        string Name, 
        int X, 
        int Y) : ITribe;
}
