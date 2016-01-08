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
#include <sstream>
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

void addUser(struct cln* c) {
    struct User user;
    user.fd = c->cfd;
    int i = 0;
    for (i=0; i<=MAX_CLIENTS; i++) {
        if (users[i].fd==0 && users[i].name.size()<1) {
            users[i] = user;
            break;
        }
    }
}

int findUserByName (std::string name) {
    int i = 0;
    int clientIndex = -1;
    for (i=0; i<=MAX_CLIENTS; i++) {
        if(name.compare(users[i].name)==0) {
            clientIndex = i;
            break;
        }
    }
    
    return clientIndex;
}

int findUserByFd (int fd) {
    int i = 0;
    int clientIndex = -1;
    for (i=0; i<=MAX_CLIENTS; i++) {
        if(users[i].fd == fd) {
            clientIndex = i;
            break;
        }
    }

    return clientIndex;
}

std::string getContacts () {
    std::stringstream ss;
    int i = 0;
    for (i=0; i<=MAX_CLIENTS; i++) {
        if (users[i].name.size() > 0 ) {
            ss << users[i].name << ";";
        }
    }
    std::string s = ss.str();
    std::string st = s.substr(0, s.size()-1);
    
    return st;
}

void createUser(int fd, std::string name, std::string password) {
    int clientIndex = findUserByFd(fd);
    users[clientIndex].name = name;
    users[clientIndex].password = password;
}

int login(int fd, std::string receivedData) {
    std::string result = "0";
    std::string delimiter = ";";
    std::string endChar = "|";
    size_t pos = 0;
    std::string token;
    
    pos = receivedData.find(delimiter);
    receivedData.erase(0, pos + delimiter.length());
    pos = receivedData.find(delimiter);
    std::string name = receivedData.substr(0, pos);
    receivedData.erase(0, pos + delimiter.length());
    pos = receivedData.find(endChar);
    std::string password = receivedData.substr(0, pos);
    
    int clientIndex = findUserByName(name);
    if(clientIndex == -1) {
        createUser(fd, name, password);
        return 1;
    }
    std::cout<<name << "--" << users[clientIndex].name << std::endl;
    std::cout <<password << "--" << users[clientIndex].password << std::endl;
    if((name.compare(users[clientIndex].name) == 0) &&
       (password.compare(users[clientIndex].password) == 0)) {
        users[clientIndex].fd = fd;
        int i = 0;
        for(i=0; i <= MAX_CLIENTS; i++) {
            if(users[i].fd == fd && users[i].name.size() == 0) {
                users[i].fd = 0;
            }
        }
        return 1;
    }
    return 0;
}

int sendMessage(int fd, Communication *communication) {
    std::string receivedData = communication->getBufRead();
    std::string result = "0";
    std::string delimiter = ";";
    std::string endChar = "|";
    size_t pos = 0;
    std::string token;
    
    pos = receivedData.find(delimiter);
    receivedData.erase(0, pos + delimiter.length());
    pos = receivedData.find(delimiter);
    std::string receiverName = receivedData.substr(0, pos);
    receivedData.erase(0, pos + delimiter.length());
    pos = receivedData.find(endChar);
    std::string message = receivedData.substr(0, pos);
    std::string time = "25.11.1995 18:00";
    int senderIndex = findUserByFd(fd);
    std::string senderName = users[senderIndex].name;
    
    int receiverIndex = findUserByName(receiverName);
    int receiverFd = users[receiverIndex].fd;
    
    std::stringstream ss;
    ss << "3;" << senderName << ";" << time << ";" << message << "|";
    std::string text = ss.str();
    
    std::cout << "send: " << text << std::endl;
    communication->send(receiverFd, text);
    
    return 1;
}

void* cthread(void* arg) {
    struct cln* c = (struct cln*) arg;
    printf("New connection: %s\n", inet_ntoa((struct in_addr)c->caddr.sin_addr));
    Communication *communication = new Communication();
    std::string text = "";
    addUser(c);
    int active = 1;
    while(active) {
        communication->receive(c->cfd);
        std::cout << "received: " << communication->getBufRead() <<std::endl;
        switch (communication->getTypeOfReceived()) {
            case TYPE_LOGIN:
            {
                int result = login(c->cfd, communication->getBufRead());
                if (result == 1) {
                    text = "1;1|";
                    std::cout << "send: " << text << std::endl;
                    communication->send(c->cfd, text);
                    
                    std::stringstream ss;
                    ss << "2;" << getContacts() << "|";
                    std::string contacts = ss.str();
                    int i = 0;
                    for (i=0; i<=MAX_CLIENTS; i++) {
                        if (users[i].fd!=0) {
                            std::cout << "send: " << contacts << std::endl;
                            communication->send(users[i].fd, contacts);
                        }
                    }
                    break;
                }
                if (result == 0) {
                    text = "1;0;Wrong password.|";
                    std::cout<< "send: " << text <<std::endl;
                    communication->send(c->cfd, text);
                    break;

                }
                text = "1;0;Unexpected error of server.|";
                std::cout << "send: " << text << std::endl;
                communication->send(c->cfd, text);
                break;
            }
            case TYPE_SEND_MSG:
            {
                int result = sendMessage(c->cfd, communication);
                break;
            }
            case TYPE_LOGOUT:
            {
                int clientIndex = findUserByFd(c->cfd);
                users[clientIndex].fd = 0;
                std::string text = "4;1|";
                std::cout << "send: " << text << std::endl;
                communication->send(c->cfd, text);
                active = 0;
                break;
            }
            default:
                break;
        }
    }
    close(c->cfd);
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