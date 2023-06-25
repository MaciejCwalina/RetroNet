namespace RetroNet {
	public abstract  class Context {
		protected FileStream _fileStream;
		public Context(FileStream fileStream) {
			this._fileStream = fileStream;
		}

		public abstract void ConvertIntoByteCode();
	}
}