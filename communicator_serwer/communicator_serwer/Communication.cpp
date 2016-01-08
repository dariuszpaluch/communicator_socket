//
//  Communication.cpp
//  communicator_serwer
//
//  Created by Tomasz Tomys on 05.01.2016.
//  Copyright © 2016 Tomasz Tomys, Dariusz Paluch. All rights reserved.
//

#include "Communication.hpp"

Communication::Communication() {
    bufread.resize(5001);
};

void Communication::receive(int fd) {
    bufread.clear();
    bufread.resize(5001);
    read(fd, &bufread[0], 5000);
    bufread = static_cast<std::string>(bufread);
    typeOfReceived = bufread[0] - '0';
}

void Communication::send(int fd, std::string text) {
    write(fd, &text[0],text.size());
}

std::string Communication::getBufRead() {
    return bufread;
}

int Communication::getTypeOfReceived() {
    return typeOfReceived;
}