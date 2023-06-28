namespace RetroNet {
	public abstract class Context {
		protected StreamWriter _streamWriter;
		public Context(StreamWriter streamWriter) {
			this._streamWriter = streamWriter;
		}

		public abstract void ConvertIntoByteCode();
	}
}