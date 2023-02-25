#include <iostream>
#include <time.h>
#include <string.h>

#include "HashTable.h"

#define RUNTEST 0

using namespace std;


int main()
{
    HashTable* table = new HashTable;
    string userInput;

    /* inserts 1000 random stocks 50 times and calculates to average amount of not insertable stocks per 1000 stocks*/


#if RUNTEST

    /* inserts 1000 random stocks 50 times and calculates to average amount of not insertable stocks per 1000 stocks*/
    srand (time(NULL));
    char letters[] = {'A','B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

    int memmoryFullCounter = 0;
    for(int test = 0; test < 50; ++test) //run this test 50 times to create an average
    {
        for(int i = 0; i < 1000; ++i) //repeat for 1000 stocks
        {
            string randomStockSymbol;


            for(int i = 0; i < 4; ++i) //Picks 4 random letters and creates a word
            {
                int randomizer = rand() % 26;

                randomStockSymbol += letters[randomizer];
            }
            table->insertStock("EGAL", "EGAL", randomStockSymbol); //new stock is created using random word as symbol
        }

        memmoryFullCounter += table->memoryFullCount; //add memory full together for all tests

        delete table; //reset
        HashTable* table = new HashTable;
        table = table; //gets rid of warning
    }


    cout << "Average full memory: " << memmoryFullCounter / 50 << endl; //calculate average

#endif


    while(1)
    {
        cout << "Give me orders!" << endl;

        cin >> userInput;


        if(userInput == "ADD")
        {
            table->addStock();
        }
        else if(userInput == "DEL")
        {
            table->delStock();
        }
        else if(userInput == "IMPORT")
        {
            table->importStockDataFromCSV();
        }
        else if(userInput == "SEARCH")
        {
            table->searchStock();
        }
        else if(userInput == "PLOT")
        {
            table->plotStock();
        }
        else if(userInput == "SAVE")
        {
            table->saveTable();
        }
        else if(userInput == "LOAD")
        {
            delete table;
            HashTable* table = new HashTable;

            table->loadTable();
        }
        else if(userInput == "DROP")
        {
            delete table;
            HashTable* table = new HashTable;
            table = table; //gets rid of warning

        }
        else if(userInput == "QUIT")
        {
            cout << "Goodbye" << endl;
            break;
        }
        else
        {
            cout << "Invalid Input! Try again." << endl;
        }
    }

    delete table;

    return 0;
}
