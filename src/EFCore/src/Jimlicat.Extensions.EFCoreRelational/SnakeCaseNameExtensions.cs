using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 蛇形(ab_cd)命名扩展
    /// </summary>
    public static class SnakeCaseNameExtensions
    {
        /// <summary>
        /// 蛇形字符串
        /// </summary>
        /// <param name="name">字符串</param>
        /// <returns></returns>
        public static string ToSnake(this string name)
        {
            return ToSnake(name, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 蛇形字符串
        /// </summary>
        /// <param name="name">字符串</param>
        /// <param name="culture">文化</param>
        /// <returns></returns>
        public static string ToSnake(this string name, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            var builder = new StringBuilder(name.Length + Math.Min(2, name.Length / 5));
            var previousCategory = default(UnicodeCategory?);

            for (var currentIndex = 0; currentIndex < name.Length; currentIndex++)
            {
                var currentChar = name[currentIndex];
                if (currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                var currentCategory = char.GetUnicodeCategory(currentChar);
                switch (currentCategory)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        if (previousCategory == UnicodeCategory.SpaceSeparator ||
                            previousCategory == UnicodeCategory.LowercaseLetter ||
                            previousCategory != UnicodeCategory.DecimalDigitNumber &&
                            previousCategory != null &&
                            currentIndex > 0 &&
                            currentIndex + 1 < name.Length &&
                            char.IsLower(name[currentIndex + 1]))
                        {
                            builder.Append('_');
                        }

                        currentChar = char.ToLower(currentChar, culture);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (previousCategory == UnicodeCategory.SpaceSeparator)
                            builder.Append('_');
                        break;

                    default:
                        if (previousCategory != null)
                            previousCategory = UnicodeCategory.SpaceSeparator;
                        continue;
                }

                builder.Append(currentChar);
                previousCategory = currentCategory;
            }
            return builder.ToString();
        }
    }
}
