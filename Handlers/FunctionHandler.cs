using System.Reflection;

namespace RetroNet.Handlers {
	public class FunctionHandler {
		private List<Function> _functions = new List<Function>();
		private List<Token> _tokens;
		private VariableHandler _variableHandler;

		public FunctionHandler(List<Token> tokens) {
			this._tokens = tokens;
			this._variableHandler = new VariableHandler();
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
				if (TypesHelper.IsType(this._tokens[secondIndex].etoken)) {
					function.parameters.Add(new Parameter {
						type = this._tokens[secondIndex].etoken,
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

		public void RunMainFunction() {
			for (Int32 i = 0; i < this._functions[0].body.Count; i++) {
				if (Char.IsSymbol(this._functions[0].body[i].token[0])) {
					IEnumerable<MethodInfo> methods = this._variableHandler.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
														  .Where(methodInfo => methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding ==
																		       this._functions[0].body[i].token[0]);
					methods.ElementAt(0).Invoke(this._variableHandler, new Object?[] {
						this._functions[0], i
					});
				}

				if (this._functions[0].body[i].etoken == EToken.PRINT) {
					Interpretor.Print(this._functions[0].localVariables.Where(x => x.name == this._functions[0].body[i + 2].token).ElementAt(0));
				}

				if (this.TryGetFunction(this._functions[0], i, out Function function)) {
					this.RunFunction(function);
				}
			}
		}

		public void RunFunction(Function function) {
			Int32 index = 0;
			foreach (Token token in function.body) {
				if (Char.IsSymbol(token.token[0])) {
					IEnumerable<MethodInfo> methods = typeof(VariableHandler).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(methodInfo => {
						if (methodInfo.CustomAttributes.Count() == 1) {
							return methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding == token.token[0];
						}

						return false;
					});

					methods.ElementAt(0).Invoke(typeof(VariableHandler), new Object[] {
						function, index
					});
				}

				if (token.etoken == EToken.PRINT) {
					Interpretor.Print(function.localVariables.Where(x => x.name == this._functions[0].body[index + 2].token).ElementAt(0));
				}

				if (this.TryGetFunction(function, index, out Function func)) {
					this.RunFunction(func);
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
				function = this._functions.Where(function => function.name == functionCaller.body[index].token).ElementAt(0);
				index++;
				Int32 paramIndex = 0;
				for (Int32 i = index + 1; i < functionCaller.body.Count; i++) {
					if (functionCaller.body[i].etoken == EToken.RPAR) {
						break;
					}

					Parameter[] parameters = function.parameters.ToArray();
					parameters[paramIndex].referenceVariable = (Variable)functionCaller.localVariables
																					   .Where(x => x.name == functionCaller.body[i].token).ElementAt(0);
					foreach (Parameter parameter in parameters) {
						function.localVariables.Add(parameter);
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