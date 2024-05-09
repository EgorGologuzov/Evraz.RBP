using RBP.Services.Dto;

namespace RBP.Services.Interfaces
{
    public interface IHandbookValidator
    {
        void Validate(HandbookEntityCreateDto entity);
        void Validate(HandbookEntityUpdateDto entity);
    }
}