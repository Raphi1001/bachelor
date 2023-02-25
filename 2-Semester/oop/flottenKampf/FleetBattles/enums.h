#pragma once
#include <iostream>

enum class Shiptype {
	none,
	hunter,
	destroyer,
	cruiser
};

struct Position {
	int x = 0;
	int y = 0;
};

struct shipInfo {
	int playerId;
	unsigned int shipId;
};

template <typename Enumeration>
auto as_integer(Enumeration const value)
-> typename std::underlying_type<Enumeration>::type
{
	return static_cast<typename std::underlying_type<Enumeration>::type>(value);
}