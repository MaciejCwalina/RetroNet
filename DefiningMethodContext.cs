using System.Runtime.InteropServices;
using System.Text;

namespace RetroNet {
	public class DefiningMethodContext : Context {
		public List<Token> bodyOfContext;
		public Token? methodName;
		public Token? returnType;
		public DefiningMethodContext(StreamWriter streamWriter) : base(streamWriter) {
		}

		public override void ConvertIntoByteCode() {
			this._streamWriter.WriteLine($"#method {this.returnType.Value.token} {this.methodName.Value.token} () " + "{");
			this._streamWriter.WriteLine("#localVariables (");
			foreach (Variable variable in this.CreateAllVariables()) {
				this._streamWriter.WriteLine($"{variable.type} {variable.name.token}");
			}

			this._streamWriter.WriteLine(")");

			for (int i = 0; i < this.bodyOfContext.Count; i++) {
				if (TypesHelper.IsType(this.bodyOfContext[i].etoken)) {
					while (this.bodyOfContext[++i].etoken != EToken.EOL) {
						if (this.bodyOfContext[i].etoken == EToken.EQUALS) {
							this._streamWriter.WriteLine($"movValue {this.bodyOfContext[i-1].token} {this.bodyOfContext[i + 1].token};");
						}
					}
				}

				if (Program.nameOfFunctions.Contains(this.bodyOfContext[i].token)) {
					this._streamWriter.WriteLine($"call {this.bodyOfContext[i].token} ();");
				}
			}

			this._streamWriter.WriteLine("}");
			this._streamWriter.WriteLine("");
		}

		private List<Variable> CreateAllVariables() {
			List<Variable> variables = new List<Variable>();
			for (int i = 0; i < this.bodyOfContext.Count; i++) {
				if (!TypesHelper.IsType(this.bodyOfContext[i].etoken)) {
					continue;
				}

				variables.Add(new Variable {
					type = this.bodyOfContext[i].etoken,
					name = this.bodyOfContext[i + 1]
				});
			}

			return variables;
		}
	}
}