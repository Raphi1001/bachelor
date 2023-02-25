#ifndef USER_H
#define USER_H

#include <iostream>
#include <string>
#include <vector>
#include <dirent.h>
#include <string.h>

#include "../shared/message.h"
#include "../shared/functions.h"

class User
{

private:
    std::string username;
    std::vector<Message> receivedMessages;

public:
    /* SETTER */
    bool setUsername(std::string username);
    bool addMessage(std::string sender, std::string receiver, std::string subject, std::string messageContent, std::string pathToMsg);
    void increaseFileCount();

    /* GETTER */
    std::string getUsername();
    int getFileCount();
    std::vector<Message> getAllMessages();
    Message *getMessage(int msgNumber);

    /* FUNC */
    void loadMessage(std::string filepath);
    bool deleteMessage(int msgNumber, std::string userDirectory);
    bool deleteMessageVector(int msgNumber);
    bool deleteMessageFiles(int msgNumber, std::string userDirectory);
};
#endif