using System.Reflection.Metadata;
using RetroNet;

internal class Program {
	private static void Main() {
		Lexer lexer = new Lexer("../../../main.rn");
		List<Token> tokens = lexer.Tokenize();
		for (int i = 0; i < tokens.Count; i++) {
			if (tokens[i].etoken == EToken.FN) {
				
			}
		}
	}
}