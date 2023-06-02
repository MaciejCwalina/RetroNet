namespace RetroNet {
	public enum EToken{
		EQUALS,
		PLUS,
		MINUS,
		I32,
		F32,
		STRING,
		FN,
		RETURN,
		UNDEFINED,
		LBRACE,
		RBRACE,
		LPAR,
		RPAR,
		MAIN,
		WHITESPACE,
		EOL,
		PRINT,
		//New tokens added, not sure if they're supposed to appear right after each other of if we have some kind of convention regarding their placement.
		IF,
		ISEQUAL, // Comparison, not assignment
		GREATERTHAN, //Other lambda bullshit
		GREATEREQUAL,
		LESSTHAN,
		LESSEQUAL,
		//Should these even be tokens ? I mean they are more than 1 char long and it's a problem for the switch in Lexer.cs (I might have a retarded fix for that tho).
	}
}