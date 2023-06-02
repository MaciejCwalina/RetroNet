namespace RetroNet {
	public class Lexer {
		private String[] _file;

		public Lexer(String pathToFile) {
			this._file = File.ReadAllLines(pathToFile);
		}

		public List<Token> Lex() {
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

			return tokens;
		}

		private List<Token> DetermineTokenLineByLine(String token) {
			String placeholder = String.Empty;
			List<Token> tokens = new List<Token>();
			foreach (Char c in token) {
				placeholder += c;
				if (EnumHelper.Contains<EToken>(placeholder)) {
					tokens.Add(new Token {
						token = placeholder,
						etoken = this.DetermineToken(placeholder).etoken
					});
					placeholder = String.Empty;
				}

				if (placeholder.Length == token.Length) {
					tokens.Add(new Token {
						token = token,
						etoken = EToken.UNDEFINED
					});
				}
			}

			return tokens;
		}

		private Token DetermineToken(String token) {
			if (token == "" || token == " ") {
				return new Token {
					token = "WHITESPACE",
					etoken = EToken.WHITESPACE
				};
			}

			token = token.ToUpper().Replace(" ", "");
			if (Char.IsSymbol(token[0]) || Char.IsPunctuation(token[0])) {
				switch (token[0]) {
					case '=':
						return new Token {
							token = "=",
							etoken = EToken.EQUALS
						};
						break;
					case '-':
						return new Token {
							token = "-",
							etoken = EToken.MINUS
						};
						break;
					case '+':
						return new Token {
							token = "+",
							etoken = EToken.PLUS
						};
						break;
					case '(':
						return new Token {
							token = "(",
							etoken = EToken.LPAR
						};
						break;
					case ')':
						return new Token {
							token = ")",
							etoken = EToken.RPAR
						};
						break;
					case '{':
						return new Token {
							token = "{",
							etoken = EToken.LBRACE
						};
						break;
					case '}':
						return new Token {
							token = "}",
							etoken = EToken.RBRACE
						};
						break;
					case ';':
						return new Token {
							token = ";",
							etoken = EToken.EOL
						};
						break;
					
				}
			}

			if (!Enum.IsDefined(typeof(EToken), token)) {
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
	}
}