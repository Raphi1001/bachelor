#ifndef TESTS_H
#define TESTS_H

#include "HashTable.h"

class Test
{
public:

    void runTest(HashTable* table); /* inserts 1000 random stocks 50 times and calculates to average amount of not insertable stocks per 1000 stocks*/
};

#endif // TESTS_H

