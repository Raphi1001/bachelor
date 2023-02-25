#include <iostream>
#include <string>
#include <fstream>
#include <algorithm>

#include "Stock.h"

using namespace std;
/* default ctor */
Stock::Stock()
{
    this->name = "NULL";
    this->WKN = "NULL";
    this->symbol = "NULL";
    this->data = NULL;
}
/* optional ctor */
Stock::Stock(std::string name, std::string WKN, std::string symbol)
{
    this->name = name;
    this->WKN = WKN;
    this->symbol = symbol;
    this->data = NULL;

}
/*returns the name of a stock */
string Stock::getName()
{
    return this->name;
}
/*returns the WKN of a stock */
string Stock::getWKN()
{
    return this->WKN;
}

/*returns the symbol of a stock */
string Stock::getSymbol()
{
    return this->symbol;
}

/*deletes a stock*/
void Stock::del()
{
    this->name = "DEL";
    this->WKN = "DEL";
    this->symbol = "DEL";
    if(this->data != NULL)
    {
        this->data->deleteData();
    }
}

/* prints a stock*/
void Stock::print()
{
    cout << "Name:   " <<this->name << endl;
    cout << "WKN:    " <<this->WKN<< endl;
    cout << "Symbol: " <<this->symbol << endl;
    if(this->data != NULL)
    {
        this->data->print();
    }
    else
    {
        cout << "For this stock are no stats available yet. You can import them via the command IMPORT." <<endl;
    }
}

/* plots the close values of the last 30 days of stock*/
void Stock::plotClose30Days()
{
    if(this->data == NULL)
    {
        cout << "For this stock are no stats available yet. You can import them via the command IMPORT." <<endl;
        return;
    }

    string highestClose = this->getHighestClose()->getClose();
    string lowestClose = this->getLowestClose()->getClose();
    string highestCloseClean = highestClose;
    string lowestCloseClean = lowestClose;

    highestCloseClean.erase (remove(highestCloseClean.begin(), highestCloseClean.end(), '.'), highestCloseClean.end()); //get highest close and delete points from string
    lowestCloseClean.erase (remove(lowestCloseClean.begin(), lowestCloseClean.end(), '.'), lowestCloseClean.end()); //get highest close and delete points from string
    long long volume = stoll(highestCloseClean) - stoll(lowestCloseClean);

    int remainder[30] = { 0 }; //temporarily stores all close values for a stock

    string latestDate;
    string earliestDate;

    StockData* current = this->data;
    for(int i = 0; i < 30 && current != NULL; ++i)
    {
        string currentClose = current->getClose();
        currentClose.erase(remove(currentClose.begin(), currentClose.end(), '.'), currentClose.end()); //get current close and delete points from string
        remainder[i] = stoll(currentClose) - stoll(lowestCloseClean);

        if(i == 0)
        {
            latestDate = current->getDate(); //saves the most recent date
        }

        if(i == 29)
        {
            earliestDate = current->getDate(); //saves the oldest stored date
        }

        current = current->getNext();
    }

    int stepSize = 50; //determines how precise the graph will be
    int singleStep = volume / stepSize;

    /* the largest close will always have the max size the lowest close will always have size one*/

    cout << endl << "         STOCK PRIZE: " << this->name << "(" <<this-> symbol << ")            WKN: " << this->WKN << endl <<
         "  ______________________________________________________________________" << endl <<
         " |                                                                      |" << endl;
    for(int step = 0; step < stepSize; ++step)
    {
        cout << " |     ";
        for(int i = 29; i >= 0; --i)
        {
            if(remainder[i] - ( singleStep * ( stepSize - 1 - step) ) >= 0)
            {
                cout << "||";
            }
            else
            {
                cout << "  ";
            }
        }
        cout << "     |";

        if(step == 0) cout << highestClose;
        if(step == stepSize - 1) cout << lowestClose;

        cout << endl;
    }
    cout << " |                                                                      |" << endl <<
         " | " << earliestDate << "                                                " << latestDate << " |" << endl <<
         " |______________________________________________________________________|" << endl << endl;
}



/* returns the highest close from the last 30 days from a stock */
StockData* Stock::getHighestClose()
{
    StockData* highestClose = NULL;
    if(this->data != NULL)
    {
        StockData* current = this->data;
        StockData* next = current->getNext();
        highestClose = current;

        while(current != NULL && next != NULL)
        {
            if(highestClose->getClose() < next->getClose())
            {
                highestClose = next;
            }

            current= current->getNext();
            next = next->getNext();
        }
    }
    return highestClose;
}

