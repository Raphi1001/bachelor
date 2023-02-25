#include <string>
#include <iostream>
#include <fstream>
#include <vector>
#include <sstream>
#include <map>

#include "graph.h"
#include "structs.h"


using namespace std;

Graph::Graph(std::string fileToOpen) {
	head = nullptr;
	verticesCount = 0;
	vector<graphEdge> allEdges;
	allEdges = readFile(fileToOpen);
	if (allEdges.size() == 0) return;

	edgesCount = allEdges.size(); // Number of edges in the graph
	buildGraph(allEdges);

	// print adjacency list representation of graph
	cout << "Graph adjacency list " << endl << "(start_vertex, end_vertex, weight):" << endl;
	for (int i = 0; i < verticesCount; i++)
	{
		// display adjacent vertices of vertex i
		display_AdjList(head[i], i, allEdges[i]);
	}
}

Graph::~Graph() {
	if (head != nullptr) {
		for (int i = 0; i < verticesCount; i++) {
			if (head[i] != nullptr) delete[] head[i];
		}
		delete[] head;
	}
}


void Graph::buildGraph(vector<graphEdge> edges) {

	// allocate new node
	head = new station * [verticesCount]();
	this->verticesCount = verticesCount;
	// initialize head pointer for all vertices
	for (int i = 0; i < verticesCount; ++i) {
		head[i] = nullptr;
	}
	// construct directed graph by adding edges to it
	for (unsigned i = 0; i < edgesCount; i++) {
		int start_index = edges[i].start_index;
		int end_index = edges[i].end_index;
		int weight = edges[i].weight;
		std::string end_name = edges[i].end_name;
		// insert in the beginning
		station* newStation = getStationListNode(end_index, weight, head[start_index]);

		// point head pointer to new node
		head[start_index] = newStation;
	}
}

// insert new nodes into adjacency list from given graph
station* Graph::getStationListNode(int index, int weight, station* head) {
	station* newStation = new station;
	newStation->index = index;
	newStation->weight = weight;

	newStation->next = head;   // point new node to current head
	return newStation;
}

vector<graphEdge> Graph::readFile(std::string filename) {
	// Create a text string, which is used to output the text file
	vector<graphEdge> allEdges;
	map<string, int> allStations;
	cout << endl << endl << endl;

	// Read from the text file
	ifstream allLines(filename + ".txt");
	if (!allLines.is_open()) {
		cout << "The file your entered doesn't exist!" << endl;
		vector<graphEdge> empty;
		return empty;
	}
	string currentLine;
	graphEdge currentEdge;
	string subString;

	while (getline(allLines, currentLine))
	{
		stringstream line(currentLine);


		getline(line, subString, '\:');									//gets line name
		currentEdge.line = subString;

		getline(line, subString, '\"');									//gets first start station name (temporary endstation)
		getline(line, subString, '\"');
		currentEdge.end_name = subString;

		if (allStations[currentEdge.end_name]) {						//gets index for start_station (temporary endstation)
			currentEdge.end_index = allStations[currentEdge.end_name];
		}
		else {
			currentEdge.end_index = verticesCount;
			allStations[currentEdge.end_name] = verticesCount;
			++verticesCount;
		}

		while (1) {
			getline(line, subString, '\"');								//gets edge weight
			if (subString.size() == 0) {								//if end of line is reached
				break;
			}

			currentEdge.start_name = currentEdge.end_name;
			currentEdge.start_index = currentEdge.end_index;

			stringstream stringToInt(subString);						//convert string to int
			int weight = 0;
			stringToInt >> weight;
			currentEdge.weight = weight;

			getline(line, subString, '\"');								//gets end station name
			currentEdge.end_name = subString;

			if (allStations[currentEdge.end_name]) {					//gets index for end_station
				currentEdge.end_index = allStations[currentEdge.end_name];
			}
			else {
				currentEdge.end_index = verticesCount;
				allStations[currentEdge.end_name] = verticesCount;
				++verticesCount;
			}
			allEdges.push_back(currentEdge);
		}
	}
	allLines.close();													// Close the file
	return allEdges;
}

// print all adjacent vertices of given vertex
void Graph::display_AdjList(station *ptr, int i, graphEdge edges)
{
	while (ptr != nullptr) {
		cout << "(" << i << ", " << ptr->index
			<< ", " << ptr->weight << edges.start_name << ") ";
		ptr = ptr->next;
	}
	cout << endl;
}

