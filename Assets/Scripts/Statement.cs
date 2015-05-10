using System;
using System.Collections;
using System.Collections.Generic;

public class Statement{
	private string type;
	private string condition;
	private string result;

	public Statement(string type, string condition, string result){
		this.type = type;
		this.condition = condition;
		this.result = result;
	}

	public string getType(){
		return this.type;
	}

	public string getCondtion(){
		return this.condition;
	}

	public string getResult(){
		return this.result;
	}
}
