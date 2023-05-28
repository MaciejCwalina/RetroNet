namespace RetroNet {
	public class EnumHelper {
		public static Boolean Contains<TEnum>(String name) where TEnum : Enum {
			if (String.IsNullOrEmpty(name)) {
				return false;
			}

			if (Char.IsSymbol(name[0]) || Char.IsPunctuation(name[0])) {
				return true;
			}

			return Enum.GetNames(typeof(TEnum)).Count(n => n == name.ToUpper()) > 0;
		}

		public static TEnum GetEnumValueByName<TEnum>(String name) where TEnum : Enum {
			Type enumType = typeof(TEnum);
			if (!enumType.IsEnum) {
				throw new ArgumentException("TEnum must be an enumeration type.");
			}

			//Krzyczało tu coś o konwersji możliwego typu null na typ nienullowalny, dodałem ? do typu
			String? enumName = Enum.GetNames(enumType).FirstOrDefault(n => String.Equals(n, name, StringComparison.OrdinalIgnoreCase));

			if (enumName != null) {
				return (TEnum)Enum.Parse(enumType, enumName);
			}

			return (TEnum)(Object)EToken.UNDEFINED;
		}
	}
}