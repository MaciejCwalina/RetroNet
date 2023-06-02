using System.Reflection;

namespace RetroNet {
	public class Interpretor {
		private List<Function> _functions = new List<Function>();
		private List<Token> _tokens;
		private List<Variable> _variables = new List<Variable>();

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

		public void InitFunctionsList() {
			for (Int32 i = 0; i < this._tokens.Count; i++) {
				if (this._tokens[i].etoken == EToken.FN) {
					this.CreateFunction(i);
				}
			}
		}

		public void RunMainFunction() {
			Int32 index = 0;

			for (Int32 i = 0; i < this._functions[0].body.Count; i++) {
				if (Char.IsSymbol(this._functions[0].body[i].token[0])) {
					IEnumerable<MethodInfo> methods = typeof(Interpretor).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(methodInfo => {
						if (methodInfo.CustomAttributes.Count() == 1) {
							return methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding == this._functions[0].body[i].token[0];
						}

						return false;
					});

					methods.ElementAt(0).Invoke(this, new Object?[] {
						this._functions[0], i
					});
				}

				if (this._functions[0].body[i].etoken == EToken.PRINT) {
					this.Print(this._functions[0], i);
				}

				if (this.TryGetFunction(this._functions[0].body, i, out Function function)) {
					this.RunFunction(function);
				}
			}
		}

		private Boolean TryGetFunction(List<Token> body, Int32 index, out Function function) {
			try {
				function = this._functions.Where(function => function.name == body[index].token).ElementAt(0);
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
					IEnumerable<MethodInfo> methods = typeof(Interpretor).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(methodInfo => {
						if (methodInfo.CustomAttributes.Count() == 1) {
							return methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding == token.token[0];
						}

						return false;
					});

					methods.ElementAt(0).Invoke(this, new Object[] {
						function, index
					});
				}

				if (token.etoken == EToken.PRINT) {
					this.Print(function, index);
				}

				if (this.TryGetFunction(function.body, index, out Function func)) {
					this.RunFunction(func);
				}

				index++;
			}
		}

		[OperatorBinding(OperatorBinding = '=')]
		private void CreateVariable(Function function, Int32 index) {
			//TODO: Re-write this shouldn't use indexing for determining the variable.
			
			EToken type = function.body[index - 2].etoken;
			String name = function.body[index - 1].token;
			String value = function.body[index + 1].token;
			/* Got this ready to implement the new solution, gotta figure out what it is though.
			EToken type = 
			String name =
			String value = 
			*/
			this._variables.Add(new Variable {
				type = type,
				name = name,
				value = value
			});
		}

		private void CreateFunction(Int32 index) {
			index++;
			Function function = new Function {
				returnType = this._tokens[index].etoken,
				name = this._tokens[index + 1].token
			};

			List<Token> body = new List<Token>();
			index += 5;
			while (this._tokens[index].etoken != EToken.RBRACE) {
				body.Add(this._tokens[index]);
				index++;
			}

			function.body = body;
			this._functions.Add(function);
		}

		private void Print(Function function, Int32 index) {
			//This is fucking trash re-write this.
			Console.WriteLine(this._variables.Where(variable => variable.name == function.body[index + 2].token).ElementAt(0).value);
		}
	}
}