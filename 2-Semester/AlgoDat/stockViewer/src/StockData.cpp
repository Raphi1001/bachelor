#include <iostream>
#include <string>

#include "StockData.h"

using namespace std;
/* ctor */
StockData::StockData(string date, string open, string high, string low, string close, string adjClose, string volume)
{
    //ctor

    this->date = date;
    this->open = open;
    this->high = high;
    this->low = low;
    this->close = close;
    this->volume = volume;
    this->adjClose = adjClose;
    this->next = NULL;
}

/* returns the next StockData object from the linked list*/
StockData* StockData::getNext()
{
    return this->next;
}

/* returns the close*/
string StockData::getClose()
{
    return this->close;
}
/* returns the date*/
string StockData::getDate()
{
    return this->date;
}

/* adds a StockData object to the linked list*/
void StockData::setNext(StockData* nextDay)
{
    this->next = nextDay;
}
/* prints the data of a stock*/
void StockData::print()
{
    cout <<"(" << this->date << "," << this->open << "," << this->high << "," << this->low << "," << this->close << "," << this->volume << "," << this->adjClose << ")"<< endl;
}

/* deletes the data of a stock and its memory*/
void StockData::deleteData()
{
    StockData* current = this;

    while(current != NULL)
    {
        StockData* previous = current;
        current = current->next;
        delete previous;
    }
}


/* saves the data of a stock as a string*/
string StockData::saveData()
{
    string stockData = date + "," + open + ","  + high + ","  + low + ","  + close + ","  + adjClose + ","  + volume + ",";
    return stockData;
}


