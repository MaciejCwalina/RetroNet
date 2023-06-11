namespace RetroNet {
	public struct RetroStruct  {
		public List<Variable> body;
		public Boolean hasCtor;
		public String? name;

		public RetroStruct(RetroStruct retroStruct) {
			this.body = new List<Variable>(retroStruct.body);
			this.hasCtor = retroStruct.hasCtor;
			this.name = retroStruct.name;
		}
	}
}