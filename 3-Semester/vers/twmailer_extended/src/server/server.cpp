#include "server.h"
#include <ldap.h>

using namespace std;

/// Helperfuncs

void Server::print_usage()
{
    std::cout << "Usage: ./twmailer-server <port> <mail-spool-directoryname>" << std::endl;
    exit(EXIT_FAILURE);
}

/// RichtigerCode

void Server::setupSocket()
{
    clientList = socket(AF_INET, SOCK_STREAM, 0);

    memset(&my_addr, 0, sizeof(my_addr));
    my_addr.sin_family = AF_INET;
    my_addr.sin_port = htons(port);
    my_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    connectionCode = bind(clientList, (struct sockaddr *)&my_addr, sizeof(my_addr));

    if (connectionCode == -1)
    {
        perror("bind error");
        exit(EXIT_FAILURE);
    }
}

void Server::readInput(int argc, char *argv[])
{
    int c;
    bool error = false;

    std::string program_name = argv[0];

    while ((c = getopt(argc, argv, ":")) != EOF && !error)
    {
        switch (c)
        {
        case '?':
            error = true;
            break;

        default:
            assert(0);
            break;
        }
    }

    if (error || argc < optind + 2)
    {
        print_usage();
        exit(EXIT_FAILURE);
    }

    try
    {
        std::string s(argv[optind]);
        port = std::stoi(s);
        optind++;
        dir = argv[optind];
    }
    catch (...)
    {
        print_usage();
        exit(EXIT_FAILURE);
    }

    database.setDir(dir);
}

void Server::listenToClient()
{
    while (connectionCode == 0)
    {

        if (listen(clientList, 1) == -1)
        {
            perror("listen error");
            exit(EXIT_FAILURE);
        }

        std::cout << "Warte auf Client" << std::endl;
        if ((clientConnect = accept(clientList, (struct sockaddr *)NULL, NULL)) == -1)
        {
            std::cout << "Es ist ein Fehler beim listen aufgetreten" << std::endl;
            exit(EXIT_FAILURE);
        }

        std::cout << "Server und Client wurden erfolgreich verbunden!" << std::endl;
        snprintf(dataSending, sizeof(dataSending), "Du wurdest erfolgreich verbunden!\n");
        write(clientConnect, dataSending, strlen(dataSending));

        while (clientConnect > 0)
        {
            msg.cleanMsg();
            if (msg.setMessageHead(reciveClient()))
                workWithMsgHead();
            else
                sendAnswer(false);
        }
    }
}

std::string Server::reciveClient()
{
    int rec;
    std::string tmp;

    std::cout << "Warte auf Client send" << std::endl;
    do
    {
        if ((rec = recv(clientConnect, dataReceiving, sizeof(dataReceiving), 0)) == -1)
        {
            std::cout << "Es ist ein Fehler beim recive aufgetreten" << std::endl;
            break;
        }
        else if (rec == 0)
        {
            std::cout << "Remote socket wurde geschlossen" << std::endl;
            break;
        }
        else
        {
            tmp += dataReceiving;
        }
    } while (rec == 2048);

    tmp.pop_back();
    std::cout << tmp << std::endl;
    return tmp;
}

// nimmt keinen switch
void Server::workWithMsgHead()
{
    if (msg.getMessageHead() == "SEND" && islogedIn)
    {
        setMsgSEND();
        if (database.sendMessage(msg.getSender(), msg.getReceiver(), msg.getSubject(), msg.getMessageContent()))
        {
            std::cout << "ok" << std::endl;
            sendAnswer(true);
        }
        else
        {
            std::cout << "nicht ok" << std::endl;
            sendAnswer(false);
        }
    }
    else if (msg.getMessageHead() == "LIST" && islogedIn)
    {
        User *user = database.getUser(msg.getSender());
        if (user)
        {

            snprintf(dataSending, sizeof(dataSending), "OK\n");

            write(clientConnect, dataSending, strlen(dataSending));

            std::vector<Message> messages = user->getAllMessages();

            sendSendAnswer(std::to_string(messages.size()));
            for (int i = 0; i < (int)messages.size(); ++i)
            {
                sendSendAnswer(messages[i].getSubject());
            }
        }
        else
        {
            sendSendAnswer("0");
        }
    }
    else if (msg.getMessageHead() == "READ" && islogedIn)
    {
        setUser();
        setMsgNr();

        Message *answere = database.getUserMessage(msg.getSender(), msg.getMessageNumber());
        if (!answere)
        {
            std::cout << "nachricht oder user nicht gefunden" << std::endl;
            sendAnswer(false);
            return;
        }

        sendReadAnswer(answere);
    }
    else if (msg.getMessageHead() == "DEL" && islogedIn)
    {
        setUser();
        setMsgNr();
        if (database.deleteUserMessage(msg.getSender(), msg.getMessageNumber()))
        {
            std::cout << "gelöscht" << std::endl;
            sendAnswer(true);
        }
        else
        {
            std::cout << "nicht gelöscht" << std::endl;
            sendAnswer(false);
        }
    }
    else if (msg.getMessageHead() == "LOGIN")
    {
        setUser();
        msg.setPassword(reciveClient());
        sendAnswer(loginLDAP());
    }
    else
        close(clientConnect);
}

