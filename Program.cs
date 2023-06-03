using RetroNet;

class Program {
	private static void Main() {
		Lexer lexer = new Lexer(@"C:\Users\User\Desktop\Projekty\CS\main.rn");
		List<Token> tokens = lexer.Lex();
		Interpretor interpretor = new Interpretor(tokens);
		interpretor.InitFunctionsList();
		interpretor.RunMainFunction();
	}
}