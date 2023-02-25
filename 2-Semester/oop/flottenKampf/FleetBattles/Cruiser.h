#pragma once
#include "Ship.h"

#include <string>

class Cruiser :
    public Ship
{
public:

    Cruiser(int posX, int posY, std::string playerName);
    ~Cruiser();
    void attack(Ship* shipToAttack);
};

