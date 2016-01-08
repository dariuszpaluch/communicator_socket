communicator:
	g++ -c Communication.cpp -o communicator.o -Wall
	g++ -c main.cpp -o main.o -Wall
	g++ -pthread communicator.o main.o -o serwer -Wall
	
	rm -f *.o
