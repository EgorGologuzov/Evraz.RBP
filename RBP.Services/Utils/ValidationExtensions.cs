using System.Collections;
using System.Text.RegularExpressions;
using RBP.Services.Exceptions;
using RBP.Services.Static;

namespace RBP.Services.Utils
{
    public static class ValidationExtensions
    {
        public static void CheckMatch(this string input, string pattern, string fieldName, string message = null)
        {
            if (input is null || !Regex.IsMatch(input, pattern))
            {
                throw new InvalidFieldValueException(fieldName, message ?? $"Поле должно соответствовать паттерну '{pattern}'");
            }
        }

        public static void CheckLength(this string input, int min, int max, string fieldName, string message = null)
        {
            int length = string.IsNullOrEmpty(input) ? 0 : input.Length;

            if (length < min || length > max)
            {
                throw new InvalidFieldValueException(fieldName, message ?? $"Длина строки должна быть от {min} до {max} символов");
            }
        }

        public static void CheckContains<T>(this T input, IEnumerable<T> variants, string fieldName, string message = null)
        {
            if (input is null || !variants.Contains(input))
            {
                throw new InvalidFieldValueException(fieldName, message ?? $"Значение должно быть вариантом из {variants.ToJson()}");
            }
        }

        public static void CheckRange<T>(this T input, T min, T max, string fieldName, string message = null)
            where T : IComparable<T>
        {
            if (input is null || (min is not null && input.CompareTo(min) < 0) || (max is not null && input.CompareTo(max) > 0))
            {
                throw new InvalidFieldValueException(fieldName, message ?? $"Значение должно быть в диапозоне от {min} до {max}");
            }
        }

        public static T CheckParseJson<T>(this string json, string fieldName, string message = null)
        {
            try
            {
                return json.FromJson<T>();
            }
            catch
            {
                throw new InvalidFieldValueException(fieldName, message ?? "Неверный формат JSON");
            }
        }

        public static void ThrowIfNull(this object input, Exception exception)
        {
            if (input is null)
            {
                throw exception;
            }
        }

        public static void CheckPassword(this string password, string fieldName, string message = null)
        {
            if (password is null || !Regex.IsMatch(password, ValidationSettings.PasswordPattern))
            {
                throw new InvalidFieldValueException(fieldName, message ?? "Пароль должен содержать от 8 до 25 символов, одну маленькую и одну большую латинские буквы, цифру и специальный символ");
            }
        }

        public static void CheckCount<T>(this IList<T> list, int min, int max, string fieldName, string message = null)
        {
            int length = (list?.Count) ?? 0;

            if (length < min || length > max)
            {
                throw new InvalidFieldValueException(fieldName, message ?? $"Количество элементов должно быть от {min} до {max}");
            }
        }

        public static void CheckEach<T>(this IList<T> list, Action<T> checkAction)
        {
            if (list is null)
            {
                return;
            }

            foreach (var entity in list)
            {
                checkAction.Invoke(entity);
            }
        }

        public static void CheckNotNull(this object input, string fieldName, string message = null)
        {
            if (input is null)
            {
                throw new InvalidFieldValueException(fieldName, message ?? "Значение должно быть определено");
            }
        }

        public static void CheckNotGreater<T>(this T input, T max, string fieldName, string message = null)
            where T : IComparable<T>
        {
            if (input.CompareTo(max) > 0)
            {
                throw new InvalidFieldValueException(fieldName, message ?? $"Значение должно быть меньше или равно {max}");
            }
        }

        public static void ThrowEntityNotExistsIfNull(this object input, object id)
        {
            if (input is null)
            {
                throw new EntityNotExistsException(id);
            }
        }

        public static void CheckIsEqual<T>(this T object1, T object2, string fieldName, string message = null)
            where T : IComparable<T>
        {
            if (object1.CompareTo(object2) != 0)
            {
                throw new InvalidFieldValueException(fieldName, message ?? "Значение не равно заданному");
            }
        }
    }
}