using RetroNet.Interfaces;

namespace RetroNet {
	public class Function {
		public EToken returnType;
		public String? name;
		public List<Token> body;
		public List<IVariable> localVariables = new List<IVariable>();
		public List<Parameter> parameters;
		public Object? returnValue;
		public Function() {
		}
	}
}