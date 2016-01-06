//
//  main.cpp
//  communicator_serwer
//
//  Created by Tomasz Tomys on 05.01.2016.
//  Copyright © 2016 Tomasz Tomys, Dariusz Paluch. All rights reserved.
//

#include <iostream>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/time.h>
#include <sys/param.h>
#include <netinet/in.h>
#include <netdb.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <arpa/inet.h>
#include <sys/select.h>
#include <unistd.h>
#include <signal.h>
#include <sys/wait.h>
#include <pthread.h>

#include "Communication.hpp"

#define MAX_CLIENTS 1000
#define MAX_MSG_LENGTH 5000

#define TYPE_LOGIN 1
#define TYPE_GET_CONTACTS 2
#define TYPE_SEND_MSG 3
#define TYPE_LOGOUT 4

struct cln {
    int cfd;
    struct sockaddr_in caddr;
    int index;
};

struct User {
    std::string name;
    std::string password;
    int fd;
};

struct User users[MAX_CLIENTS];

int clientsCount = 0;
pthread_t client_threads[MAX_CLIENTS];

void* cthread(void* arg) {
    struct cln* c = (struct cln*) arg;
    printf("New connection: %s\n", inet_ntoa((struct in_addr)c->caddr.sin_addr));
    Communication *communication = new Communication();
    
    while(1) {
        communication->receive(c->cfd);
        std::cout<<communication->getBufRead() <<std::endl;
        std::string text = "1;0;problem z polaczeniem";
        switch (communication->getTypeOfReceived()) {
            case TYPE_LOGIN:
                communication->send(c->cfd, text);
                break;
            case TYPE_GET_CONTACTS:
                break;
            case TYPE_SEND_MSG:
                break;
            case TYPE_LOGOUT:
                break;
            default:
                break;
        }
    }
    return 0;
}

int main(int argc, char** argv) {
    socklen_t slt;
    int sfd, on=1;
    
    struct sockaddr_in saddr;
    saddr.sin_family = AF_INET;
    saddr.sin_port = htons(1234);
    saddr.sin_addr.s_addr = INADDR_ANY;
    
    sfd = socket(AF_INET, SOCK_STREAM, 0);
    
    setsockopt(sfd, SOL_SOCKET, SO_REUSEADDR, (char*)&on, sizeof(on));
    bind(sfd, (struct sockaddr*) &saddr, sizeof(saddr));
    
    listen(sfd, 5);
    
    while(1) {
        int i = 0;
        for(i=0; i<MAX_CLIENTS; i++)
        {
            if(client_threads[i] == 0)
                break;
        }
        
        struct cln* c = (struct cln*)malloc(sizeof(struct cln));
        slt = sizeof(c->caddr);
        c->cfd = accept(sfd, (struct sockaddr*)&c->caddr, &slt);
        c->index = i;
        pthread_create(&client_threads[i], NULL, cthread, c);
    }
    
    return 0;
}