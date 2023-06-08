using System.Reflection;
using RetroNet;

internal class Program {
	private static void Main() {
		Lexer lexer = new Lexer(@"C:\Users\user1\RiderProjects\RetroNet\RetroNet\main.rn");
		List<Token> tokens = lexer.Lex();
		Interpretor interpretor = new Interpretor(SanitazeTokens(tokens));
		interpretor.LoadIntoMemory();
	}

	private static List<Token> SanitazeTokens(List<Token> tokens) {
		List<Token> result = new List<Token>();
		foreach (Token token in tokens) {
			if (token.etoken == EToken.WHITESPACE) {
				continue;
			}

			result.Add(token);
		}

		return result;
	}
}
