#include <iostream>
#include <string>
#include <vector>

#include "structs.h"
#include "Graph.h"


using namespace std;


int main()
{
	cout << "Welcome" << endl
		<< "Please make sure your textfiles are placed in the folder 'textfiles' " << endl << endl;

	while (1)
	{
		cout << "Enter your orders:" << endl
			<< "Quit => (quit)" << endl
			<< "find_path ";

		string eingabe;
		vector<string> inputWords;

		getline(cin, eingabe);
		string singleWord = "";
		for (auto x : eingabe)
		{
			if (x == ' ')
			{
				inputWords.push_back(singleWord);
				singleWord = "";
			}
			else {
				singleWord = singleWord + x;
			}
		}
		inputWords.push_back(singleWord);

		if (inputWords.size() == 1) { //create graph 
			Graph diagraph(inputWords[0]);
		}
		else if (inputWords.size() == 1 && inputWords[0] == "quit") {//quit program
			cout << endl << "Programm wird beendet." << endl;
			break;
		}
		else {
			cout << endl << "Invalid input!" << endl << endl << endl; //invalid input
		}
	}
	return 0;
}
