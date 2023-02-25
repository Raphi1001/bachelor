#include "Cruiser.h"

#include <iostream>
#include <string>

#include "enums.h"
#include "Game.h"

using namespace std;

Cruiser::Cruiser(int posX, int posY, string playerName) :Ship(posX, posY, playerName) {
	shell = 250;
	size = 3;
	damage = 40;
	type = Shiptype::cruiser;
}

Cruiser::~Cruiser() {


}

void Cruiser::attack(Ship* shipToAttack)
{
	while (1) {
		int rand = Game::getRandomNumber(10) + 1;

		if (experience > 10) {
			rand = Game::getRandomNumber(12) + 1;			//gets better dice
		}

		if (experience > 5) {
			size = 5;										//gets better defence
		}


		if (rand < shipToAttack->getSize()) {

			cout << "The attack was not successfull." << endl;
			++experience;
			shipToAttack->addExperience(2);
			return;
		}

		cout << "The attack was successfull." << endl;

		float attackDamage;

		attackDamage = damage;

		float diffX = abs(pos.x - shipToAttack->getPosition().x);
		float diffY = abs(pos.y - shipToAttack->getPosition().y);
		float distance = diffX + diffY;
		cout << "Distance: " << distance << endl;

		float maxDistance = 2 * Game::mapSize - 2;
		float distancePercent = distance / maxDistance;
		float damageReduction = 0.3 * distancePercent;
		attackDamage *= (1 - damageReduction);

		cout << "Attack Damage: " << attackDamage << endl;
		cout << "Shell Points: ";

		if (!shipToAttack->reduceShell(attackDamage))
		{
			cout << "Defender was destroyed" << endl;
			experience += 5;
			return;
		}

		cout << "Defender has " << shipToAttack->getShell() << " shell points left." << endl
			<< "The attacker was able to start a another attack." << endl << endl;
		experience += 2;
		shipToAttack->addExperience(1);
	}
}

