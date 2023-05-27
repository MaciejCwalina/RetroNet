namespace RetroNet {
	public class Lexer {
		private String[] _file;

		public Lexer(String pathToFile) {
			this._file = File.ReadAllLines(pathToFile);
		}

		public List<Token> Lex() {
			List<Token> tokens = new List<Token>();
			foreach (String line in this._file) {
				foreach (String str in line.Split(' ')) {
					Token token = this.DetermineToken(str);
					if (token.etoken == EToken.UNDEFINED) {
						tokens.Add(this.DetermineTokenLineByLine(str));
						continue;
					}

					tokens.Add(token);
				}
			}

			return tokens;
		}

		private Token DetermineTokenLineByLine(String token) {
			String placeholder = String.Empty;
			foreach (Char c in token) {
				placeholder += c;
				if (EnumHelper.Contains<EToken>(placeholder)) {
					return new Token {
						token = placeholder,
						etoken = this.DetermineToken(placeholder).etoken
					};
				}
			}

			return new Token {
				token = token,
				etoken = EToken.UNDEFINED
			};
		}

		private Token DetermineToken(String token) {
			if (String.IsNullOrEmpty(token)) {
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