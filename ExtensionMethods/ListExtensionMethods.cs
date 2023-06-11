using RetroNet.Interfaces;

namespace RetroNet.ExtensionMethods {
	public static class ListExtensionMethods {
		public static void AddIVariable(this List<IVariable> list, IVariable variableToAdd) {
			if (list.All(x => x.name != variableToAdd.name)) {
				list.Add(variableToAdd);
				return;
			}

			Int32 index = list.IndexOf(list.Where(x=>x.name==variableToAdd.name).ElementAt(0));
			IVariable[] variables = list.ToArray();
			variables[index].value = variableToAdd.value;
		}

		public static void AddParameter(this List<Parameter> list, Parameter parameterToAdd) {
			if (list.All(x => x.name != parameterToAdd.name)) {
				list.Add(parameterToAdd);
				return;
			}

			Int32 index = list.IndexOf(list.Where(x=>x.name==parameterToAdd.name).ElementAt(0));
			Parameter[] variables = list.ToArray();
			variables[index].value = parameterToAdd.value;
		}
	}
}