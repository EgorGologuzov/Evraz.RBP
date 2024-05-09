using RBP.Services.Dto;
using RBP.Services.Interfaces;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.Services.Validators
{
    public class HandbookValidator : IHandbookValidator
    {
        public void Validate(HandbookEntityCreateDto entity)
        {
            entity.Name.CheckMatch(ValidationSettings.HandbookEntityNamePattern, nameof(entity.Name));
            entity.Comment.CheckLength(0, ValidationSettings.MaxCommentLength, nameof(entity.Comment));
        }

        public void Validate(HandbookEntityUpdateDto entity)
        {
            entity.Name.CheckMatch(ValidationSettings.HandbookEntityNamePattern, nameof(entity.Name));
            entity.Comment.CheckLength(0, ValidationSettings.MaxCommentLength, nameof(entity.Comment));
        }
    }
}