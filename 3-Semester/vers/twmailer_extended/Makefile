all: clean client server

client: ./src/client/clientMain.cpp
	g++ -std=c++17 -Wall -Werror -o ./bin/client ./src/client/clientMain.cpp ./src/client/client.cpp ./src/client/client.h ./src/shared/message.cpp ./src/shared/message.h ./src/shared/functions.cpp ./src/shared/functions.h

server: ./src/server/serverMain.cpp
	g++ -std=c++17 -Wall -Werror -o ./bin/server ./src/server/serverMain.cpp ./src/server/server.cpp ./src/server/server.h ./src/server/user.cpp ./src/server/user.h ./src/server/database.cpp ./src/server/database.h ./src/shared/message.cpp ./src/shared/message.h ./src/shared/functions.cpp ./src/shared/functions.h -pthread -lldap -llber

clean:
	rm -f bin/*
