#include <iostream>
#include <string>
#include <fstream>
#include <cmath>

#include "HashTable.h"
#include "Stock.h"


using namespace std;


HashTable::~HashTable()
{
    for(int i = 0; i < tableSize; ++i)
    {
        if(this->stocks[i].getSymbol() != "NULL" && this->stocks[i].getSymbol() != "DEL")
        {
            this->stocks[i].del();
        }
    }
}


int HashTable::hashFunktion(string key, tableOperation operation)
{
    string valueToSearch = (operation == adding) ? "NULL" : key; //if operations is adding search for NULL, else search for key

    int index = 0;

    for (int i = 0; i < (int) key.length(); ++i)
    {
        index += key[i];
    }

    index %= this->tableSize; //make sure index is not out of bound

    /* Alternating  quadratic probing */ //index + 1 -> index -1 -> index + 4 -> index -4 -> index + 9 -> ....
    int newIndex = index;

    for
    (
        /* positive abortion */
        int i = 2;
        this->stocks[newIndex].getSymbol() != valueToSearch && //break if searchFor value is found or
        !( operation == adding && this->stocks[newIndex].getSymbol() == "DEL" ); //break if operation is adding and DEL is found
        ++i
    )
    {
        int sign = pow( -1, i ); //alternates the sign for each i;

        int increment = pow( ( i / 2 ), 2 ); //value is squared every second i


        newIndex = index + sign * increment; //combines sign and increment and adds it to the original index;
        newIndex %= this->tableSize; //puts index in bound

        /* negative abortion */
        if(i > 60) return this->tableSize + 1; //stop after  200 tries;
        if(operation == searching && this->stocks[newIndex].getSymbol() == "NULL") return this->tableSize + 1; //if operation is searching and NULL is found return index out of bound
    }

    return newIndex;
}

string HashTable::getInput()
{
    while(1)
    {
        string userInput;
        cin >> userInput;
        size_t found = userInput.find_first_of("/.;,\\:");
        if(userInput == "DEL" || userInput == "NULL" || found != string::npos)
            cout << "Invalid Input! Try again." << endl;
        else
            return userInput;
    }
}
/* Gets user input and returns the index of a stock*/
int HashTable::getStockIndex()
{
    cout << endl << "Which Stock (Symbol)?" << endl;
    string stockSymbol = this->getInput();

    int index = this->hashFunktion(stockSymbol, searching);

    if(index > this->tableSize) cout << "Error: Stock not found!"<< endl;
    return index;
}

/* Creates a new stock and adds it into the table*/
int HashTable::insertStock(string stockName, string stockWKN, string stockSymbol)
{
    int index = this->hashFunktion(stockSymbol, adding);

    if(index < this->tableSize)
    {
        Stock newStock(stockName, stockWKN, stockSymbol);
        this->stocks[index] = newStock;
    }
    else
    {
        cout << "Error: Memory full!"<< endl;
        ++this->memoryFullCount;
    }

    return index;
}


/* Gets user input for a new stock and adds it to the table */
void HashTable::addStock()
{
    cout << endl << "Adding a new stock." << endl;

    cout << endl << "Stock Name?" << endl;
    string stockName = this->getInput();

    cout << endl << "Stock WKN?" << endl;
    string stockWKN = this->getInput();

    cout << endl << "Stock Symbol?" << endl;
    string stockSymbol = this->getInput();

    int index = this->insertStock(stockName, stockWKN, stockSymbol);

    if(index < this->tableSize)
    {
        Stock newStock(stockName, stockWKN, stockSymbol);
        this->stocks[index] = newStock;
        cout << "Stock was added at index: " << index << endl;
    }
}

/*deletes a stock from the table */
void HashTable::delStock()
{
    cout << endl << "Deleting a stock." << endl;

    int index = this->getStockIndex();
    if(index < this->tableSize)
    {
        this->stocks[index].del();
        cout <<"The Stock at index " << index << " was deleted." << endl;
    }
}

/*Imports data for a stock from a CSV file */
void HashTable::importStockDataFromCSV()
{
    cout << endl << "Importing stock data." << endl
         << "Please place to CSV file inside the folder stocks and name it \"STOCKSYMBOL.csv\" (eg: \"MSFT.csv\")." << endl;

    int index = this->getStockIndex();
    if(index < this->tableSize)
    {
        this->stocks[index].importLast30LinesFromCSV(this->stocks[index].getSymbol());
    }
}

/*Searches a stock */
void HashTable::searchStock()
{
    cout << endl << "Searching a stock." << endl;

    int index = this->getStockIndex();
    if(index < this->tableSize)
    {
        this->stocks[index].print();
    }
}

/* plots stock data in an ACSII format*/
void HashTable::plotStock()
{
    cout << endl << "Plotting a stock." << endl;

    int index = this->getStockIndex();
    if(index < this->tableSize)
    {
        this->stocks[index].plotClose30Days();
    }
}

/*saves the current table to a CSV file */
void HashTable::saveTable()
{
    cout << endl << "Saving the table..." << endl <<
         "Please give your table a name. Once saved, you can find the file in the folder \"tables\" (existing tables matching names will be overwritten)." << endl;

    string tableName;

    while(1)
    {
        cin >> tableName;
        size_t found = tableName.find_first_of("/.;,\\:");
        if(found == string::npos) break;
        cout << "Invalid Input! Try again." << endl;

    }

    ofstream tableFile ("tables/" + tableName + ".csv");

    string table = "";
    for(int i = 0; i < tableSize; ++i)
    {
        if(this->stocks[i].getSymbol() != "NULL" && this->stocks[i].getSymbol() != "DEL")
        {
            table += this->stocks[i].saveStock();
            table += "\n";
        }
    }
    tableFile << table << endl;
    tableFile.close();

    cout << endl << "Saving complete." << endl;

}

/* loads the current table from a CSV file*/
void HashTable::loadTable()
{
    cout << endl << "Loading the table..." << endl <<
         "Please enter the name of the table you want to load (e.g. table1) and make sure the CSV file is placed inside the folder \"tables \"." << endl;
    string tableName;

    while(1) //validates user input
    {
        cin >> tableName;
        size_t found = tableName.find_first_of("/.;,\\:");
        if(found == string::npos) break;
        cout << "Invalid Input! Try again." << endl;

    }
    ifstream tableToLoad("tables/" + tableName + ".csv"); // Create an output filestream object to CSV file
    if(tableToLoad.is_open())
    {
        string currentStock;
        int isRunning = true;
        while(getline(tableToLoad,currentStock))// Read the current line
        {
            if(currentStock.length() == 0) break;
            string dataArray[3]; //temporarily stores all the data for one day

            for(int i = 0; i < 3; ++i) //repeat the process for each stock attribute(name, WKN, symbol)
            {
                int dataEnd = currentStock.find(","); //index of next semicolon
                string dataToExtract = currentStock.substr(0, dataEnd); //save string until first semi colon
                currentStock = currentStock.substr(dataEnd + 1);//cut dataToExtract out of line

                dataArray[i] = dataToExtract; //temporarily store the data

                if(dataArray[i].length() == 0)
                {
                    cout << "The selected file was corrupted and can not be used!" << endl;
                    isRunning = false;
                }
            }

            if(!isRunning) break;

            int index = this->insertStock(dataArray[0], dataArray[1], dataArray[2]);

            this->stocks[index].loadStock(currentStock);
        }

        // Close the file
        tableToLoad.close();
    }
    else
    {
        cout << "The file could not be found" << endl;
    }

}
