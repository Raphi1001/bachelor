#ifndef NODE_H
#define NODE_H
#define Length 200

class Node
{
    public:
        Node(int key, int height, Node *left, Node *right);
        virtual ~Node();

        int key;
        int height;
        Node *left;
        Node *right;
        // stats
        int minimum = 300;
        int maximum = -1;
        float sumKeys = 2; // weil 1 knoten
        int sumKeysAvg = 0;

        Node *importValues(Node *import, std::string fileToOpen);
        int getBalanceFactor(Node *N);
        void preOrder(Node *root);
        void printStats(Node *stats);

        void deleteTree(Node *root);

    protected:

    private:
        int heightTree(Node *N);
        int max(int a, int b);
        Node *newNode(int key);
        Node *rightRotate(Node *y);
        Node *leftRotate(Node *x);
        Node *insertValues(Node *node, int key);

};

#endif // NODE_H
