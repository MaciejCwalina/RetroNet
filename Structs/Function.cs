using RetroNet.Interfaces;

namespace RetroNet {
	public struct Function {
		public EToken returnType;
		public String name;
		public List<Token> body;
		public List<IVariable> localVariables = new List<IVariable>();
		public List<Parameter> parameters = new List<Parameter>();
		public Function() {
		}
	}
}