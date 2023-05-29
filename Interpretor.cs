using System.Reflection;

namespace RetroNet {
	public class Interpretor {
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

		public void Interpret() {
			Int32 index = 0;
			foreach (Token token in this._tokens) {
				//TODO: Dodac kontext eg czy jestesmy w danej funkcji itd itp
				index++;
			}
		}

		[OperatorBinding(OperatorBinding = '=')]
		private void CreateVariable(Int32 index) {
			EToken type = this._tokens[index - 2].etoken;
			String name = this._tokens[index - 1].token;
			String value = this._tokens[index + 1].token;
			this._variables.Add(new Variable {
				type = type,
				name = name,
				value = value
			});
		}

		[FunctionBinding(functionName = "PRINT")]
		private void Print(int index) {
			Console.WriteLine(this._variables.Where(variable => variable.name == this._tokens[index + 2].token).ElementAt(0).value);
		}
	}
}