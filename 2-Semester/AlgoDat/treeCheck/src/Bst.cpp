#include <iostream>
#include <string.h>
#include <fstream>
#include <vector>
#include <algorithm>
#include <bits/stdc++.h>

#include "Bst.h"
using namespace std;

Bst::Bst(int key, int height, Bst *left, Bst *right)
{
    this->key = new int;
    *(this->key) = key;
    this->height = 1;
    this->left = nullptr;
    this->right = nullptr;
}

Bst::~Bst()
{
    delete key;
    // dtor
}

void Bst::deleteTree(Bst *root)
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

Bst* Bst::importValues(Bst *root, string fileToOpen)
{
    string filename("textfiles/" + fileToOpen + ".txt");
    vector<int> input;
    int value;

    ifstream input_file(filename);
    if (!input_file.is_open())
    {
        cerr << endl << "Can't open file: " << filename << endl << endl << endl;
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
int Bst::heightTree(Bst *N)
{
    if(N == nullptr) return 0;
    return N->height;
}

int Bst::max(int a, int b)
{
    return (a > b) ? a : b;
}

// allokiert eine neue Node
Bst* Bst::newNode(int key)
{
    Bst *node = new Bst(key, 1, nullptr, nullptr);
    return node;
}

// getter für balance
int Bst::getBalanceFactor(Bst *N)
{
    if(N == nullptr) return 0;
    return heightTree(N->right) - heightTree(N->left);
}

// Rekursive funktion zum einfügen eines keys
// in Teilbaum mit Rückgabe der neuen root
Bst* Bst::insertValues(Bst *node, int key)
{
    // falls keine root vorhanden, wird eine erstellt
    if(node == nullptr) return newNode(key);

    if(key < *(node->key)) node->left = insertValues(node->left, key);
    else if(key > *(node->key)) node->right = insertValues(node->right, key);
    else return node; // gleiche keys sind nicht erlaubt

    // Höhe vorgängerknoten aktualisieren
    node->height = 1 + max(heightTree(node->left),
                           heightTree(node->right));

    // stats
    if(key < node->minimum) node->minimum = key;
    if(key > node->maximum) node->maximum = key;
    node->sumKeys++;
    node->sumKeysAvg += key;

    // retuniert node
    return node;
}

// traversierung des Baumes für preorder
void Bst::preOrder(Bst *root, bool& isAVL)
{
    if(root == nullptr) return;

    preOrder(root->right, isAVL);
    preOrder(root->left, isAVL);

    int balance = getBalanceFactor(root);
    if( -1 <= balance && balance <= 1)
        cout << "bal(" << *(root->key) << ") = " << balance << endl;
    else
    {
        cout << "bal(" << *(root->key) << ") = " << balance << " (AVL violation!)" << endl;
        isAVL = false;
    }
}


void Bst::printStats(Bst *stats, bool& isAVL)
{
    cout <<  endl << "AVL: ";
    if(isAVL) cout << "yes" << endl;
    else cout << "no" << endl;
    cout << "Min: " << stats->minimum << " Max: " << stats->maximum << " Avg: " << (*(stats->key) + stats->sumKeysAvg) / sumKeys << endl << endl << endl;
}


/* SEARCH */

void Bst::searchSubTree(string fileToSearch)
{
    string filename("textfiles/" + fileToSearch + ".txt");
    vector<int> input;
    int value;

    ifstream input_file(filename);
    if (!input_file.is_open())
    {
        cerr << endl << "Can't open file: " << filename << endl << endl << endl;
    }

    while (input_file >> value)
    {
        input.push_back(value);
    }

    if(input.size() == 1)
    {
        bool found = false;
        simpleSearch(this, input[0], found);
        if(!found)
            cout << endl << input[0] << " not found!" << endl << endl << endl;
        else
        {
            printPath(this, input[0]);
            cout << endl << endl << endl;
        }
    }
    else if(input.size() > 1)
    {
        Bst *subtree = nullptr;
        subtree = subtree->importValues(subtree, fileToSearch); //create binary search tree
        complexSearch(this, subtree);

        bool found = true;
        checkSubTree(subtree, found);
        if(found) cout << endl << "Subtree found" << endl << endl << endl;
        else cout << endl << "Subtree not found!" << endl << endl << endl;
    }
}


void Bst::simpleSearch(Bst *root, int valueToSearch, bool& found)
{
    if(root == nullptr) return;
    if(*(root->key) == valueToSearch)
    {
        cout << endl << valueToSearch << " found ";
        found = true;
        return;
    }
    else if(valueToSearch > *(root->key))
        simpleSearch(root->right, valueToSearch, found);
    else
        simpleSearch(root->left, valueToSearch, found);
}

void Bst::printPath(Bst *root, int valueToSearch)
{
    if(root == nullptr) return;

    if(valueToSearch == *(root->key)) cout << *(root->key) << " ";
    else cout << *(root->key) << ", ";

    if(valueToSearch > *(root->key))
        printPath(root->right, valueToSearch);
    else if(valueToSearch < *(root->key))
        printPath(root->left, valueToSearch);
}

void Bst::complexSearch(Bst *root, Bst *subTree)
{
    if(root == nullptr) return;
    if(subTree == nullptr) return;


    if(*(root->key) == *(subTree->key))
    {
        subTree->key = nullptr;
        complexSearch(root, subTree->left);
        complexSearch(root, subTree->right);
    }
    else if(*(subTree->key) > *(root->key))
        complexSearch(root->right, subTree);
    else
        complexSearch(root->left, subTree);
}


void Bst::checkSubTree(Bst *subTree, bool& found)
{
    if(subTree == nullptr) return;
    if(found == false) return;

    if(subTree->key != nullptr) found = false;

    checkSubTree(subTree->left, found);
    checkSubTree(subTree->right, found);
}
