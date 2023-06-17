namespace RetroNet {
	public class Lexer {
		private String[] _file;
		private HashSet<String> _namesOfStructs = new HashSet<String>();

		public Lexer(String pathToFile) {
			this._file = File.ReadAllLines(pathToFile);
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

		public List<Token> Tokenize() {
			List<Token> tokens = new List<Token>();
			foreach (String line in this._file) {
				String placeholder = String.Empty;
				foreach (Char c in line) {
					if (c == ' ' || this.DetermineToken(c.ToString()).etoken != EToken.UNDEFINED) {
						Token token = this.DetermineToken(placeholder);
						tokens.Add(token);
						token = this.DetermineToken(c.ToString());
						tokens.Add(token);
						placeholder = String.Empty;
						continue;
					}

					placeholder += c;
				}
			}

			tokens = SanitazeTokens(tokens);
			return tokens;
		}

		private Token DetermineToken(String token) {
			if (token is "" or " ") {
				return new Token {
					token = "WHITESPACE",
					etoken = EToken.WHITESPACE
				};
			}

			token = token.Replace(" ", "");

			Token? tkn = this.IsRegisteredCharacter(token[0]);
			if (tkn != null) {
				return tkn.Value;
			}

			if (!Enum.IsDefined(typeof(EToken), token.ToUpper())) {
				return new Token {
					token = token,
					etoken = EToken.UNDEFINED
				};
			}

			return new Token {
				token = token,
				etoken = EnumHelper.GetEnumValueByName<EToken>(token)
			};
		}

		private Token? IsRegisteredCharacter(Char reg) {
			if (Char.IsSymbol(reg) || Char.IsPunctuation(reg)) {
				switch (reg) {
					case '=':
						return new Token {
							token = "=",
							etoken = EToken.EQUALS
						};
					case '-':
						return new Token {
							token = "-",
							etoken = EToken.MINUS
						};
					case '+':
						return new Token {
							token = "+",
							etoken = EToken.PLUS
						};
					case '(':
						return new Token {
							token = "(",
							etoken = EToken.LPAR
						};
					case ')':
						return new Token {
							token = ")",
							etoken = EToken.RPAR
						};
					case '{':
						return new Token {
							token = "{",
							etoken = EToken.LBRACE
						};
					case '}':
						return new Token {
							token = "}",
							etoken = EToken.RBRACE
						};
					case ';':
						return new Token {
							token = ";",
							etoken = EToken.EOL
						};
					case '"':
						return new Token {
							token = '"'.ToString(),
							etoken = EToken.QOUTE
						};
					case '.':
						return new Token {
							token = ".",
							etoken = EToken.PERIOD
						};
					case '<':
						return new Token {
							token = "<",
							etoken = EToken.LESS
						};
				}
			}

			return null;
		}
	}
}