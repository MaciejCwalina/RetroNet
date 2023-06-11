using System.Reflection;
<<<<<<< HEAD
using System.Runtime.InteropServices;
using RetroNet.ExtensionMethods;
using RetroNet.Interfaces;
=======
using RetroNet.ExtensionMethods;
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9

namespace RetroNet.Handlers {
	public class FunctionHandler {
		private List<Function> _functions = new List<Function>();
		private List<Token> _tokens;
		private VariableHandler _variableHandler;

<<<<<<< HEAD
		public FunctionHandler(List<Token> tokens, VariableHandler variableHandler) {
			this._tokens = tokens;
			this._variableHandler = variableHandler;
=======
		public FunctionHandler(List<Token> tokens) {
			this._tokens = tokens;
			this._variableHandler = new VariableHandler();
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
		}

		public void CreateFunction(Int32 index) {
			index++;
			Function function = new Function {
				returnType = this._tokens[index].etoken,
				name = this._tokens[index + 1].token
			};

			Int32 secondIndex = index + 2;
			function.parameters = new List<Parameter>();
			while (this._tokens[secondIndex].etoken != EToken.RPAR) {
<<<<<<< HEAD
				bool customType = false;
				if (TypesHelper.IsType(this._tokens[secondIndex].etoken) || (customType = this._variableHandler.IsCustomType(this._tokens[secondIndex].token))) {
					function.parameters.AddParameter(new Parameter {
						type = !customType ? this._tokens[secondIndex].etoken : EToken.CUSTOMTYPE,
=======
				if (TypesHelper.IsType(this._tokens[secondIndex].etoken)) {
					function.parameters.AddParameter(new Parameter {
						type = this._tokens[secondIndex].etoken,
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
						name = this._tokens[secondIndex + 1].token
					});

					secondIndex++;
				}

				secondIndex++;
			}

			List<Token> body = new List<Token>();
			index += 5;
			while (this._tokens[index].etoken != EToken.RBRACE) {
				body.Add(this._tokens[index]);
				index++;
			}

			function.body = body;
			this._functions.Add(function);
		}

		public void RunFunction(ref Function function) {
			Int32 index = 0;
			foreach (Token token in function.body) {
				if (Char.IsSymbol(token.token[0])) {
					IEnumerable<MethodInfo> methods = this._variableHandler.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
														  .Where(methodInfo => methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding ==
																		       token.token[0]);

<<<<<<< HEAD
					methods.ElementAt(0).Invoke(this._variableHandler, new Object[] {
						function, index, this
					});
				}

				if (token.etoken == EToken.PRINT) {
					Function? functionCopy;
					if (function.body[index + 3].etoken == EToken.PERIOD) {
						if (!this._variableHandler.TryGetVariable(ref function, function.body[index + 2].token, out IVariable variable)) {
							throw new Exception($"Variable: {function.body[index + 2].token}, doesnt exist");
						}

						List<Variable> variables = (List<Variable>)variable.value;
						functionCopy = function;
						Interpretor.Print(variables.Where(x => x.name == functionCopy.body[index + 4].token).ElementAt(0));

						return;
					}

					functionCopy = function;

=======
					methods.ElementAt(0).Invoke(this._variableHandler, new Object[] {function, index,this});
				}

				if (token.etoken == EToken.PRINT) {
					Function? functionCopy = function;
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
					Interpretor.Print(function.localVariables.Where(x => x.name == functionCopy.body[index + 2].token).ElementAt(0));
				}

				if (this.TryGetFunction(function, index, out Function func)) {
					this.RunFunction(ref func);
				}

				if (token.etoken == EToken.RETURN) {
					this.ReturnFromMethod(ref function, index);
					return;
				}

				index++;
			}
		}

		public Boolean TryGetFunction(Function functionCaller, Int32 index, out Function function) {
			try {
<<<<<<< HEAD
				function = this._functions.Where(func => func.name == functionCaller.body[index].token).ElementAt(0);
				index++;
				Int32 paramIndex = 0;
				for (Int32 i = index + 1; i < functionCaller.body.Count; i++) {
					Parameter[] parameters;
					if (functionCaller.body[index + 2].etoken == EToken.PERIOD) {
						if (!this._variableHandler.TryGetVariable(ref functionCaller, functionCaller.body[index + 1].token, out IVariable variable)) {
							throw new Exception("This does not exist");
						}

						List<Variable> arguments = (List<Variable>)variable.value;
						parameters = function.parameters.ToArray();
						foreach (Parameter parameter in parameters) {
							parameter.referenceVariable = arguments.Where(x => x.name == functionCaller.body[index + 3].token).ElementAt(0);
							index++;
						}


						foreach (Parameter parameter in parameters) {
							function.localVariables.AddIVariable(parameter);
						}

						break;
					}

=======
				function = this._functions.Where(function => function.name == functionCaller.body[index].token).ElementAt(0);
				index++;
				Int32 paramIndex = 0;
				for (Int32 i = index + 1; i < functionCaller.body.Count; i++) {
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
					if (functionCaller.body[i].etoken == EToken.RPAR) {
						break;
					}

<<<<<<< HEAD
					parameters = function.parameters.ToArray();
=======
					Parameter[] parameters = function.parameters.ToArray();
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
					parameters[paramIndex].referenceVariable = (Variable)functionCaller.localVariables
																					   .Where(x => x.name == functionCaller.body[i].token).ElementAt(0);
					foreach (Parameter parameter in parameters) {
						function.localVariables.AddIVariable(parameter);
					}

					paramIndex++;
				}

				return true;
			}
			catch {
				function = new Function();
				return false;
			}
		}

		public Boolean TryGetFunction(String? name, out Function? function) {
			try {
				function = this._functions.Where(x => x.name == name).ElementAt(0);
				return true;
			}
			catch {
				function = null;
				return false;
			}
		}

		private void ReturnFromMethod(ref Function function, Int32 index) {
			function.returnValue = function.body[index + 1].token;
		}
	}
}