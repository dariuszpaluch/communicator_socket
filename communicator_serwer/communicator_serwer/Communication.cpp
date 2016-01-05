//
//  Communication.cpp
//  communicator_serwer
//
//  Created by Tomasz Tomys on 05.01.2016.
//  Copyright © 2016 Tomasz Tomys, Dariusz Paluch. All rights reserved.
//

#include "Communication.hpp"

Communication::Communication() {
    
};

void Communication::childend(int signo) {
    wait(NULL);
    printf("*** END CHILD\n");
}

void Communication::init() {
    fd = socket(AF_INET, SOCK_STREAM, 0);
    int n,n2, on;
    struct sockaddr_in sa;
    
    signal(SIGCHLD, childend);
    
    sa.sin_family = AF_INET;
    sa.sin_port = htons(1234);
    sa.sin_addr.s_addr = INADDR_ANY;
    
    setsockopt(fd, SOL_SOCKET, SO_REUSEADDR, (char*)&on, sizeof(on));
    bind(fd, (struct sockaddr*) &sa, sizeof(sa));
    listen(fd, 5);

};

void Communication::receive(int fd) {
     read(fd, &bufread[0], 1000);
}

void Communication::send(int fd, std::string text) {
     write(fd, &text[0],text.size());
}

int Communication::getFd() {
    return fd;
}

std::string Communication::getBufRead() {
    return bufread;
}