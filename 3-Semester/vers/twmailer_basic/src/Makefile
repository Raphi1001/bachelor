#############################################################################################
# Makefile
#############################################################################################
# G++ is part of GCC (GNU compiler collection) and is a compiler best suited for C++
CC=g++

# Compiler Flags: https://linux.die.net/man/1/g++
#############################################################################################
# -g: produces debugging information (for gdb)
# -Wall: enables all the warnings
# -Wextra: further warnings
# -O: Optimizer turned on
# -std: c++ 17 is the latest stable version c++2a is the upcoming version
# -I: Add the directory dir to the list of directories to be searched for header files.
# -c: says not to run the linker
# -pthread: Add support for multithreading using the POSIX threads library. This option sets 
#           flags for both the preprocessor and linker. It does not affect the thread safety 
#           of object code produced by the compiler or that of libraries supplied with it. 
#           These are HP-UX specific flags.
#############################################################################################

# $^ dependencies, $@: target, % pattern

CFLAGS=-g -Wall -Wextra -O -std=c++17 -pthread

OBJDIR= obj
BINDIR= bin

SERVER = $(addprefix $(OBJDIR)/, ServerClass.o)
CLIENT = $(addprefix $(OBJDIR)/, ClientClass.o)

rebuild: clean all

all: $(BINDIR)/client $(BINDIR)/server

clean:
	clear
	rm -f $(BINDIR)/* $(OBJDIR)/*

$(OBJDIR)/%.o: %.cpp 
	$(CC) $(CFLAGS) -c $^ -o $@

$(OBJDIR)/%.o: Server/%.cpp
	$(CC) $(CFLAGS) -c $^ -o $@

$(OBJDIR)/%.o: Client/%.cpp
	$(CC) $(CFLAGS) -c $^ -o $@

$(BINDIR)/server: $(OBJDIR)/server.o $(SERVER)
	$(CC) $(CFLAGS) $^ -o $@

$(BINDIR)/client: $(OBJDIR)/client.o $(CLIENT)
	$(CC) $(CFLAGS) $^ -o $@
