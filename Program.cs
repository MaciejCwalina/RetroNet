using System.Reflection;
using RetroNet;
<<<<<<< HEAD
using RetroNet.Handlers;

struct MyStruct {
	public int x;
}

=======

>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
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
