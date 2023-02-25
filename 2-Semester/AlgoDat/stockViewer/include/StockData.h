#ifndef STOCKDATA_H
#define STOCKDATA_H

#include <string>


class StockData
{
public:
    /* ctor*/
    StockData(std::string data, std::string open, std::string high, std::string low, std::string close, std::string adjClose, std::string volume);

    /*getter functions */
    std::string getDate();
    std::string getOpen();
    std::string getHigh();
    std::string getLow();
    std::string getClose();
    std::string getAdjClose();
    std::string getVolume();
    StockData* getNext();

    /*setter functions */
    void setNext(StockData* nextDay);

    /*other functions */
    void print();
    void deleteData();
    std::string saveData();



private:

    /*variables */
    std::string date;
    std::string open;
    std::string high;
    std::string low;
    std::string close;
    std::string adjClose;
    std::string volume;
    StockData* next = NULL;

};

#endif // STOCKDATA_H
