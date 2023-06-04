﻿using RetroNet.Interfaces;

namespace RetroNet {
	public struct Variable : IVariable {
		public EToken type { get; set; }
		public String name { get; set; }
		public Object value { get; set; }
	}
}