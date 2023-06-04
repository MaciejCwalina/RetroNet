namespace RetroNet.Interfaces {
	public interface IVariable {
		public EToken type { get; set; }
		public String name { get; set; }
		public Object value { get; set; }
	}
}