
using RetroNet.Interfaces;

namespace RetroNet {
	public class Variable : IVariable {
		public EToken type { get; set; }
		public String name { get; set; }
		public Object value { get; set; }
	}
}