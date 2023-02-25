#pragma once
#include "enums.h"

class Ship
{
public:

	Ship(int posX, int posY, std::string playerName);
	virtual ~Ship();

	int getShell();
	int getSize();
	int getDamage();
	bool reduceShell(int value);
	
	void deletePos();
	Position getPosition();

	void addExperience(int amount);
	int getExperience();
	


	virtual void attack(Ship* shipToAttack) = 0;
	void move();
	std::string getPlayerName();

	std::string getShipTypeString();

	Shiptype getShiptype();


protected:
	int shell = 0;
	int size = 0;
	int damage = 0;
	Shiptype type = Shiptype::none;

	Position pos;
	std::string playerName;

	int experience = 0;

private:

};

