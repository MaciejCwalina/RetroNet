using System.Runtime.InteropServices;
using System.Text;

namespace RetroNet {
	public class DefiningMethodContext : Context {
		public List<Token> bodyOfContext;
		public Token? methodName;
		public Token? returnType;

		public DefiningMethodContext(FileStream fileStream) : base(fileStream) {
		}

		public override void ConvertIntoByteCode() {
			using StreamWriter streamWriter = new StreamWriter(this._fileStream);
			streamWriter.WriteLine($"#method {this.returnType.Value.token} {this.methodName.Value.token} ()" + "{");
			streamWriter.WriteLine("#localVariables(");
			foreach (Variable variable in this.CreateAllVariables()) {
				streamWriter.WriteLine($"{variable.type} {variable.name.token}");
			}

			streamWriter.WriteLine(")");

			for (int i = 0; i < this.bodyOfContext.Count; i++) {
				if (TypesHelper.IsType(this.bodyOfContext[i].etoken)) {
					while (this.bodyOfContext[i++].etoken != EToken.EOL) {
						if (this.bodyOfContext[i].etoken == EToken.PERIOD) {
							//add handling for whenever we are calling a property from a struct ! :)
							continue;
						}

						streamWriter.WriteLine($"movValue {this.bodyOfContext[i].token} {this.bodyOfContext[i + 2].token}");
						break;
					}
				}
			}

			streamWriter.WriteLine("}");
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