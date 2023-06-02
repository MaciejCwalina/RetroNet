namespace RetroNet {
	[AttributeUsage(AttributeTargets.Method)]
	public class FunctionBindingAttribute : Attribute{
		public String functionName { get; set; }
	}
}