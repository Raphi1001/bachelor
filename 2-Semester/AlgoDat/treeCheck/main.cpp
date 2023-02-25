#include <iostream>
#include <string.h>
#include <fstream>
#include <vector>
#include <algorithm>
#include <bits/stdc++.h>

#include "include\Node.h"
#include "include\Bst.h"
using namespace std;

int main()
{
    string eingabe;
    cout << "Welcome" << endl
         << "Please make sure your textfiles are placed in the folder 'textfiles' "<< endl << endl;
    while(1)
    {
        cout << "Enter your orders:" << endl
             << "Create: Binary Search Tree => (treecheck filename), eg: 'treecheck values' " << endl
             << "Search: Binary Search Tree => (treecheck filename-BST filename-subtree), eg: 'treecheck values subtree'" << endl
             << "Create: AVL Tree (Bonus) => (AVLtree filename), eg: 'AVltree values'" << endl
             << "Quit => (quit)" << endl;
        getline(cin, eingabe);

        stringstream ss(eingabe);
        string word;
        vector<string> inputWords;
        while (ss >> word)
        {
            inputWords.push_back(word);
        }



        if(inputWords.size() == 2 && inputWords[0] == "treecheck") //create binary search tree
        {
            Bst *root = nullptr;
            root = root->importValues(root, inputWords[1]);
            if(root != nullptr)
            {
                cout << endl << "Preorder: " << endl;
                bool isAVL = true;
                root->preOrder(root, isAVL);

                root->printStats(root, isAVL);
            }

            root->deleteTree(root);
            delete root;
        }
        else if(inputWords.size() == 3 && inputWords[0] == "treecheck") //search existing binary tree
        {
            Bst *root = nullptr;
            root = root->importValues(root, inputWords[1]); //create binary search tree
            root->searchSubTree(inputWords[2]); //search for subTree

            root->deleteTree(root);
            delete root;

        }
        else if(inputWords.size() == 1 && inputWords[0] == "quit") //quit program
        {
            cout << endl << "Programm wird beendet." << endl;
            break;
        }
        else if(inputWords.size() == 2 && inputWords[0] == "AVLtree") //create avl tree (bonus)
        {
            Node *root = nullptr;
            root = root->importValues(root, inputWords[1]);
            if(root != nullptr)
            {
                cout << endl << "Preorder: " << endl;
                root->preOrder(root);

                root->printStats(root);

                root->deleteTree(root);
                delete root;
            }
        }
        else
        {
            cout << endl << "Invalid input!" << endl << endl << endl; //invalid input
        }
    }


    return 0;
}
