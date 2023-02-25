#include "database.h"

using namespace std;

// Erstellt das mail-spool-directory falls es noch nicht existiert
void Database::setDir(string dirName)
{
    {
        // prüft ob working directory im bin ordner ist
        string path = filesystem::current_path();

        if (path.find("bin") != string::npos)
        {
            path = path.substr(0, path.size() - 3);
            filesystem::current_path(path);
        }
    }
    this->dir = dirName;
    if (!dirExists())
    {
        if (mkdir(dir.c_str(), S_IRWXU | S_IRWXG | S_IRWXO) == -1)
            exitFailure((char *)errno);
    }

    if (!dirIsEmpty())
        loadDatabase();
}

// Fügt dem users Vektor einen User hinzu
void Database::addUser(User user)
{
    for (int i = 0; i < (int)users.size(); i++)
    {
        if (user.getUsername() == users[i].getUsername())
        {
            exitFailure("Duplicate User: " + user.getUsername());
        }
    }

    users.push_back(user);
}

// Erstellt einen User und dessen directoy im mail-spool-directory
bool Database::addNewUser(User *newUser, std::string username)
{
    // username hinzufügen
    if (!newUser->setUsername(username))
        return false;

    // user directory hinzufügen
    string path = dir + "/" + username;
    if (mkdir(path.c_str(), S_IRWXU | S_IRWXG | S_IRWXO) == -1)
    {
        exitFailure((char *)errno);
        return false;
    }

    return true;
}

vector<User> Database::getAllUsers()
{
    return users;
}

// sucht einen bestimmten user und gibt diesen zurück
User *Database::getUser(string username)
{
    for (int i = 0; i < (int)users.size(); ++i)
    {
        if (username == users[i].getUsername())
        {
            return &users[i];
        }
    }
    return nullptr;
}

// sucht eine bestimmte nachricht von einem bestimmten user und gibt diese zurück
Message *Database::getUserMessage(string username, int msgNumber)
{
    //überprüng ob der user exisitert
    User *foundUser = getUser(username);

    if (foundUser == nullptr)
        return nullptr;

    //überprüfung ob die nachricht existert
    Message *foundMessage = foundUser->getMessage(msgNumber);
    if (foundMessage == nullptr)
        return nullptr;

    return foundMessage;
}

// prüft pb das mail-spool-directory existert
bool Database::dirExists()
{
    struct stat st = {0};

    if (stat(dir.c_str(), &st) == -1)
        return false;

    return true;
}

// prüft ob das mail-spool-directory leer ist
bool Database::dirIsEmpty()
{
    const filesystem::path dirPath{dir};
    if (filesystem::is_empty(dirPath))
        return true;

    return false;
}

// laed existierende datenbank
void Database::loadDatabase()
{
    struct dirent *direntp;
    DIR *dirp = opendir(dir.c_str());
    if (!dirp)
        exitFailure("Ungültige Datenbank. Directory konnte nicht geöffnet werden: " + dir);

    // ließt jedes user directory einzeln
    while ((direntp = readdir(dirp)) != NULL)
    {
        if (strcmp(direntp->d_name, ".") != 0 && strcmp(direntp->d_name, "..") != 0) //"." und ".." Verzeichnisse überspringen
            loadUser(direntp->d_name);
    }
    closedir(dirp);
}

// Läd alle nachrichten von einem user
void Database::loadUser(string username)
{
    User newUser;
    newUser.setUsername(username);

    struct dirent *direntp;
    string userDirectory(dir + "/" + username);
    DIR *dirp = opendir(userDirectory.c_str());
    if (!dirp)
        exitFailure("Ungültige Datenbank. Directory konnte nicht geöffnet werden: " + userDirectory);

    // ließt jede nachricht einzel
    while ((direntp = readdir(dirp)) != NULL)
    {
        if (strcmp(direntp->d_name, ".") != 0 && strcmp(direntp->d_name, "..") != 0) //"." und ".." Verzeichnisse überspringen
        {
            string filepath(userDirectory + "/" + direntp->d_name);
            newUser.loadMessage(filepath);
        }
    }
    closedir(dirp);
    addUser(newUser);
}

// verschickt eine nachricht von einem user an einen andern
bool Database::sendMessage(string sender, string receiver, string subject, string messageContent)
{
    User newUser;
    User *existingUser = getUser(receiver); // prüft ob der Empfänger bereits existiert
    User *user = existingUser;

    if (!existingUser)
    {
        addNewUser(&newUser, receiver); // erstellt einen neuen user, falls es ihn noch nicht gibt
        user = &newUser;
    }

    if (!user)
        return false; // ungültiger user

    if (!addMessageToUser(user, sender, receiver, subject, messageContent)) // fügt dem Empfänger die Nachricht hinzu
        return false;

    if (!existingUser)
        addUser(newUser); // speichert neuen user

    return true;
}

// fügt einem user eine nachricht bei
bool Database::addMessageToUser(User *user, string sender, string receiver, string subject, string messageContent)
{
    string newPath(dir + "/" + user->getUsername() + "/" + getCurrentTime() + ".txt");
    if (!user->addMessage(sender, receiver, subject, messageContent, newPath))
        return false;

    return true;
}

// löscht bestimmte nachricht von bestimmtem user
bool Database::deleteUserMessage(string username, int msgNumber)
{
    User *foundUser = getUser(username); // prüft ob user existert

    if (foundUser == nullptr)
        return false;

    string userDirectory(dir + "/" + foundUser->getUsername());
    if (!foundUser->deleteMessage(msgNumber, userDirectory)) // prüft ob nachricht gelöscht wurde
        return false;

    return true;
}

// gibt alle user und ihre nachrichten aus
void Database::printAllUsers()
{
    for (int i = 0; i < (int)users.size(); ++i)
    {
        cout << "USERNAME: " << users[i].getUsername() << endl;
        vector<Message> test;
        test = users[i].getAllMessages();
        for (int i = 0; i < (int)test.size(); ++i)
        {
            cout << "   message: " << test[i].getSubject() << endl;
        }
    }
}
