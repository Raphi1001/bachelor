#pragma once
#include "Ship.h"

#include <string>

class Hunter :
    public Ship
{
public:
    Hunter(int posX, int posY, std::string playerName);
    ~Hunter();

    void attack(Ship* shipToAttack);
};

