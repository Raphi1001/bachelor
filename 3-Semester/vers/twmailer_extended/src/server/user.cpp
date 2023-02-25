#include "user.h"

using namespace std;

bool User::setUsername(std::string username)
{
    if (!checkMaxSize(username, 8) || !isDigitLetterOnly(username))
        return false;

    this->username = username;
    return true;
}

bool User::addMessage(std::string sender, std::string receiver, std::string subject, std::string messageContent, string pathToMsg)
{
    Message newMessage;
    if (!newMessage.setSender(sender))
        return false;

    if (!newMessage.setReceiver(receiver))
        return false;

    if (!newMessage.setSubject(subject))
        return false;

    if (!newMessage.setMessageContent(messageContent))
        return false;

    receivedMessages.push_back(newMessage);

    ofstream newMsgFile(pathToMsg); /* TAKE A LOOK BITXh*/
    if (!newMsgFile.is_open())
        return false;

    newMsgFile << sender << endl
               << receiver << endl
               << subject << endl
               << messageContent << endl;

    newMsgFile.close();
    return true;
}

string User::getUsername()
{
    return username;
}

vector<Message> User::getAllMessages()
{
    return receivedMessages;
}

Message *User::getMessage(int msgNumber)
{
    if (msgNumber < (int)receivedMessages.size())
    {
        return &receivedMessages[msgNumber];
    }

    return nullptr;
}

void User::loadMessage(string filepath)
{
    Message newMessage;
    ifstream input_file(filepath);

    if (!input_file.is_open())
        exitFailure("Ungültige Datenbank. File konnte nicht geöffnet werden: " + filepath);

    string currentLine;

    if (!getline(input_file, currentLine).good())
        exitFailure("Ungültige Datenbank. Filestruktur nicht korrekt: " + filepath);
    if (!newMessage.setSender(currentLine))
        exitFailure("Ungültige Datenbank. Ungültiger Sender: " + currentLine + ", File: " + filepath);

    if (!getline(input_file, currentLine).good())
        exitFailure("Ungültige Datenbank. Filestruktur nicht korrekt: " + filepath);
    if (!newMessage.setReceiver(currentLine))
        exitFailure("Ungültige Datenbank. Ungültiger Empfänger: " + currentLine + ", File: " + filepath);

    if (!getline(input_file, currentLine).good())
        exitFailure("Ungültige Datenbank. Filestruktur nicht korrekt: " + filepath);
    if (!newMessage.setSubject(currentLine))
        exitFailure("Ungültige Datenbank. Ungültiger Betreff: " + currentLine + ", File: " + filepath);

    if (!getline(input_file, currentLine).good())
        exitFailure("Ungültige Datenbank. Filestruktur nicht korrekt: " + filepath);

    string message = "";
    bool messageEnd = false;
    while (getline(input_file, currentLine).good())
    {
        message.append(currentLine + '\n');

        if (message.substr(message.size() - 3) == "\n.\n")
        {
            messageEnd = true;
            break;
        }
    }

    if (!messageEnd)
        exitFailure("Ungültige Datenbank. Filestruktur nicht korrekt: " + filepath);
    if (!newMessage.setMessageContent(message))
        exitFailure("Ungültige Datenbank. Ungültige Nachricht: " + message + ", File: " + filepath);

    input_file.close();

    receivedMessages.push_back(newMessage);
}

bool User::deleteMessage(int msgNumber, std::string userDirectory)
{
    // remove from vector
    if (!deleteMessageVector(msgNumber) || !deleteMessageFiles(msgNumber, userDirectory))
        return false;

    return true;
}

bool User::deleteMessageVector(int msgNumber)
{
    if (msgNumber < (int)receivedMessages.size())
    {
        receivedMessages.erase(receivedMessages.begin() + msgNumber);
        return true;
    }
    return false;
}

bool User::deleteMessageFiles(int msgNumber, std::string userDirectory)
{
    int currentMsgNumber = 0;
    struct dirent *direntp;
    DIR *dirp = opendir(userDirectory.c_str());

    if (!dirp)
        return false;

    while ((direntp = readdir(dirp)) != NULL && currentMsgNumber <= msgNumber)
    {
        if (strcmp(direntp->d_name, ".") != 0 && strcmp(direntp->d_name, "..") != 0) //"." und ".." Verzeichnisse überspringen
        {
            if (msgNumber == currentMsgNumber)
            {
                string filepath(userDirectory + "/" + direntp->d_name);
                if (remove(filepath.c_str()) == 0)
                {
                    closedir(dirp);
                    return true;
                }
                break;
            }
            ++currentMsgNumber;
        }
    }
    return false;
    closedir(dirp);
}
