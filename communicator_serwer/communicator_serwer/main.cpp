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
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <unistd.h>
#include <signal.h>
#include <iostream>
#include <string>

void childend(int signo) {
    wait(NULL);
    printf("*** END CHILD\n");
}


int main(int argc, const char * argv[]) {
    std::string buf = "Dariusz Paluch\n";
    std::string buf2 = "Unknown\n";
    std::string bufread = "";
    bufread.resize(10);
    int fd = socket(AF_INET, SOCK_STREAM, 0);
    int n,n2, on;
    struct sockaddr_in sa;
    
    signal(SIGCHLD, childend);
    
    sa.sin_family = AF_INET;
    sa.sin_port = htons(1234);
    sa.sin_addr.s_addr = INADDR_ANY;
    
    setsockopt(fd, SOL_SOCKET, SO_REUSEADDR, (char*)&on, sizeof(on));
    bind(fd, (struct sockaddr*) &sa, sizeof(sa));
    listen(fd, 5);
    
    struct sockaddr_in message;
    
    while(1) {
        int size = sizeof(message);
        int fd2 = accept(fd, (struct sockaddr*) &message, (socklen_t *) &size);
        if (!fork()) {
            close(fd);
            printf("Accept: %d", fd2);
            printf("new connection: %s:%i\n", inet_ntoa((struct in_addr)message.sin_addr), message.sin_port);
            
            read(fd2, &bufread[0], 9);
            std::cout << bufread << std::endl;
            if(bufread.compare("117225")) {
            
                write(fd2, &buf,sizeof(buf));
            } else {
                write(fd2, &buf2,sizeof(buf2));
            }
            
            
            close(fd2);
            return 0;
        }
        close(fd2);
    }
    
    //write(fd, buf, 10);
    //n = read(fd, buf, sizeof(buf));
    //buf[n] = '\0';
    //printf("%s", buf);
    
    close(fd);
    return 0;
}

