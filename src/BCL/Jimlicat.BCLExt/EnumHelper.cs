using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Jimlicat.Extensions
{
	/// <summary>
	/// 枚举字段
	/// </summary>
	public class EnumField
	{
		/// <summary>
		/// 枚举值
		/// </summary>
		public int Value { get; set; }
		/// <summary>
		/// 枚举名
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 枚举描述
		/// </summary>
		public virtual string Description { get; set; }
		/// <summary>
		/// 枚举字段信息
		/// </summary>
		internal FieldInfo FieldInfo { get; set; }
	}

	/// <summary>
	/// 枚举帮助
	/// </summary>
	public class EnumHelper
	{
		private static readonly Assembly LocalAssembly;

		private static ImmutableDictionary<Type, EnumField[]> EnumFiledCache;

		private static ImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>> FiledInfoCultureDescCache;

		static EnumHelper()
		{
			LocalAssembly = typeof(EnumHelper).Assembly;
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			ImmutableDictionary<Type, EnumField[]>.Builder builder = ImmutableDictionary.CreateBuilder<Type, EnumField[]>();
			ImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>>.Builder builder2 = ImmutableDictionary.CreateBuilder<FieldInfo, ImmutableDictionary<CultureInfo, string>>();
			TypeInfo[] array = LocalAssembly.DefinedTypes.Where((TypeInfo x) => x.IsEnum).ToArray();
			foreach (TypeInfo typeInfo in array)
			{
				EnumField[] enumFieldsInner = GetEnumFieldsInner(typeInfo, currentCulture);
				builder.Add(typeInfo, enumFieldsInner);
				EnumField[] array2 = enumFieldsInner;
				foreach (EnumField enumField in array2)
				{
					ImmutableDictionary<CultureInfo, string>.Builder builder3 = ImmutableDictionary.CreateBuilder<CultureInfo, string>();
					builder3.Add(currentCulture, enumField.Description);
					builder2.Add(enumField.FieldInfo, ImmutableDictionary.ToImmutableDictionary<CultureInfo, string>(builder3));
				}
			}
			EnumFiledCache = ImmutableDictionary.ToImmutableDictionary<Type, EnumField[]>(builder);
			FiledInfoCultureDescCache = ImmutableDictionary.ToImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>>(builder2);
		}

		private static EnumField[] GetEnumFieldsInner(Type enumType, CultureInfo culture)
		{
			FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			int num = fields.Length;
			EnumField[] array = new EnumField[num];
			for (int i = 0; i < num; i++)
			{
				FieldInfo fieldInfo = fields[i];
				EnumField enumField = array[i] = new EnumField
				{
					Name = fieldInfo.Name,
					Value = Convert.ToInt32(fieldInfo.GetRawConstantValue()),
					Description = GetResourceStringInner(fieldInfo, culture),
					FieldInfo = fieldInfo
				};
			}
			return array;
		}

		private static string GetResourceStringInner(FieldInfo fieldInfo, CultureInfo culture)
		{
			DescriptionAttribute customAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
			string text = customAttribute?.Description?.Trim();
			string name = (!string.IsNullOrEmpty(text)) ? customAttribute.Description : ("Enum_" + fieldInfo.DeclaringType.Name + "_" + fieldInfo.Name);
			//string @string = Resources.ResourceManager.GetString(name, culture);
			string @string = "获取资源值";
			if (string.IsNullOrEmpty(@string) && !string.IsNullOrEmpty(text))
			{
				return text;
			}
			return @string;
		}

		private static string GetDescriptionInner(FieldInfo fieldInfo, CultureInfo culture)
		{
			string resourceStringInner;
			if (!FiledInfoCultureDescCache.TryGetValue(fieldInfo, out ImmutableDictionary<CultureInfo, string> value))
			{
				ImmutableDictionary<CultureInfo, string>.Builder builder = ImmutableDictionary.CreateBuilder<CultureInfo, string>();
				resourceStringInner = GetResourceStringInner(fieldInfo, culture);
				builder.Add(culture, resourceStringInner);
				value = ImmutableDictionary.ToImmutableDictionary<CultureInfo, string>(builder);
				ImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>>.Builder builder2 = FiledInfoCultureDescCache.ToBuilder();
				builder2.Add(fieldInfo, value);
				FiledInfoCultureDescCache = ImmutableDictionary.ToImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>>(builder2);
				return resourceStringInner;
			}
			if (!value.TryGetValue(culture, out resourceStringInner))
			{
				resourceStringInner = GetResourceStringInner(fieldInfo, culture);
				ImmutableDictionary<CultureInfo, string>.Builder builder3 = value.ToBuilder();
				builder3.Add(culture, resourceStringInner);
				ImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>>.Builder builder4 = FiledInfoCultureDescCache.ToBuilder();
				builder4[fieldInfo] = ImmutableDictionary.ToImmutableDictionary<CultureInfo, string>(builder3);
				FiledInfoCultureDescCache = ImmutableDictionary.ToImmutableDictionary<FieldInfo, ImmutableDictionary<CultureInfo, string>>(builder4);
			}
			return resourceStringInner;
		}
		/// <summary>
		/// 枚举转换为字典
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="getText"></param>
		/// <returns></returns>
		public static Dictionary<int, string> EnumToDictionary(Type enumType, Func<EnumField, string> getText = null)
		{
			return EnumToDictionary(enumType, CultureInfo.CurrentCulture, getText);
		}
		/// <summary>
		/// 枚举转换为字典
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="culture"></param>
		/// <param name="getText"></param>
		/// <returns></returns>
		public static Dictionary<int, string> EnumToDictionary(Type enumType, CultureInfo culture, Func<EnumField, string> getText = null)
		{
			EnumField[] enumFields = GetEnumFields(enumType, culture);
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			EnumField[] array = enumFields;
			foreach (EnumField enumField in array)
			{
				int value = enumField.Value;
				string value2;
				if (getText != null)
				{
					value2 = getText(enumField);
				}
				else
				{
					value2 = enumField.Description;
					if (string.IsNullOrEmpty(value2))
					{
						value2 = enumField.Name;
					}
				}
				dictionary.Add(value, value2);
			}
			return dictionary;
		}
		/// <summary>
		/// 获得枚举字段
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public static EnumField[] GetEnumFields(Type enumType)
		{
			return GetEnumFields(enumType, CultureInfo.CurrentCulture);
		}
		/// <summary>
		/// 获取枚举字段
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public static EnumField[] GetEnumFields(string enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			KeyValuePair<Type, EnumField[]> keyValuePair = EnumFiledCache.FirstOrDefault((KeyValuePair<Type, EnumField[]> x) => x.Key.Name == enumType || x.Key.FullName == enumType);
			if (keyValuePair.Key == null)
			{
				return Array.Empty<EnumField>();
			}
			return GetEnumFields(keyValuePair.Key);
		}
		/// <summary>
		/// 获得枚举字段
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public static EnumField[] GetEnumFields(Type enumType, CultureInfo culture)
		{
			if (!enumType.IsEnum)
			{
				//throw new ArgumentException(Resources.Arg_MustBeEnum, "enumType");
				throw new ArgumentException("参数必须是枚举类型");
			}
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			if (!EnumFiledCache.TryGetValue(enumType, out EnumField[] value))
			{
				value = GetEnumFieldsInner(enumType, culture);
				EnumFiledCache = EnumFiledCache.Add(enumType, value);
			}
			if (enumType.Assembly == LocalAssembly)
			{
				EnumField[] array = value;
				foreach (EnumField obj in array)
				{
					obj.Description = GetDescriptionInner(obj.FieldInfo, culture);
				}
			}
			return value;
		}

		internal static string GetDescription(Enum value, CultureInfo culture, bool nameInstead = true)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (string.IsNullOrEmpty(name))
			{
				return value.ToString();
			}
			string descriptionInner = GetDescriptionInner(type.GetField(name), culture);
			if (string.IsNullOrEmpty(descriptionInner) && nameInstead)
			{
				return name;
			}
			return descriptionInner;
		}
	}
}