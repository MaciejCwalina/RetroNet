using System.Reflection;

namespace RetroNet {
	public class Interpretor {
		private List<Token> _tokens;
		//Zmenic na liste typu Variable albo najlepiej jakis obiekt co ma getter i setter na liste i patrzec ile pamieci wpierdalamy.
		private Dictionary<String, Object> _variables = new Dictionary<String, Object>();

		public Interpretor(List<Token> tokens) {
			this._tokens = tokens;
		}

		public void Interpret() {
			foreach (Token token in this._tokens) {
				if (Char.IsSymbol(token.token[0])) {
					IEnumerable<MethodInfo> methods = typeof(Interpretor).GetMethods(BindingFlags.NonPublic| BindingFlags.Instance).Where(methodInfo => {
						//Chujowe rozwiazane poszukam lepszego jak wstan
						if (methodInfo.CustomAttributes.Count() == 1) {
							return methodInfo.GetCustomAttribute<MathOperationAttribute>().OperatorBinding == token.token[0];
						}

						return false;
					});

					//Wezwac z parametrami
					methods.ElementAt(0).Invoke(this, new Object?[] {
					});
				}
			}
		}

		//Dodac parametry
		[MathOperation(OperatorBinding = '=')]
		private void CreateVariable() {
			Console.WriteLine("hello world");
		}
	}
}