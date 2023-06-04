using RetroNet.Interfaces;

namespace RetroNet {
	public struct Parameter : IVariable {
		public Variable referenceVariable;

		public EToken type { get; set; }

		public String name { get; set; }

		public Object value {
			get {
				return this.referenceVariable.value;
			}
			set {
				this.referenceVariable.value = value;
			}
		}
	}
}