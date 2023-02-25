#pragma once
#include <string>
#include <vector>

#include "Ship.h"

class Player
{
public:
	Player(std::string name, int id);
	~Player();
	static int fleetSize;
	
	std::vector<Ship*> getFleet();
	Ship* getShip(int shipId);
	std::string getName();
	int getFleetSize();

	void removeShip(int shipIdToRemove);

private:
	std::string name;
	int id;
	std::vector<Ship*> fleet;

	Ship* createShip(char type, int posX, int posY, std::string playerName);

};

