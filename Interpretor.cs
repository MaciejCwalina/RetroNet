<<<<<<< HEAD
using System.Reflection;
=======
﻿using System.Reflection;
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
using RetroNet.Handlers;
using RetroNet.Interfaces;

namespace RetroNet {
	public class Interpretor {
		private List<Token> _tokens;
		private FunctionHandler _functionHandler;
<<<<<<< HEAD
		private VariableHandler _variableHandler;
		private StructHandler _structHandler;
		public Interpretor(List<Token> tokens) {
			this._tokens = tokens;
			this._structHandler = new StructHandler(this._tokens);
			this._variableHandler = new VariableHandler(this._structHandler);
			this._functionHandler = new FunctionHandler(this._tokens,this._variableHandler);
			//this._ifStatementHandler = new IfStatementHandler(this._tokens);

=======
		public Interpretor(List<Token> tokens) {
			this._tokens = tokens;
			this._functionHandler = new FunctionHandler(this._tokens);
			//this._ifStatementHandler = new IfStatementHandler(this._tokens);
			
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
		}

		public void LoadIntoMemory() {
			for (Int32 i = 0; i < this._tokens.Count; i++) {
				if (this._tokens[i].etoken == EToken.FN) {
					this._functionHandler.CreateFunction(i);
				}
<<<<<<< HEAD

=======
>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
				/*
                if (this._tokens[i].etoken == EToken.IF) {
                    this._ifStatementHandler.CreateIfStatement(i);
                }
				*/
<<<<<<< HEAD

				if (this._tokens[i].etoken == EToken.STRUCT) {
					this._structHandler.CreateStruct(i);
				}

			}

			if (!this._functionHandler.TryGetFunction("main",out Function? func)) {
				throw new Exception($"Function Main was NULL, please create a main function");
			}

=======
                /*if (this._tokens[i].etoken == EToken.STRUCT) {
					this.CreateStruct(i);
				}*/

            }

			if (!this._functionHandler.TryGetFunction("main",out Function? func)) {
				throw new Exception($"Function Main was NULL, please create a main function");
			}

>>>>>>> c2a84940eb1105249054209cde8bbf55cc891ed9
			this._functionHandler.RunFunction(ref func!);
		}

		public static void Print(IVariable variable) {
			Console.WriteLine(variable.value);
		}
	}
}