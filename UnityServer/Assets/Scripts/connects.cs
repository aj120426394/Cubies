
using System;
using System.Collections;
using System.Collections.Generic;

public class connects {
	public string firstCircle;
	public string secondCircle;
	public string thirdCircle;
	public string fourthCircle;

	public string colour1;
	public string colour2;
	public string colour3;
	public string colour4;
	
	public connects(string circle, string circle1, string circle2, string circle3){
		firstCircle = circle;
		secondCircle = circle1;
		thirdCircle = circle2;
		fourthCircle = circle3;

		colour1 = circle;
		colour2 = circle1;
		colour3 = circle2;
		colour4 = circle3;

		//'if' colour 
		if ((circle1.Substring (0, 1)) == "b") {
			colour1 = "blue";
		}
		if ((circle1.Substring (0, 1)) == "r") {
			colour1 = "red";
		}
		if ((circle1.Substring (0, 1)) == "g") {
			colour1 = "green";
		}
		if ((circle1.Substring (0, 1)) == "y") {
			colour1 = "yellow";
		}

		//first 'then' colour 
		if ((circle1.Substring (0, 1)) == "b") {
			colour2 = "blue";
		}
		if ((circle1.Substring (0, 1)) == "r") {
			colour2 = "red";
		}
		if ((circle1.Substring (0, 1)) == "g") {
			colour2 = "green";
		}
		if ((circle1.Substring (0, 1)) == "y") {
			colour2 = "yellow";
		}


		//second 'then' colour
		if (circle2 != "") {
			if ((circle2.Substring (0, 1)) == "b") {
				colour3 = "blue";
			}
			if ((circle2.Substring (0, 1)) == "r") {
				colour3 = "red";
			}
			if ((circle2.Substring (0, 1)) == "g") {
				colour3 = "green";
			}
			if ((circle2.Substring (0, 1)) == "y") {
				colour3 = "yellow";
			}
		}
	}

}