/* returns the lowest close from the last 30 days from a stock */
StockData* Stock::getLowestClose()
{
    StockData* lowestClose = NULL;
    if(this->data != NULL)
    {
        StockData* current = this->data;
        StockData* next = current->getNext();
        lowestClose  = current;

        while(current != NULL && next != NULL)
        {
            if(lowestClose->getClose() > next->getClose())
            {
                lowestClose  = next;
            }

            current= current->getNext();
            next = next->getNext();
        }
    }
    return lowestClose ;
}

/* saves a stock as a string */
string Stock::saveStock()
{
    string stock = this->name + "," + this->WKN + "," + this->symbol + ",";
    StockData* current = this->data;
    while(current != NULL)
    {
        stock += current->saveData();
        current = current->getNext();
    }

    return stock;
}

/* loads a stock from a CSV file */
void Stock::loadStock(string stockToload)
{
    stockToload = this->importSingleLineFromCSV(stockToload, NULL); //import the first line data

    if(stockToload == "") //if no data available
    {
        this->data = NULL;
        return;
    }
    StockData* current = this->data;


    for(int i = 0; i < 29; ++i)
    {
        stockToload = this->importSingleLineFromCSV(stockToload, current);
        current = current->getNext();
    }
}

/* imports the data from last 30 days of a stock*/
void Stock::importLast30LinesFromCSV(string stockSymbol)
{
    ifstream fileToImport("stocks/" + stockSymbol + ".csv"); // Create an output filestream object to CSV file
    string line;

    if(fileToImport.is_open())
    {
        cout << "Starting Import..." <<endl;


        /* Finds the last line of file */
        fileToImport.seekg(-1,ios_base::end); //Go to the EOD

        while(1)
        {
            char curChar;
            fileToImport.get(curChar); //get the current char;

            if(fileToImport.tellg() <= 1) // If current char is first char
            {
                fileToImport.seekg(0);
                break;
            }
            else if(curChar == '\n') break;// else search for last line break;

            fileToImport.seekg(-2,ios_base::cur); // else keep moving one char back
        }


        /* Gets each line and moves upwards 29 times */
        string currentLine;
        getline(fileToImport,currentLine);// Read the current line


        if(this->importSingleLineFromCSV(currentLine, NULL) == "") //import the first line data
        {
            this->data = NULL;
            cout << "nicht gut";

            return;
        }

        StockData* previousLine = this->data;

        for(int days = 0; days < 29; ++days) //repeat for the last 29 lines
        {
            for(int i = 0; i < 2; ++i)
            {
                while(1)
                {
                    fileToImport.seekg(-2, ios_base::cur); // keep moving one char back

                    char curChar;
                    fileToImport.get(curChar); //get the current char;
                    if(curChar == '\n') break;   //search for last line break;
                }
            }
            getline(fileToImport,currentLine);// Read the current line

            this->importSingleLineFromCSV(currentLine, previousLine);//import the current line data
            previousLine = previousLine->getNext();
        }


        // Close the file
        fileToImport.close();

        cout << "Import Finished." <<endl;
    }
    else
    {
        cout << "The file could not be found" << endl;
    }
}

/* imports the data from a single days of a stock*/
string Stock::importSingleLineFromCSV(string lineToImport, StockData* importAfter)
{
    string dataArray[7]; //temporarily stores all the data for one day

    for(int i = 0; i < 7; ++i) //repeat the process for each dataset (open, close, high, ...)
    {
        int dataEnd = lineToImport.find(","); //index of next semicolon
        string dataToExtract = lineToImport.substr(0, dataEnd); //save string until first semi colon
        lineToImport = lineToImport.substr(dataEnd + 1);//cut dataToExtract out of line
        dataArray[i] = dataToExtract; //temporarily store the data
        if(dataArray[i].length() == 0)
        {
            return "";
        }
    }

    StockData* newData = new StockData(dataArray[0], dataArray[1], dataArray[2], dataArray[3], dataArray[4], dataArray[5], dataArray[6]); //create new StockData Object

    if(importAfter != NULL)
    {
        importAfter->setNext(newData); //import after last
    }
    else
    {
        this->data = newData; //import as first
    }
    return lineToImport;
}
