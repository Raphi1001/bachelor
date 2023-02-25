#ifndef DATABASE_H
#define DATABASE_H

#include <iostream>
#include <vector>
#include <string>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
#include <string.h>
#include <filesystem>
#include <fstream>
#include <dirent.h>

#include "user.h"
#include "../shared/functions.h"

class Database
{

private:
    std::string dir;
    std::vector<User> users;

    bool addMessageToUser(User *user, std::string sender, std::string receiver, std::string subject, std::string messageContent);

public:
    /* SETTER */

    void setDir(std::string dirName);
    void addUser(User user);
    bool addNewUser(User *newUser, std::string username);

    /* GETTER */

    std::vector<User> getAllUsers();
    User *getUser(std::string username);
    Message *getUserMessage(std::string username, int msgNumber);

    /* FUNC */

    bool dirExists();
    bool dirIsEmpty();
    void loadDatabase();
    void loadUser(std::string username);
    bool sendMessage(std::string sender, std::string receiver, std::string subject, std::string messageContent);
    bool deleteUserMessage(std::string username, int msgNumber);

    void printAllUsers();
};
#endif