using System.Collections;
using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.Services.Validators
{
    public class ProductValidator : IValidator<Product>
    {
        public void Validate(Product product)
        {
            product.Name.CheckMatch(ValidationSettings.ProductNamePattern, nameof(product.Name));
            product.Comment.CheckLength(0, ValidationSettings.MaxCommentLength, nameof(product.Comment));
            IList<KeyValuePair<string, string>> properties = product.PropertiesJson.CheckParseJson<IList<KeyValuePair<string, string>>>(nameof(product.PropertiesJson));
            properties.CheckCount(0, ValidationSettings.ProductPropertiesMaxCount, nameof(product.PropertiesJson));
            properties.CheckEach(p =>
            {
                p.Key.CheckMatch(ValidationSettings.PropertyNamePattern, nameof(p.Key));
                p.Value.CheckMatch(ValidationSettings.PropertyValuePattern, nameof(p.Value));
            });
        }
    }
}