#pragma once
#include "Player.h"

#include <map>

#include "enums.h"

class Game
{
public:
	Game();  //ctor
	static int getRandomNumber(int max);
	static int mapSize;
	static std::map<int, std::map<int, Ship*>> shipPositions;
	static int shipCount;


	void printRow(int posY);

	void printBorderTop();
	void printBorderBottom(std::string name);
	void printNone();


	void printSubRows(int subRowToPrint, Shiptype type);

	void printSubRow1(Shiptype typeToPrint);
	void printSubRow2(Shiptype typeToPrint);
	void printSubRow3(Shiptype typeToPrint);

	void printHunter1();
	void printHunter2();
	void printHunter3();
	
	void printDestroyer1();
	void printDestroyer2();
	void printDestroyer3();
	
	void printCruiser1();
	void printCruiser2();
	void printCruiser3();
private:
	
	/* variables */
	std::vector<Player *> players;
	const static int playerAmount = 2;


	/* functions */
	void setFleetSize();
	void setPlayerFlet(int player);

	void removePlayer(int playerIdToRemove);

	void startRound(int player);

	void printMap();
};

