using System.Reflection;

namespace RetroNet {
	public class Interpretor {
		private List<Token> _tokens;

		private List<Variable> _variables = new List<Variable>();

		public Interpretor(List<Token> tokens) {
			this._tokens = tokens;
		}

		public void Interpret() {
			Int32 index = 0;
			foreach (Token token in this._tokens) {
				if (Char.IsSymbol(token.token[0])) {
					IEnumerable<MethodInfo> methods = typeof(Interpretor).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(methodInfo => {
						//Chujowe rozwiazane poszukam lepszego jak wstane
						//Nie chce mi sie szukac lepszego rozwiazania.
						if (methodInfo.CustomAttributes.Count() == 1) {
							return methodInfo.GetCustomAttribute<OperatorBindingAttribute>()?.OperatorBinding == token.token[0];
						}

						return false;
					});

					methods.ElementAt(0).Invoke(this, new Object?[] {
						index
					});
				}

				index++;
			}
		}

		[OperatorBinding(OperatorBinding = '=')]
		private void CreateVariable(Int32 index) {
			EToken type = this._tokens[index - 2].etoken;
			String name = this._tokens[index - 1].token;
			String value = this._tokens[index + 1].token;
			//TODO: Poprawić kod w lekserze żeby automatycznie usuwał ";" i dodawał jako End of Line (EOL) do listy tokenów
			this._variables.Add(new Variable {
				type = type,
				name = name,
				value = value
			});
		}
	}
}