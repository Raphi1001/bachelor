#include "Player.h"

#include <iostream>

#include "Ship.h"
#include "Hunter.h"
#include "Destroyer.h"
#include "Cruiser.h"
#include "enums.h"
#include "Game.h"

using namespace std;


Player::Player(string name, int id) {

	this->name = name;
	this->id = id;
	cout << name + " choose Ship types. Hunter (h), Destroyer (d) or Cruiser (c)" << endl;
	for (int i = 1; i <= fleetSize; ++i)
	{
		char shipType;
		while (1) {
			cout << "Ship " << i << ": ";
			cin >> shipType;
			if (shipType == 'h' || shipType == 'd' || shipType == 'c') {


				int posX = Game::shipCount;
				++Game::shipCount;
				int posY = Game::getRandomNumber(Game::mapSize);
				
				Ship* newShip = createShip(shipType, posX, posY, name);
				fleet.push_back(newShip);
				Game::shipPositions[posY][posX] = newShip;

				break;
			}
			cout << "Invalid Input" << endl;
		}
	}
}

Player::~Player() {
}

vector<Ship*> Player::getFleet()
{
	return this->fleet;
}

Ship* Player::getShip(int shipId)
{
	if (fleet.size() > 0)
		return fleet[shipId];

	cout << "Invalid Ship Id" << endl;
	return nullptr;
}

string Player::getName()
{
	return name;
}

int Player::getFleetSize()
{
	return fleet.size();
}

void Player::removeShip(int shipIdToRemove)
{
	if (fleet[shipIdToRemove] && fleet[shipIdToRemove] != nullptr) {
		fleet[shipIdToRemove]->deletePos();
		delete fleet[shipIdToRemove];
		fleet.erase(fleet.begin() + shipIdToRemove);
	}
}

Ship* Player::createShip(char type, int posX, int posY, string playerName) {

	Ship* newShipPointer = nullptr;
	switch (type)
	{
	case 'h':
	{
		Hunter* newHunter = new Hunter(posX, posY, playerName);
		newShipPointer = newHunter;
	}
	break;
	case 'd':
	{
		Destroyer* newDestroyer = new Destroyer(posX, posY, playerName);

		newShipPointer = newDestroyer;
		break;
	}
	case 'c':
	{
		Cruiser* newCruiser = new Cruiser(posX, posY, playerName);

		newShipPointer = newCruiser;
		break;
	}
	default:
		cout << "Error: Unknown Shiptype" << endl;
	}
	return newShipPointer;
}


