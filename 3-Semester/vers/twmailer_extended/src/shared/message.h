#ifndef MESSAGE_H
#define MESSAGE_H

#include <string>
#include <iostream>
#include <fstream>
#include <vector>

#include "functions.h"
#include "SendOptionEnum.h"

class Message
{
private:
    int messageNumber;
    std::string sender;
    std::string receiver;
    std::string subject;
    std::string messageContent;
    std::string messageHead;
    std::string password;
    std::string messageString;

public:
    /* SETTER */
    bool setMessageNumber(int number);
    bool setSender(std::string sender);
    void setPassword(std::string password);
    bool setReceiver(std::string receiver);
    bool setSubject(std::string subject);
    bool setMessageContent(std::string messageContent);
    void setMessageHead(SendOption messageHead);
    bool setMessageHead(std::string messageHead);

    /* GETTER */
    int getMessageNumber();
    std::string getSender();
    std::string getPassword();
    std::string getReceiver();
    std::string getSubject();
    std::string getMessageContent();
    std::string getMessageHead();
    std::string getMessageString();

    /* FUNC */
    void loadMessage(std::string msgPath, int number);
    void cleanMsg();
    void createMsgString();

    Message();
};
#endif