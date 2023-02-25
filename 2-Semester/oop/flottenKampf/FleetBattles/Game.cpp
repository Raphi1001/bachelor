#include "Game.h"

#include <iostream>
#include <string>
#include <vector>
#include <time.h>
#include <map>
#include <stdexcept>

#include "Player.h"

using namespace std;

Game::Game() {
	cout << "Fleet Battles" << endl << endl << endl;
	srand(time(NULL));
	setFleetSize();

	mapSize = playerAmount * Player::fleetSize;				//mapSize is total amount of ships

	for (int i = 0; i < mapSize; ++i) {						//set all positions to empty
		for (int u = 0; u < mapSize; ++u) {
			shipPositions[i][u] = nullptr;
		}
	}


	for (int i = 1; i <= playerAmount; ++i)
		setPlayerFlet(i);
	int currentPlayerId = getRandomNumber(players.size()); //get random player for first  round
	Player* currentPlayer = players[currentPlayerId];


	while (players.size() > 1) {
		startRound(currentPlayerId);
		currentPlayerId = (currentPlayerId + 1) % players.size(); //get next player;
	}

	cout << endl << endl << "###########################" << endl;
	cout << players.front()->getName() << " has won the game" << endl;
	cout << "###########################" << endl;

	printMap();
}


void Game::setFleetSize() {

	int fleetSize;
	while (1) {
		cout << "Fleet size: (1-9): ";
		cin >> fleetSize;
		if (cin.fail()) {
			cin.clear();
			cin.ignore(numeric_limits<streamsize>::max(), '\n');
		}
		else if (1 <= fleetSize && fleetSize <= 9) {
			Player::fleetSize = fleetSize;
			break;
		}
		cout << "Invalid Input" << endl;
	}

}

void Game::setPlayerFlet(int playerId) {

	string name;
	cout << endl << endl
		<< "Player " << playerId << " Name: ";
	cin >> name;

	Player* newPlayer = new Player(name, players.size());

	players.push_back(newPlayer);

}

void Game::removePlayer(int playerIdToRemove)
{
	if (players.size() < playerIdToRemove) return;
	if (players[playerIdToRemove] != nullptr) {
		delete players[playerIdToRemove];
		players.erase(players.begin() + playerIdToRemove);
	}
}

int Game::getRandomNumber(int max) {
	if (max == 0) return 0;
	int random = rand() % max;
	return random;
}

void Game::startRound(int playerId)
{
	if (playerId < 0 || players.size() <= playerId) {
		cout << "Invalid player Id" << endl;
		return;
	}

	printMap();

	int playerIdToAttack = (playerId + getRandomNumber(players.size() - 1) + 1) % players.size(); //random playerId that is not already selected

	Player* player = players[playerId];
	Player* playerToAttack = players[playerIdToAttack];


	int shipId = getRandomNumber(player->getFleetSize()); //random ship Id to attack with
	int shipIdToAtack = getRandomNumber(playerToAttack->getFleetSize()); //random ship Id to attack

	Ship* ship = player->getShip(shipId);
	if (ship == nullptr) {
		return;
	}
	Ship* shipToAttack = playerToAttack->getShip(shipIdToAtack);

	if (shipToAttack == nullptr) {
		return;
	}
	cout << endl << "<-----------ATACK----------->" << endl << endl;
	cout << "Attacker: " << player->getName() << " (" << ship->getShipTypeString() << ") Fleet size: " << player->getFleetSize() << " Experience: " << ship->getExperience() << endl
		<< "Defender: " << playerToAttack->getName() << " (" << shipToAttack->getShipTypeString() << ") Fleet size: " << playerToAttack->getFleetSize() << " Experience: " << shipToAttack->getExperience() << endl
		<< "Success: ";
	ship->attack(shipToAttack);
	ship->move();

	if (shipToAttack->getShell() <= 0)
	{
		playerToAttack->removeShip(shipIdToAtack);
	}

	if (ship->getShell() <= 0)
	{
		player->removeShip(shipId);
	}

	if (playerToAttack->getFleetSize() == 0) {
		cout << endl << "<>--<>--<>--<>--<>--<>--<>--<>--<>--<>--<>--<>" << endl
			<< "The ENTIRE fleet from " << playerToAttack->getName() << " was destroyed" << endl
			<< "<>--<>--<>--<>--<>--<>--<>--<>--<>--<>--<>--<>" << endl;
		removePlayer(playerIdToAttack);
	}
}

