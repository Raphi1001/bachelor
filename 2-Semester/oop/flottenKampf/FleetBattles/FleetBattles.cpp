// Flottenkampf.cpp : Diese Datei enthält die Funktion "main". Hier beginnt und endet die Ausführung des Programms.
//
#include <iostream>
#include <map>

#include "Game.h"
#include "Player.h"
#include "enums.h"

int Player::fleetSize = 1;
int Game::mapSize = 1;
std::map<int, std::map<int, Ship*>> Game::shipPositions;
int Game::shipCount = 0;

int main()
{
	Game newGame;
	return 0;
}


