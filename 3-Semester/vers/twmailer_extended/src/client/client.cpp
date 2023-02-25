#include "client.h"

/// Helperfunc
void Client::print_usage()
{
    std::cout << "Usage: ./twmailer-client <ip> <port>" << std::endl;
    exit(EXIT_FAILURE);
}

/// RichtierCode

void Client::readInput(int argc, char *argv[])
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
        ip_s = argv[optind];
        optind++;
        std::string port_s(argv[optind]);
        port = std::stoi(port_s);
    }
    catch (...)
    {
        print_usage();
        exit(EXIT_FAILURE);
    }
}

void Client::setupSocket()
{
    socket_fd = socket(AF_INET, SOCK_STREAM, 0);

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(port);
    server_addr.sin_addr.s_addr = inet_addr(ip_s.c_str());

    connectionCode = connect(socket_fd, (struct sockaddr *)&server_addr, sizeof(server_addr));

    if (connectionCode == -1)
    {
        perror("bind error");
        exit(EXIT_FAILURE);
    }
}

void Client::userLogin()
{
    std::string s;

    std::cout << "Username: ";
    std::getline(std::cin, s);

    while (!msg.setSender(s))
    {
        std::cout << "Ungültige Eingabe, keine Sonderzeichen und weniger als 8 Zeichen: ";
        s = "";
        std::getline(std::cin, s);
    }
}

SendOption Client::getOptions()
{
    std::string option;

    std::cout << std::endl
              << "Optionen: " << std::endl
              << "  (0) LOGIN" << std::endl
              << "  (1) SEND" << std::endl
              << "  (2) LIST" << std::endl
              << "  (3) READ" << std::endl
              << "  (4) DEL" << std::endl
              << "  (5) QUIT" << std::endl
              << std::endl
              << "Deine Eingabe: ";
    std::getline(std::cin, option);

    while (option.length() != 1 || option[0] < 48 || option[0] > 53)
    {
        std::cout << "Keine Gültige Option: ";
        std::getline(std::cin, option);
    }

    switch (option[0])
    {
    case '0':
        return SendOption::LOGIN;
    case '1':
        return SendOption::SEND;

    case '2':
        return SendOption::LIST;

    case '3':
        return SendOption::READ;

    case '4':
        return SendOption::DEL;

    case '5':
        return SendOption::QUIT;
    }
    return SendOption::QUIT;
}

void Client::startOption(SendOption input)
{
    msg.cleanMsg();

    switch (input)
    {
    case SEND:
        setSEND();
        msg.createMsgString();
        sendServer();
        break;

    case LIST:
        msg.setMessageHead(LIST);
        msg.createMsgString();
        sendServer();
        waitListAnswer();
        break;

    case READ:
        msg.setMessageHead(READ);
        setMsgNrClient();
        msg.createMsgString();
        sendServer();
        waitServerRespons();
        waitReadAnswer();
        break;

    case DEL:
        msg.setMessageHead(DEL);
        setMsgNrClient();
        msg.createMsgString();
        sendServer();
        waitServerRespons();
        break;

    case QUIT:
        msg.setMessageHead(QUIT);
        msg.createMsgString();
        sendServer();
        break;

    case LOGIN:
        setLOGIN();
        msg.createMsgString();
        sendServer();
        waitServerRespons();
        break;
    }
}

void Client::waitServerRespons()
{
    int i = 0;

    std::cout << "Warte auf Server response: " << std::endl;

    do
    {
        i = recv(socket_fd, dataReceiving, sizeof(dataReceiving) - 1, 0);
        dataReceiving[i] = 0;
        if (fputs(dataReceiving, stdout) == EOF)
        {
            std::cout << "Standard output error";
            break;
        }
        if (dataReceiving[i] == '\n')
            break;
    } while (i == 2048);

    if (i < 0)
    {
        std::cout << "Standard output error" << std::endl;
    }
}

void Client::setLOGIN()
{
    msg.setMessageHead(LOGIN);

    std::string tmp;
    std::cout << "Passwort: ";
    std::getline(std::cin, tmp);
    msg.setPassword(tmp);
}

void Client::setSEND()
{
    std::string tmp;

    msg.setMessageHead(SEND);

    tmp = "";

    std::cout << "Empfänger: ";
    std::getline(std::cin, tmp);
    while (!msg.setReceiver(tmp))
    {
        std::cout << "Ungültige Eingabe, versuche es erneut: ";
        std::getline(std::cin, tmp);
    }

    std::cout << "Betreff: ";
    std::getline(std::cin, tmp);
    while (!msg.setSubject(tmp))
    {
        std::cout << "Ungültige Eingabe, versuche es erneut: ";
        std::getline(std::cin, tmp);
    }
    std::cout << "Nachricht: ";

    while (1)
    {
        tmp = "";
        std::string message = "";
        while (getline(std::cin, tmp))
        {
            message.append(tmp + '\n');

            if (message.substr(message.size() - 3) == "\n.\n")
            {
                break;
            }
        }
        if (msg.setMessageContent(message))
            break;
        std::cout << "Ungültige Eingabe, versuche es erneut: ";
    }
}

void Client::setMsgNrClient()
{
    std::string tmp;
    bool isValid = false;

    std::cout << "NachrichtenNr: ";
    std::getline(std::cin, tmp);
    while (!isValid)
    {
        try
        {
            while (!msg.setMessageNumber(stoi(tmp)))
            {
                std::cout << "Ungültige Eingabe, versuche es erneut: ";
                std::getline(std::cin, tmp);
            }
            isValid = true;
        }
        catch (...)
        {
        }
    }
}

void Client::sendServer()
{
    int i;

    std::string msgString = msg.getMessageString();

    if ((i = send(socket_fd, msgString.c_str(), sizeof(msgString), 0)) == -1)
    {
        std::cout << "Fehler beim senden!" << std::endl;
        exit(EXIT_FAILURE);
    }

    i == -1 ? std::cout << "Senden war nicht erfolgreich!" << std::endl : std::cout << "Senden war erfolgreich!" << std::endl;
}

void Client::waitReadAnswer()
{
    int rec;
    std::string tmp = "";
    do
    {
        if ((rec = recv(socket_fd, dataReceiving, sizeof(dataReceiving), 0)) == -1)
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
            tmp += '\n';
        }

    } while (rec == 2048);

    std::cout << tmp << std::endl;
}

void Client::waitListAnswer()
{
    int rec;
    std::string tmp = "";
    do
    {
        if ((rec = recv(socket_fd, dataReceiving, sizeof(dataReceiving), 0)) == -1)
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
            tmp += '\n';
        }
    } while (rec == 2048);

    std::cout << tmp << std::endl;
}