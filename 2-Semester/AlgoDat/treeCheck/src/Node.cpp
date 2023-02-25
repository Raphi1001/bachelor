#include <iostream>
#include <string.h>
#include <fstream>
#include <vector>
#include <algorithm>
#include <bits/stdc++.h>

#include "Node.h"
using namespace std;

Node::Node(int key, int height, Node *left, Node *right)
{
    this->key = key;
    this->height = 1;
    this->left = nullptr;
    this->right = nullptr;
}

Node::~Node()
{
    // dtor
}

void Node::deleteTree(Node *root)
{
    if(root == nullptr) return;
    deleteTree(root->left);
    deleteTree(root->right);

    if(root->left != nullptr)
    {
        delete root->left;
        root->left = nullptr;
    }
    if(root->right != nullptr)
    {
        delete root->right;
        root->right = nullptr;
    }
}

Node* Node::importValues(Node *root, std::string fileToOpen)
{
    string filename("textfiles/" + fileToOpen + ".txt");
    vector<int> input;
    int value;

    ifstream input_file(filename);
    if (!input_file.is_open())
    {
        cerr << "Can't open file: " << filename << endl;
        return root;
    }

    while (input_file >> value)
    {
        input.push_back(value);
    }

    for (const auto& i : input)
    {
        root = root->insertValues(root, i);
    }

    return root;
}

// getter für höhe
int Node::heightTree(Node *N)
{
    if(N == nullptr) return 0;
    return N->height;
}

int Node::max(int a, int b)
{
    return (a > b) ? a : b;
}

// allokiert eine neue Node
Node* Node::newNode(int key)
{
    Node *node = new Node(key, 1, nullptr, nullptr);
    return node;
}

// rechts rotation
Node* Node::rightRotate(Node *y)
{
    Node *x = y->left;
    Node *tmp = x->right;

    // rotation
    x->right = y;
    y->left = tmp;

    // neue höhe
    y->height = max(heightTree(y->left),
                    heightTree(y->right)) + 1;
    x->height = max(heightTree(x->left),
                    heightTree(x->right)) + 1;
    return x;
}

// links rotation
Node* Node::leftRotate(Node *x)
{
    Node *y = x->right;
    Node *tmp = y->left;

    // rotation
    y->left = x;
    x->right = tmp;

    // neue höhe
    x->height = max(heightTree(x->left),
                    heightTree(x->right)) + 1;
    y->height = max(heightTree(y->left),
                    heightTree(y->right)) + 1;

    return y;
}

// getter für balance
int Node::getBalanceFactor(Node *N)
{
    if(N == nullptr) return 0;
    return heightTree(N->left) - heightTree(N->right);
}

// Rekursive funktion zum einfügen eines keys
// in Teilbaum mit Rückgabe der neuen root
Node* Node::insertValues(Node *node, int key)
{
    // falls keine root vorhanden, wird eine erstellt
    if(node == nullptr) return newNode(key);

    if(key < node->key) node->left = insertValues(node->left, key);
    else if(key > node->key) node->right = insertValues(node->right, key);
    else return node; // gleiche keys sind nicht erlaubt

    // Höhe vorgängerknoten aktualisieren
    node->height = 1 + max(heightTree(node->left),
                           heightTree(node->right));

    // getter methode für balance, um zu
    // prüfen ob es unbalanciert ist
    int balance = getBalanceFactor(node);

    // wenn es unbalanciert ist führt es
    // die entsprechende funktion aus
    if(balance > 1 && key < node->left->key) return rightRotate(node);
    if(balance < -1 && key > node->right->key) return leftRotate(node);
    if(balance > 1 && key > node->left->key)
    {
        node->left = leftRotate(node->left);
        return rightRotate(node);
    }
    if(balance < -1 && key < node->right->key)
    {
        node->right = rightRotate(node->right);
        return leftRotate(node);
    }

    // stats
    if(key < node->minimum) node->minimum = key;
    if(key > node->maximum) node->maximum = key;
    node->sumKeys++;
    node->sumKeysAvg += key;

    // retuniert node
    return node;
}

// traversierung des Baumes für preorder
void Node::preOrder(Node *root)
{
    if(root != nullptr)
    {
        cout << root->key << " ";
        preOrder(root->left);
        preOrder(root->right);
    }
}

void Node::printStats(Node *stats)
{
    cout << endl << endl << "Min: " << stats->minimum << " Max: " << stats->maximum << " Avg: " << stats->sumKeysAvg / sumKeys << endl;
}
