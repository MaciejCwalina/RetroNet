using System.Reflection;
using RetroNet.Handlers;
using RetroNet.Interfaces;

namespace RetroNet {
	public class Interpretor {
		private List<Token> _tokens;
		private FunctionHandler _functionHandler;
		private StructHandler _structHandler;
		private LoopHandler _loopHandler;
		private VariableHandler _variableHandler;
		public Interpretor(List<Token> tokens) {
			this._tokens = tokens;
			this._structHandler = new StructHandler(tokens);
			this._variableHandler = new VariableHandler(this._structHandler);
			this._loopHandler = new LoopHandler(this._variableHandler, tokens);
			this._functionHandler = new FunctionHandler(this._tokens,this._variableHandler,this._loopHandler);
			//this._ifStatementHandler = new IfStatementHandler(this._tokens);

		}

		public void LoadIntoMemory() {
			for (Int32 i = 0; i < this._tokens.Count; i++) {
				if (this._tokens[i].etoken == EToken.FN) {
					this._functionHandler.CreateFunction(i);
				}
				/*
                if (this._tokens[i].etoken == EToken.IF) {
                    this._ifStatementHandler.CreateIfStatement(i);
                }

				*/
                if (this._tokens[i].etoken == EToken.STRUCT) {
					this._structHandler.CreateStruct(i);
				}

				if (this._tokens[i].etoken == EToken.FOR || this._tokens[i].etoken == EToken.WHILE) {
					this._loopHandler.CreateLoop(i);
				}
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