void Server::sendAnswer(bool answer)
{
    if (answer)
        snprintf(dataSending, sizeof(dataSending), "OK\n");
    else
        snprintf(dataSending, sizeof(dataSending), "ERR\n");

    write(clientConnect, dataSending, strlen(dataSending));
}

void Server::setMsgSEND()
{
    if (
        msg.setReceiver(reciveClient()) &&
        msg.setSubject(reciveClient()) &&
        msg.setMessageContent(reciveClient()) &&
        reciveClient() == ".")
        std::cout << "Msg wurde erfolgreich verarbeitet" << std::endl;
    else
        std::cout << "Msg wurde nicht erfolgreich verarbeitet" << std::endl;
}

void Server::setMsgNr()
{
    try
    {
        if (!msg.setMessageNumber(stoi(reciveClient())))
            std::cout << "MsgNr wurde nicht erfolgreich verarbeitet" << std::endl;
    }
    catch (...)
    {
        std::cout << "Msg wurde nicht erfolgreich verarbeitet" << std::endl;
    }
}

void Server::setUser()
{
    if (!msg.setSender(reciveClient()))
        std::cout << "Sender wurde nicht erfolgreich verarbeitet" << std::endl;
}

void Server::sendReadAnswer(Message *answere)
{
    int i;
    string str = answere->getMessageString();

    if ((i = send(clientConnect, str.c_str(), sizeof(str), 0)) == -1)
    {
        std::cout << "Fehler beim senden!" << std::endl;
        exit(EXIT_FAILURE);
    }
    sleep(1);
    i == -1 ? std::cout << "Senden war nicht erfolgreich!" << std::endl : std::cout << "Senden war erfolgreich!" << std::endl;
}

void Server::sendSendAnswer(std::string str)
{
    int i;
    std::cout << str << std::endl;

    if ((i = send(clientConnect, str.c_str(), sizeof(str), 0)) == -1)
    {
        std::cout << "Fehler beim senden!" << std::endl;
        exit(EXIT_FAILURE);
    }
    sleep(1);
    i == -1 ? std::cout << "Senden war nicht erfolgreich!" << std::endl : std::cout << "Senden war erfolgreich!" << std::endl;
}

bool Server::loginLDAP()
{
    const int ldapVersion = LDAP_VERSION3;
    std::string ldapUri = "ldap://ldap.technikum-wien.at:389";
    std::string ldapBindUser = "uid=" + msg.getSender() + ",ou=people,dc=technikum-wien,dc=at";
    int rc = 0;
    LDAP *ldapHandle;

    rc = ldap_initialize(&ldapHandle, ldapUri.c_str());

    if (rc != LDAP_SUCCESS)
    {
        std::cout << "ldap_init failed" << std::endl;
        return false;
    }

    rc = ldap_set_option(
        ldapHandle,
        LDAP_OPT_PROTOCOL_VERSION,
        &ldapVersion);

    if (rc != LDAP_OPT_SUCCESS)
    {
        std::cout << "ldap_set_option(PROTOCOL_VERSION): " << ldap_err2string(rc) << std::endl;
        ldap_unbind_ext_s(ldapHandle, NULL, NULL);
        return false;
    }

    rc = ldap_start_tls_s(
        ldapHandle,
        NULL,
        NULL);

    if (rc != LDAP_SUCCESS)
    {
        std::cout << "ldap_start_tls_s(): " << ldap_err2string(rc) << std::endl;
        ldap_unbind_ext_s(ldapHandle, NULL, NULL);
        return false;
    }

    BerValue bindCredentials;
    bindCredentials.bv_val = (char *)msg.getPassword().c_str();
    bindCredentials.bv_len = strlen(msg.getPassword().c_str());
    BerValue *servercredp;

    rc = ldap_sasl_bind_s(
        ldapHandle,
        ldapBindUser.c_str(),
        LDAP_SASL_SIMPLE,
        &bindCredentials,
        NULL,
        NULL,
        &servercredp);

    if (rc != LDAP_SUCCESS)
    {
        std::cout << "LDAP bind error: " << ldap_err2string(rc);
        ldap_unbind_ext_s(ldapHandle, NULL, NULL);
        return false;
    }

    islogedIn = true;
    return true;
}