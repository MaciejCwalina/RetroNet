using RetroNet.Interfaces;

namespace RetroNet.Handlers {
	public class LoopHandler {
		private List<WhileLoop> _loops = new List<WhileLoop>();
		private VariableHandler _variableHandler;
		private List<Token> _tokens;

		public LoopHandler(VariableHandler variableHandler, List<Token> tokens) {
			this._variableHandler = variableHandler;
			this._tokens = tokens;
		}

		public void CreateLoop(int index) {
			WhileLoop whileLoop = new WhileLoop {condition = new List<Token>(), body = new List<Token>()};
			index += 2;
			while (this._tokens[index].etoken != EToken.RPAR) {
				whileLoop.condition.Add(this._tokens[index]);
				index++;
			}

			index++;
			while (this._tokens[index].etoken != EToken.LBRACE) {
				whileLoop.body.Add(this._tokens[index]);
			}

			this._loops.Add(whileLoop);
		}

		public void RunLoop(WhileLoop loop) {
			while (IsConditonTrue(loop.condition)) {

			}
		}

		private Boolean IsConditonTrue(List<Token> condition) {
			this._variableHandler.try
			return true;
		}
	}
}