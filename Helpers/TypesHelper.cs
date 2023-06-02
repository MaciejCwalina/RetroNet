namespace RetroNet {
	public class TypesHelper {
		public static bool IsType(EToken type) {
			switch (type) {
				case EToken.STRING:
				case EToken.I32:
				case EToken.F32:
					return true;
				default:
					return false;
			}
		}
	}
}