void Game::printMap() {

	for (int y = 0; y < mapSize; ++y) {
		printRow(y);
	}
}

void Game::printRow(int posY) {

	for (int x = 0; x < mapSize; ++x) {
		printBorderTop();
	}
	cout << endl;

	for (int i = 1; i <= 3; ++i) {
		for (int posX = 0; posX < mapSize; ++posX) {

			Shiptype type = Shiptype::none;

			if (shipPositions[posY][posX] != nullptr) {
				type = shipPositions[posY][posX]->getShiptype();
			}

			printSubRows(i, type);
		}
		if (posY == 0 && i == 1) {
			cout << "   ";
			printHunter1();
			printDestroyer1();
			printCruiser1();
		}
		else if (posY == 0 && i == 2) {
			cout << "   ";
			printHunter2();
			printDestroyer2();
			printCruiser2();
		}
		else if (posY == 0 && i == 3) {
			cout << "   ";
			printHunter3();
			printDestroyer3();
			printCruiser3();
		}
		if (posY == 1 && i == 1)
		{
			cout << "   ";
			cout << "  HUNTER    DESTROYER   CRUISER";
		}
		cout << endl;
		if (i == 3) {
			for (int x = 0; x < mapSize; ++x) {
				string playerName = "_________";

				if (shipPositions[posY][x] != nullptr) {
					playerName = shipPositions[posY][x]->getPlayerName();
				}
				printBorderBottom(playerName);
			}
		}
	}
	cout << endl;
}




void Game::printSubRows(int subRowToPrint, Shiptype type)
{
	switch (subRowToPrint)
	{
	case 1:
		printSubRow1(type);
		break;
	case 2:
		printSubRow2(type);
		break;
	case 3:
		printSubRow3(type);
		break;
	}
}



void Game::printSubRow1(Shiptype typeToPrint) {
	switch (typeToPrint) {
	case Shiptype::hunter:
		printHunter1();
		break;
	case Shiptype::destroyer:
		printDestroyer1();
		break;
	case Shiptype::cruiser:
		printCruiser1();
		break;
	default:
		printNone();
	}
}

void Game::printSubRow2(Shiptype typeToPrint) {
	switch (typeToPrint) {
	case Shiptype::hunter:
		printHunter2();
		break;
	case Shiptype::destroyer:
		printDestroyer2();
		break;
	case Shiptype::cruiser:
		printCruiser2();
		break;
	default:
		printNone();
	}
}
void Game::printSubRow3(Shiptype typeToPrint) {
	switch (typeToPrint) {
	case Shiptype::hunter:
		printHunter3();
		break;
	case Shiptype::destroyer:
		printDestroyer3();
		break;
	case Shiptype::cruiser:
		printCruiser3();
		break;
	default:
		printNone();
	}
}

void Game::printBorderTop() {
	cout << " _________ ";
}
void Game::printBorderBottom(string name) {
	if (name.size() > 9) {
		name = name.substr(0, 7);
		cout << "|" << name << "..|";
		return;
	}


	int spaces = 9 - name.size();
	int leftSpaces = spaces;
	cout << "|";
	while (leftSpaces > spaces / 2) {
		cout << "_";
		--leftSpaces;
	}
	cout << name;
	while (leftSpaces > 0) {
		cout << "_";
		--leftSpaces;
	}
	cout << "|";


}
void Game::printNone() {
	cout << "|         |";
}

void Game::printHunter1() {
	cout << "|  _ | _  |";
}
void Game::printHunter2() {
	cout << "| ___|___ |";
}
void Game::printHunter3() {
	cout << "| \\_____/ |";
}

void Game::printDestroyer1() {
	cout << "|  _   _  |";
}
void Game::printDestroyer2() {
	cout << "| _|___|_ |";
}
void Game::printDestroyer3() {
	cout << "| \\_____/ |";
}

void Game::printCruiser1() {
	cout << "|   / \\   |";
}
void Game::printCruiser2() {
	cout << "| _|___|_ |";
}
void Game::printCruiser3() {
	cout << "| \\_____/ |";
}


