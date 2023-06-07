using System.Reflection;
using RetroNet.Interfaces;

namespace RetroNet {
	public class Interpretor {
		private List<Function> _functions = new List<Function>();
		private List<Struct> _structs = new List<Struct>();
		private List<Token> _tokens;

		public Interpretor(List<Token> tokens) {
			this._tokens = this.SanitazeTokens(tokens);
		}

		private List<Token> SanitazeTokens(List<Token> tokens) {
			List<Token> result = new List<Token>();
			foreach (Token token in tokens) {
				if (token.etoken == EToken.WHITESPACE) {
					continue;
				}

				result.Add(token);
			}

			return result;
		}

		public void LoadIntoMemory() {
			for (Int32 i = 0; i < this._tokens.Count; i++) {
				if (this._tokens[i].etoken == EToken.FN) {
					this.CreateFunction(i);
				}

				if (this._tokens[i].etoken == EToken.STRUCT) {
					this.CreateStruct(i);
				}
			}
		}

		public void RunMainFunction() {
			for (Int32 i = 0; i < this._functions[0].body.Count; i++) {
				if (Char.IsSymbol(this._functions[0].body[i].token[0])) {
					IEnumerable<MethodInfo> methods = typeof(Interpretor)
						.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(methodInfo => {
							if (methodInfo.CustomAttributes.Count() == 1) {
								return methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding ==
									   this._functions[0].body[i].token[0];
							}

							return false;
						});

					methods.ElementAt(0).Invoke(this, new Object?[] {this._functions[0], i});
				}

				if (this._functions[0].body[i].etoken == EToken.PRINT) {
					this.Print(this._functions[0].localVariables
						.Where(x => x.name == this._functions[0].body[i + 2].token).ElementAt(0));
				}

				if (this.TryGetFunction(this._functions[0], i, out Function function)) {
					this.RunFunction(function);
				}
			}
		}

		private Boolean TryGetFunction(String name, out Function? function) {
			try {
				function = this._functions.Where(x => x.name == name).ElementAt(0);
				return true;
			}
			catch {
				function = null;
				return false;
			}
		}

		private Boolean TryGetFunction(Function functionCaller, Int32 index, out Function function) {
			try {
				function = this._functions.Where(function => function.name == functionCaller.body[index].token)
					.ElementAt(0);
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


		private void RunFunction(Function function) {
			Int32 index = 0;
			foreach (Token token in function.body) {
				if (Char.IsSymbol(token.token[0])) {
					IEnumerable<MethodInfo> methods = typeof(Interpretor)
						.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(methodInfo => {
							if (methodInfo.CustomAttributes.Count() == 1) {
								return methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding ==
									   token.token[0];
							}

							return false;
						});

					methods.ElementAt(0).Invoke(this, new Object[] {function, index});
				}

				if (token.etoken == EToken.PRINT) {
					this.Print(
						function.localVariables.Where(x => x.name == function.body[index + 2].token).ElementAt(0));
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

		private void ReturnFromMethod(ref Function function, Int32 index) {
			function.returnValue = function.body[index + 1].token;
		}

		private Boolean GetVariable(Function function, String name, out IVariable? variable) {
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
			EToken type;
			if (!this.GetVariable(function, function.body[index - 1].token, out IVariable? variable)) {
				String name = function.body[index + 1].token;
				String value;
				if (TryGetFunction(name, out Function refFunction)) {
					value = refFunction.returnValue.ToString();
				}

				type = function.body[index - 2].etoken;
				name = function.body[index - 1].token;
				value = type == EToken.STRING ? function.body[index + 2].token : function.body[index + 1].token;

				function.localVariables.Add(new Variable {type = type, name = name, value = value});

				return;
			}


			String token = function.body[index - 1].token;
			type = function.body[index - 2].etoken;
			function.localVariables[
					function.localVariables.IndexOf(function.localVariables.Where(x => x.name == token).ElementAt(0))]
				.value = type == EToken.STRING ? function.body[index + 2].token : function.body[index + 1].token;
		}

		private void CreateFunction(Int32 index) {
			index++;
			Function function = new Function {
				returnType = this._tokens[index].etoken, name = this._tokens[index + 1].token
			};

			Int32 secondIndex = index + 2;
			function.parameters = new List<Parameter>();
			while (this._tokens[secondIndex].etoken != EToken.RPAR) {
				if (TypesHelper.IsType(this._tokens[secondIndex].etoken)) {
					function.parameters.Add(new Parameter {
						type = this._tokens[secondIndex].etoken, name = this._tokens[secondIndex + 1].token
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

		private void CreateStruct(Int32 index) {
			Struct RetroStruct = new Struct {name = this._tokens[index + 1].token, body = new List<Variable>()};

			index += 3;
			while (this._tokens[index].etoken != EToken.RBRACE) {
				if (TypesHelper.IsType(this._tokens[index].etoken)) {
					RetroStruct.body.Add(new Variable {
						type = this._tokens[index].etoken, name = this._tokens[index + 1].token
					});

					index++;
				}

				index++;
			}

			RetroStruct.hasCtor = false;
			this._structs.Add(RetroStruct);
		}

		private void Print(IVariable variable) {
			Console.WriteLine(variable.value);
		}
	}
}