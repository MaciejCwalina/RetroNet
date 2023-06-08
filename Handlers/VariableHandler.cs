using RetroNet.Interfaces;

namespace RetroNet.Handlers {
	public class VariableHandler {
		public Boolean GetVariable(Function function, String? name, out IVariable? variable) {
			try {
				if (function.localVariables.Count(x => x.name == name) > 0) {
					variable = function.localVariables.Where(x => x.name == name).ElementAt(0);
					return true;
				}
			}
			catch {
				variable = null;
				return false;
			}

			variable = null;
			return false;
		}

		[OperatorBinding(OperatorBinding = '=')]
		private void CreateVariable(ref Function function, Int32 index) {

		}
	}
}