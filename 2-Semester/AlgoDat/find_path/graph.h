#pragma once
#include <string>
#include <vector>

#include "structs.h"

class Graph {

public:
	Graph(std::string fileToOpen);														// Constructor
	~Graph();																			// Destructor
	
	std::vector<graphEdge> readFile(std::string filename);
	void buildGraph(std::vector<graphEdge> edges);
	station** head;																		//station list as array of pointers

private:
	station* getStationListNode(int value, int weight, station* head);					// insert new nodes into station list from given graph
	
	void display_AdjList(station* ptr, int i, graphEdge edges);

	int verticesCount;																	// number of edges in the graph
	int edgesCount;																		// number of possible routes in the graph
};

