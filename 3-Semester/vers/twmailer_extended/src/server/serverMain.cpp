#include "server.h"
#include "database.h"

int main(int argc, char *argv[])
{
    Server server;
    server.readInput(argc, argv);

    server.setupSocket();

    server.listenToClient();
}