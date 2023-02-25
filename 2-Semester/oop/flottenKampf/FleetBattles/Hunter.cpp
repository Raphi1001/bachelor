#include "Hunter.h"

#include <iostream>
#include <string>

#include "enums.h"
#include "Game.h"

using namespace std;

Hunter::Hunter(int posX, int posY, string playerName) : Ship(posX, posY, playerName) {
	shell = 100;
	size = 3;
	damage = 40;
	type = Shiptype::hunter;
}

Hunter::~Hunter() {


}

void Hunter::attack(Ship* shipToAttack)
{
	int rand = Game::getRandomNumber(10) + 1;
	if (rand < shipToAttack->getSize()) {

		cout << "The attack was not successfull." << endl;
		++experience;
		shipToAttack->addExperience(2);
		return;
	}

	cout << "The attack was successfull." << endl;
	float attackDamage;

	attackDamage = damage * (1 + 0.1 * experience); //damage grows with experience

	float diffX = abs(pos.x - shipToAttack->getPosition().x);
	float diffY = abs(pos.y - shipToAttack->getPosition().y);
	float distance = diffX + diffY;
	cout << "Distance: " << distance << endl;

	float maxDistance = 2 * Game::mapSize - 2;
	float distancePercent = distance / maxDistance;
	float damageReduction = 0.3 * distancePercent;
	attackDamage *= (1 - damageReduction);


	if (9 <= rand) {
		attackDamage *= 2;
		cout << "The attacker was able to get a critical hit." << endl;
	}

	if (experience >= 10 && shipToAttack->getShell() < attackDamage) {
		attackDamage = shipToAttack->getShell();
		cout << "The attacker used Kamikaze to destroy the enemie." << endl;
		if (Game::getRandomNumber(10) >= 6) {									//60 % odds of being destroyed
			cout << "The attacker was damaged badly during Kamikaze and was destroyed." << endl;
			shell = 0;
		}
	}
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
