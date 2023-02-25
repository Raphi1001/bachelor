#pragma once
#include "Ship.h"

#include <string>
class Destroyer :
    public Ship
{
public:
    Destroyer(int posX, int posY, std::string playerName);
    ~Destroyer();

    void attack(Ship* shipToAttack);
};

