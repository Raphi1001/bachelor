#include <iostream>
#include <time.h>
#include <string.h>

#include "tests.h"
#include "Hashtable.h"

using namespace std;


/* inserts 1000 random stocks 50 times and calculates to average amount of not insertable stocks per 1000 stocks*/
void Test::runTest(HashTable* table)
{
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
}
