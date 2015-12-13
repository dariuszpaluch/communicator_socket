#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <unistd.h>
#include <signal.h>

void childend(int signo) {
	wait(NULL);
	printf("*** END CHILD\n");
}

int main() {
	char buf[100] = "Dariusz Paluch\n";
	char buf2[100] = "Unknown\n";
	char bufread[100];
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
		int fd2 = accept(fd, (struct sockaddr*) &message, &size);
		if (!fork()) {
			close(fd);
			printf("Accept: %d", fd2);
			printf("new connection: %s\n", inet_ntoa((struct in_addr)message.sin_addr));

			read(fd2, bufread, sizeof(bufread));
			if (strncmp("117225",bufread, 6)==0) 
			{
				write(fd2, buf,sizeof(buf));
			}
			else 
			{
				write(fd2, buf2,sizeof(buf2));
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
