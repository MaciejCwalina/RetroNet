using System.Runtime.InteropServices;
using RetroNet.ExtensionMethods;
using RetroNet.Interfaces;

namespace RetroNet.Handlers {
	public class VariableHandler {
		private StructHandler _structHandler;

		public VariableHandler(StructHandler structHandler) {
			this._structHandler = structHandler;
		}

		public Boolean TryGetVariable(ref Function function, String? name, out IVariable? variable) {
			try {
				variable = function.localVariables.Where(x => x.name == name).ElementAt(0);
				return true;
			}
			catch {
				variable = null;
				return false;
			}
		}

		public Boolean IsCustomType(String? name) {
			return this._structHandler.TryGetStruct(out RetroStruct placeHolder, name);
		}

		[OperatorBinding(OperatorBinding = '=')]
		private void CreateVariable(ref Function functionCaller, Int32 index, FunctionHandler handler) {
			if (functionCaller.body[index - 2].etoken == EToken.PERIOD) {
				if (!this.TryGetVariable(ref functionCaller, functionCaller.body[index - 3].token, out IVariable structVariable)) {
					throw new Exception($"Variable with the name {functionCaller.body[index - 3].token} does not exist");
				}

				List<Variable> variables = (List<Variable>)structVariable.value;
				Function? caller = functionCaller;
				Variable structProperty = variables.Where(x => x.name == caller.body[index - 1].token).ElementAt(0);
				Variable variable = new Variable(structProperty);
				int indexOf = variables.IndexOf(structProperty);
				variable.value = variable.type == EToken.STRING ? functionCaller.body[index + 2].token : functionCaller.body[index + 1].token;
				variables[indexOf] = variable;
				return;
			}

			if (functionCaller.body[index + 1].etoken == EToken.NEW) {
				Variable variable = new Variable {
					name = functionCaller.body[index - 1].token,
					type = EToken.CUSTOMTYPE
				};

				if (!this._structHandler.TryGetStruct(out RetroStruct retroStruct, functionCaller.body[index - 2].token)) {
					throw new Exception($"Struct with the name {functionCaller.body[index - 1].token} does not exist");
				}

				variable.value = new List<Variable>(retroStruct.body);
				functionCaller.localVariables.Add(variable);
				return;
			}

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

			if (this.TryGetVariable(ref functionCaller, functionCaller.body[index - 1].token, out IVariable? variableToAssign)) {
				Int32 indexOf;
				IVariable[] variables;
				if (this.TryGetVariable(ref functionCaller, functionCaller.body[index + 1].token, out IVariable? variable)) {
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