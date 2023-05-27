namespace RetroNet {
	[AttributeUsage(AttributeTargets.Method)]
	public class MathOperationAttirbute : Attribute {
		public Char OperatorBinding { get; set; }
	}
}