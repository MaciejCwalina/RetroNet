using System.Reflection.Metadata;
using RetroNet;

internal class Program {
	private static void Main() {
		Lexer lexer = new Lexer("../../../main.rn");
		FileStream fileStream;
		if (File.Exists(@"C:\Users\user1\Desktop\RetroNetMisc\ByteCode.rnbc")) {
			fileStream = File.Open(@"C:\Users\user1\Desktop\RetroNetMisc\ByteCode.rnbc", FileMode.Create);
		}
		else {
			fileStream = File.Create(@"C:\Users\user1\Desktop\RetroNetMisc\ByteCode.rnbc");
		}

		List<Token> tokens = lexer.Tokenize();
		Context currentContext = null;
		for (int i = 0; i < tokens.Count; i++) {
			List<Token> contextBasedTokens = new List<Token>();
			if (currentContext is DefiningMethodContext definingMethodContext) {
				if (definingMethodContext.returnType == null) {
					definingMethodContext.returnType = tokens[i];
				}
				else if (definingMethodContext.methodName == null) {
					definingMethodContext.methodName = tokens[i];
				}
				else {
					i += 2;
					while (tokens[++i].etoken != EToken.RBRACE) {
						contextBasedTokens.Add(tokens[i]);
					}

					definingMethodContext.bodyOfContext = contextBasedTokens;
					definingMethodContext.ConvertIntoByteCode();
					currentContext = null;
				}
			}

			if (tokens[i].etoken == EToken.FN) {
				currentContext = new DefiningMethodContext(fileStream);
			}
		}
	}
}