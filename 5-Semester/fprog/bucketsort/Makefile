all: main

.outputFolder:
	mkdir -p out

main: .outputFolder
	g++ -std=c++17 -lstdc++ -lm -Iinclude/ main.cpp -Wall -Wextra -Werror -o out/main
	./out/main
