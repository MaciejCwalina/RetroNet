using RetroNet;

internal class Program {
	private static void Main() {
		Lexer lexer = new Lexer(@"C:\Users\Windows\Documents\GitHub\RetroNet\main.rn");
		List<Token> tokens = lexer.Lex();
		Interpretor interpretor = new Interpretor(tokens);
		interpretor.LoadIntoMemory();
		interpretor.RunMainFunction();
	}
}