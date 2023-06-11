using System.Runtime.InteropServices;
using RetroNet.Interfaces;

namespace RetroNet {
	public class Variable : IVariable {

		public EToken type { get; set; }
		public String? name { get; set; }
		public Object? value { get; set; }

		public Variable(Variable variable) {
			this.type = variable.type;
			this.name = variable.name;
			this.value = variable.value;
		}

		public Variable() {

		}
	}
}