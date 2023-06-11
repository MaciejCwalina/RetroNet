<<<<<<< HEAD
﻿using RetroNet.Interfaces;

namespace RetroNet.Handlers {
	public class StructHandler {
		private List<RetroStruct> _structs = new List<RetroStruct>();
		private List<Token> _tokens;
		public StructHandler(List<Token> tokens) {
			this._tokens = tokens;
		}

		public void CreateStruct(int index) {
			RetroStruct retroStruct = new RetroStruct {
				name = this._tokens[index+1].token,
				body = new List<Variable>()
			};

			index += 2;
			while (this._tokens[index].etoken != EToken.RBRACE) {
				if (TypesHelper.IsType(this._tokens[index].etoken)) {
					retroStruct.body.Add(new Variable {
						type = this._tokens[index].etoken,
						name = this._tokens[index+1].token,
					});

					index++;
				}

				index++;
			}

			this._structs.Add(retroStruct);
		}

		public Boolean TryGetStruct(out RetroStruct retroStruct, String? name) {
			try {
				retroStruct = new RetroStruct(this._structs.Where(x => x.name == name).ElementAt(0));
				return true;
			}
			catch (Exception e) {
				retroStruct = new RetroStruct();
				return false;
			}
		}
=======
﻿namespace RetroNet.Handlers {
	public class StructHandler {
		//TODO: IMPLEMENT STRUCTS!!!!
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
	}
}