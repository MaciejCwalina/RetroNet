namespace RetroNet {
	//Trzeba zmenic nazwe z MathOperationAttribute na cos innego
	[AttributeUsage(AttributeTargets.Method)]
	public class MathOperationAttribute : Attribute {
		public Char OperatorBinding { get; set; }
	}
}