#include "Destroyer.h"

#include <iostream>
#include <cstdlib>
#include <string>

#include "enums.h"
#include "Game.h"

using namespace std;

Destroyer::Destroyer(int posX, int posY, string playerName) : Ship(posX, posY, playerName) {
	shell = 150;
	size = 4;
	damage = 60;
	type = Shiptype::destroyer;
}

Destroyer:: ~Destroyer() {


}



void Destroyer::attack(Ship* shipToAttack)
{
	if ((Game::getRandomNumber(10) + 1) < (shipToAttack->getSize() - 2)) {

		cout << "The attack was not successfull." << endl;
		++experience;
		shipToAttack->addExperience(2);
		return;
	}

	float attackDamage = damage;

	if (experience < 7) {
		float diffX = abs(pos.x - shipToAttack->getPosition().x);
		float diffY = abs(pos.y - shipToAttack->getPosition().y);
		float distance = diffX + diffY;
		cout << "Distance: " << distance << endl;

		float maxDistance = 2 * Game::mapSize - 2;
		float distancePercent = distance / maxDistance;
		float damageReduction = 0.3 * distancePercent;
		attackDamage *= (1 - damageReduction);
	}

	if (experience >= 15) {
		cout << "The attacker healed himselve and gained 20 shell points" << endl; //heal
		shell += 20;
	}

	cout << "The attack was successfull." << endl;
	cout << "Attack Damage: " << attackDamage << endl;
	cout << "Shell Points: ";

	if (!shipToAttack->reduceShell(attackDamage))
	{
		cout << "Defender was destroyed" << endl;
		experience += 5;
		return;
	}

	cout << "Defender has " << shipToAttack->getShell() << " shell points left." << endl;
	experience += 2;
	shipToAttack->addExperience(1);
}

