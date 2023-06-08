using RetroNet.ExtensionMethods;
using RetroNet.Interfaces;

namespace RetroNet.Handlers {
	public class VariableHandler {
		public Boolean GetVariable(ref Function function, String? name, out IVariable? variable) {
			try {
				variable = function.localVariables.Where(x => x.name == name).ElementAt(0);
				return true;
			}
			catch {
				variable = null;
				return false;
			}
		}

		[OperatorBinding(OperatorBinding = '=')]
		private void CreateVariable(ref Function functionCaller, Int32 index, FunctionHandler handler) {
			EToken type = functionCaller.body[index - 2].etoken;
			functionCaller.localVariables.AddIVariable(new Variable {
				name = functionCaller.body[index - 1].token,
				type = type,
				value = type == EToken.STRING ? functionCaller.body[index + 2].token : functionCaller.body[index + 1].token
			});

			if (handler.TryGetFunction(functionCaller.body[index + 1].token, out Function function)) {
				handler.RunFunction(ref function);
				functionCaller.localVariables.AddIVariable(new Variable {
					name = functionCaller.body[index - 1].token,
					type = type,
					value = function.returnValue
				});

				return;
			}

			if (this.GetVariable(ref functionCaller, functionCaller.body[index - 1].token, out IVariable? variableToAssign)) {
				Int32 indexOf;
				IVariable[] variables;
				if (this.GetVariable(ref functionCaller, functionCaller.body[index + 1].token, out IVariable? variable)) {
					variables = functionCaller.localVariables.ToArray();
					indexOf = Array.IndexOf(variables, variableToAssign);
					variables[indexOf].value = variable.value;
					functionCaller.localVariables = variables.ToList();
					return;
				}

				variables = functionCaller.localVariables.ToArray();
				indexOf = Array.IndexOf(variables, variableToAssign);
				variables[indexOf].value =
					variables[indexOf].type == EToken.STRING ? functionCaller.body[index + 2].token : functionCaller.body[index + 1].token;
				functionCaller.localVariables = variables.ToList();
			}
		}
	}
}