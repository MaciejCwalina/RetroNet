namespace RetroNet {
	//Trzeba zmenic nazwe z MathOperationAttribute na cos innego
	[AttributeUsage(AttributeTargets.Method)]
	public class OperatorBindingAttribute : Attribute {
		public Char OperatorBinding { get; set; }
	}
}