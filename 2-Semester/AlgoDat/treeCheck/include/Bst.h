#ifndef BST_H
#define BST_H
#define Length 200

class Bst
{
public:
    Bst(int key, int height, Bst *left, Bst *right);
    virtual ~Bst();

    int* key;
    int height;
    Bst *left;
    Bst *right;
    // stats
    int minimum = 300;
    int maximum = -1;
    float sumKeys = 1; // weil 1 knoten
    int sumKeysAvg = 0;

    Bst *importValues(Bst *import, std::string fileToOpen);
    int getBalanceFactor(Bst *N);
    void preOrder(Bst *root, bool& isAVL);
    void printStats(Bst *stats, bool& isAVL);



    void deleteTree(Bst *root);


    /* SEARCH */
    void searchSubTree(std::string fileToSearch);
    void printPath(Bst *root, int valueToSearch);
    void simpleSearch(Bst *root, int valueToSearch, bool& found);
    void complexSearch(Bst *root, Bst *subTree);
    void checkSubTree(Bst *subTree, bool& found);
protected:

private:
    int heightTree(Bst *N);
    int max(int a, int b);
    Bst *newNode(int key);
    Bst *insertValues(Bst *node, int key);

};

#endif // BST_H
