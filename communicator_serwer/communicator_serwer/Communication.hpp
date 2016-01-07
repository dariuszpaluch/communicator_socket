//
//  Communication.hpp
//  communicator_serwer
//
//  Created by Tomasz Tomys on 05.01.2016.
//  Copyright © 2016 Tomasz Tomys, Dariusz Paluch. All rights reserved.
//

#ifndef Communication_hpp
#define Communication_hpp

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/wait.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <unistd.h>
#include <signal.h>
#include <iostream>
#include <string>

#endif /* Communication_hpp */

class Communication {
    
    int typeOfReceived;
    std::string bufread;
 
    
public:
    
    Communication();
    void send(int fd, std::string text);
    void receive(int fd);
    int getTypeOfReceived();
    std::string getBufRead();
    
};
