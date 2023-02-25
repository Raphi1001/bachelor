#pragma once

#include <string>

// stores adjacency list items
struct station {
	int index, weight;
	station* next = nullptr;
};
// structure to store edges
struct graphEdge {
	int start_index = 0, end_index = 0, weight = 0;
	std::string start_name;
	std::string end_name;
	std::string line;
};


