#ifndef FUNCTIONS_H
#define FUNCTIONS_H

#include <string>
#include <iostream>
#include <chrono>
#include <ctime>

void exitFailure(std::string errorMsg);
bool checkMaxSize(std::string word, int max);
bool isDigitLetterOnly(std::string word);
bool isDigitLetterSpaceOnly(std::string word);
bool hasNoBackSlash(std::string str);

std::string getCurrentTime();

#endif