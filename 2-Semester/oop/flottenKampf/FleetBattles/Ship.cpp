#include "Ship.h"
#include "enums.h"
#include "Game.h"

#include <iostream>
#include <string>

Ship::Ship(int posX, int posY, std::string playerName) {
	pos.x = posX;
	pos.y = posY;
	this->playerName = playerName;
}

Ship::~Ship() {

}


int Ship::getShell() {
	return shell;
}

int Ship::getSize() {
	return size;
}

int Ship::getDamage() {
	return damage;
}

bool Ship::reduceShell(int value) {
	if (shell - value <= 0) {
		shell = 0;
		return false;
	}

	shell = shell - value;
	return true;
}

Position Ship::getPosition() {
	return pos;
}

void Ship::deletePos() {
	Game::shipPositions[pos.y][pos.x] = nullptr;
}

void Ship::addExperience(int amount)
{
	experience += amount;
}

int Ship::getExperience()
{
	return experience;
}

void Ship::move()
{
	int newPosX = pos.x;
	int newPosY = pos.y;
	for (int i = 0; i < 10; ++i) { //stop after 10 tries or if available spot is found
		int posX = Game::getRandomNumber(Game::mapSize);
		int posY = Game::getRandomNumber(Game::mapSize);
		if (Game::shipPositions[posY][posX] == nullptr) {
			newPosX = posX;
			newPosY = posY;
			break;
		}
	}
	
	deletePos();
	pos.x = newPosX;
	pos.y = newPosY;
	
	Game::shipPositions[newPosY][newPosX] = this;
}

std::string Ship::getPlayerName()
{
	return playerName;
}

std::string Ship::getShipTypeString() {

	switch (type)
	{
	case Shiptype::cruiser:
		return "cruiser";
		break;
	case Shiptype::destroyer:
		return "destroyer";
		break;
	case Shiptype::hunter:
		return "hunter";
		break;
	default:
		return "none";
	}
}

Shiptype Ship::getShiptype()
{
	return type;
}


