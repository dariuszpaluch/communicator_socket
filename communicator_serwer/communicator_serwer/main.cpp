//
//  main.cpp
//  communicator_serwer
//
//  Created by Tomasz Tomys on 05.01.2016.
//  Copyright © 2016 Tomasz Tomys, Dariusz Paluch. All rights reserved.
//

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

#include "Communication.hpp"

int main(int argc, const char * argv[]) {
    std::string buf = "3;FajnyNick;05.01.2016 21:44;Jestem taki fajnt fajny fajny\na to druga linia mojej fajnosci";
    std::string buf2 = "1;tokenniksdks";
    
    Communication *communication = new Communication();
    communication->init();

    struct sockaddr_in message;
    
    while(1) {
        int size = sizeof(message);
        int fd2 = accept(communication->getFd(), (struct sockaddr*) &message, (socklen_t *) &size);
        if (!fork()) {
            close(communication->getFd());

            printf("Accept: %d", fd2);
            printf("new connection: %s:%i\n", inet_ntoa((struct in_addr)message.sin_addr), message.sin_port);
            
           
            communication->receive(fd2);
            std::cout << "Received data: " << communication->getBufRead() << std::endl;
            std::cout << "Type of received data: " << communication->getTypeOfReceived() << std::endl;
            switch (communication->getTypeOfReceived()) {
                case 1:
                    communication->send(fd2, buf2);
                    break;
                case 3:
                    communication->send(fd2, buf);
                    break;
                    
                default:
                    break;
            }
            //std::cout << bufread << std::endl;
            //if(bufread.compare("117225")) {
            
           
           // } else {
              //  write(fd2, &buf2[0],buf2.size());
            //}
            
            
            close(fd2);
            return 0;
        }
        close(fd2);
    }
    
    //write(fd, buf, 10);
    //n = read(fd, buf, sizeof(buf));
    //buf[n] = '\0';
    //printf("%s", buf);
    
    close(communication->getFd());
    return 0;
}