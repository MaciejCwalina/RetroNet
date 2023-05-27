using System.Reflection;

namespace RetroNet {
	public class Interpretor {
		private List<Token> _tokens;
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
							return methodInfo.GetCustomAttribute<MathOperationAttirbute>().OperatorBinding == token.token[0];
						}

						return false;
					});

					methods.ElementAt(0).Invoke(this, new Object?[] {
					});
				}
			}
		}

		[MathOperationAttirbute(OperatorBinding = '=')]
		private void CreateVariable() {
			Console.WriteLine("hello world");
		}
	}
}