#ifndef STOCK_H
#define STOCK_H

#include <string>

#include "StockData.h"

class Stock
{
public:
    /* ctors */
    Stock();
    Stock(std::string name, std::string WKN, std::string symbol);

    /*Getter Functions */
    std::string getName();
    std::string getWKN();
    std::string getSymbol();

    /* Other functions */
    void del(); //Sets all values to "DEL"
    void print(); //Prints all values
    void plotClose30Days();
    void importLast30LinesFromCSV(std::string stockSymbol);
    std::string saveStock();
    void loadStock(std::string stockToload);




private:
    /* functions */
    std::string importSingleLineFromCSV(std::string lineToImport, StockData* importAfter);
    StockData* getHighestClose();
    StockData* getLowestClose();

    /*Variables */
    std::string name;
    std::string WKN;
    std::string symbol;
    StockData* data;
};

#endif // STOCK_H
