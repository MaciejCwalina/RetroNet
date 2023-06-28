using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using RetroNet;

internal class Program {
	public static List<String> nameOfFunctions = new List<String>();

	private static void Main() {
		Lexer lexer = new Lexer("../../../main.rn");
		StreamWriter streamWriter;
		if (File.Exists(@"C:\Users\user1\Desktop\RetroNetMisc\ByteCode.rnbc")) {
			streamWriter = new StreamWriter(File.Open(@"C:\Users\user1\Desktop\RetroNetMisc\ByteCode.rnbc", FileMode.Create));
		}
		else {
			streamWriter = new StreamWriter(File.Create(@"C:\Users\user1\Desktop\RetroNetMisc\ByteCode.rnbc"));
		}

		List<Token> tokens = lexer.Tokenize();
		List<DefiningMethodContext> methodContexts = new List<DefiningMethodContext>();
		Context currentContext = null;
		for (int i = 0; i < tokens.Count; i++) {
			List<Token> contextBasedTokens = new List<Token>();
			if (currentContext is DefiningMethodContext definingMethodContext) {
				if (definingMethodContext.returnType == null) {
					definingMethodContext.returnType = tokens[i];
				}
				else if (definingMethodContext.methodName == null) {
					definingMethodContext.methodName = tokens[i];
					nameOfFunctions.Add(tokens[i].token);
					methodContexts.Add(definingMethodContext);
				}
				else {
					i += 2;
					if (tokens[i].etoken == EToken.EOL) {
						continue;
					}

					while (tokens[++i].etoken != EToken.RBRACE) {
						contextBasedTokens.Add(tokens[i]);
					}

					definingMethodContext.bodyOfContext = contextBasedTokens;
					currentContext = null;
				}
			}

			if (tokens[i].etoken == EToken.FN) {
				currentContext = new DefiningMethodContext(streamWriter);
			}
		}

		foreach (DefiningMethodContext definingMethodContext in methodContexts) {
			definingMethodContext.ConvertIntoByteCode();
		}

		streamWriter.Close();
	}
}