namespace RetroNet {
	[AttributeUsage(AttributeTargets.Method)]
	public class OperatorBindingAttribute : Attribute {
		public Char OperatorBinding { get; set; }
	}
}