#ifndef HASHTABLE_H
#define HASHTABLE_H

#include <string>

#include "Stock.h"

#include "tableOperation.h"

class HashTable
{
public:
    /* destructor*/
    ~HashTable();

    /* getter functions */
    std::string getInput();
    int getStockIndex();

    /* other functions */
    void addStock();
    int insertStock(std::string stockName, std::string stockWKN, std::string stockSymbol);
    void delStock();
    void importStockDataFromCSV();
    void searchStock();
    void plotStock();
    void saveTable();
    void loadTable();

    /* variables*/
    int memoryFullCount = 0;


private:
    /* functions*/
    int hashFunktion(std::string key, tableOperation operation);

    /* variables */
    static const int tableSize = 1901;
    Stock stocks[tableSize];

};

#endif // HASHTABLE_H
