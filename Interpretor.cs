using System.Reflection;
using RetroNet.Handlers;
using RetroNet.Interfaces;

namespace RetroNet {
	public class Interpretor {
		private List<Token> _tokens;
		private FunctionHandler _functionHandler;
		public Interpretor(List<Token> tokens) {
			this._tokens = tokens;
			this._functionHandler = new FunctionHandler(this._tokens);
		}

		public void LoadIntoMemory() {
			for (Int32 i = 0; i < this._tokens.Count; i++) {
				if (this._tokens[i].etoken == EToken.FN) {
					this._functionHandler.CreateFunction(i);
				}

				/*if (this._tokens[i].etoken == EToken.STRUCT) {
					this.CreateStruct(i);
				}*/
			}

			if (!this._functionHandler.TryGetFunction("main",out Function? func)) {
				throw new Exception($"Function Main was NULL, please create a main function");
			}

			this._functionHandler.RunFunction(ref func!);
		}

		public static void Print(IVariable variable) {
			Console.WriteLine(variable.value);
		}
	}
}