// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

//*****************************************************************************
//  DESCRIPTION     :	TCP/IP Standard Socket class.
//*****************************************************************************
#pragma warning( disable : 4127 )

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "socket.h"


#ifdef _WINDOWS

static UINT16	WinSockUsed	= 0;
static WSADATA	WsaData;

#else // Unix

#define	closesocket(xxx) close(xxx) // Unix closes the socket using close()

#ifndef INVALID_SOCKET
#define INVALID_SOCKET	-1
#endif

#endif

//>>===========================================================================

SOCKET_SOCKET_CLASS::SOCKET_SOCKET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	socketFdM = 0;		
	connectedM = false;
	listeningM = false;

#ifdef _WINDOWS
	// windows specifics
	if (!WinSockUsed)
	{
		// WinSock.dll not WSAStartup(), so call it and init the library
		UINT16 versionNeeded = 0x0202;

		int error = WSAStartup(versionNeeded, &WsaData);
		if (error)
		{
			// no socket library
			return;
		}
	}
	
	++WinSockUsed;
#endif
}

//>>===========================================================================

SOCKET_SOCKET_CLASS::SOCKET_SOCKET_CLASS(const SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr) : 
											BASE_SOCKET_CLASS(socketParams, logger_ptr)

//  DESCRIPTION     : Create a socket filling in the parameters.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	socketFdM = 0;		
	connectedM = false;
	listeningM = false;

#ifdef _WINDOWS
	// windows specifics
	if (!WinSockUsed)
	{
		// WinSock.dll not WSAStartup(), so call it and init the library
		UINT16 versionNeeded = 0x0202;

		int error = WSAStartup(versionNeeded, &WsaData);
		if (error)
		{
			// no socket library
			return;
		}
	}
	
	++WinSockUsed;
#endif
}
		
//>>===========================================================================

SOCKET_SOCKET_CLASS::SOCKET_SOCKET_CLASS(const SOCKET_SOCKET_CLASS& socket, int socketFd) : BASE_SOCKET_CLASS(socket)

//  DESCRIPTION     : Create a copy of the socket using socketFd as the connected socket.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities - the copy will be connected
	socketFdM = socketFd;		
	connectedM = true;
	listeningM = false;

#ifdef _WINDOWS
	// windows specifics
	if (!WinSockUsed)
	{
		// WinSock.dll not WSAStartup(), so call it and init the library
		UINT16 versionNeeded = 0x0202;

		int error = WSAStartup(versionNeeded, &WsaData);
		if (error)
		{
			// no socket library
			return;
		}
	}
	
	++WinSockUsed;
#endif
}
		
//>>===========================================================================

SOCKET_SOCKET_CLASS::~SOCKET_SOCKET_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
#ifdef _WINDOWS
	if (!WinSockUsed)
		return;	// must have failed installation
#endif
	
	// close the socket
	close();

#ifdef _WINDOWS
	// undo windows specials
	--WinSockUsed;
	if (!WinSockUsed)
	{
		// WSAUnhookBlockingHook();
		WSACleanup();
	}	
#endif
}

//>>===========================================================================

bool SOCKET_SOCKET_CLASS::socketParametersChanged(const SOCKET_PARAMETERS& socketParams)

//  DESCRIPTION     : Determines if the given socket parameters are different than the parameters given in the call.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (socketParams.useSecureSocketsM)
	{
		// they must have changed, since we don't use secure sockets here
		return true;
	}

	return BASE_SOCKET_CLASS::socketParametersChanged(socketParams);
}


//>>===========================================================================

bool SOCKET_SOCKET_CLASS::connect()

//  DESCRIPTION     : Set up connection to the remote host.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	struct sockaddr_in	sin;

	// make sure the socket is not already in use
	if (connectedM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Already connected to \"%s\".  Cannot connect again.", remoteHostnameM.c_str());
		}

		// return - already connected
		return false;
	}
	else if (listeningM)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket is already listening to port %d.  Cannot connect to \"%s\".", 
				localListenPortM, remoteHostnameM.c_str());
		}

		// return - socket is already being used to listen
		return false;
	}

	// initialise port
	connectedM = false;
	
	if (remoteHostnameM.length() == 0)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - No Remote Hostname defined. Can't make connection.");
		}

		// return - don't know what to connect too
		return false;
	}

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - socket::connect(%s, %d)", remoteHostnameM.c_str(), remoteConnectPortM);
	}

	// check hostname format
	if (!isdigit(remoteHostnameM.at(0)))
	{
		// try to resolve hostname to ip address
		struct hostent* he_ptr = gethostbyname(remoteHostnameM.c_str());
		if (!he_ptr)
		{
			// can't resolve hostname
			if (loggerM_ptr) 
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot resolve hostname \"%s\" to IP address", remoteHostnameM.c_str());
			}
			return false;
		}


		// set up sin
		sin.sin_family = (INT16) he_ptr->h_addrtype;
		memcpy(&sin.sin_addr, he_ptr->h_addr, he_ptr->h_length);
	}
	else
	{
		// is hostname in dot format
		UINT32 address = inet_addr(remoteHostnameM.c_str());
		if (address == 0xFFFFFFFF)
		{
			// invalid ip dot format
			if (loggerM_ptr) 
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Invalid IP address \"%s\"", remoteHostnameM.c_str());
			}
			return false;
		}
		
		// set up sin
		sin.sin_family = AF_INET;
		sin.sin_addr.s_addr = address;
	}

	// set port number
	sin.sin_port = htons(remoteConnectPortM);

	// create a socket
	socketFdM = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (socketFdM == INVALID_SOCKET)
	{
		// could not create socket
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot create socket to \"%s\"", remoteHostnameM.c_str());
		}
		socketFdM = 0;
		return false;
	}
	
	// connect to peer
	if (::connect(socketFdM, (sockaddr*) &sin, sizeof(sin)) < 0)
	{
		// could not connect to peer
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot connect to \"%s\"", remoteHostnameM.c_str());
		}
		close();

		return false;
	}
	
	// socket connected to peer
	connectedM = true;

	// return whether connected or not
	return connectedM;
}

//>>===========================================================================

bool SOCKET_SOCKET_CLASS::listen()

//  DESCRIPTION     : Setup the listen port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	struct sockaddr_in	sin;

	// make sure the socket is not already in use
	if (connectedM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Already connected to \"%s\".  Cannot listen to port %d.", 
				remoteHostnameM.c_str(), localListenPortM);
		}

		// return - already connected
		return false;
	}
	else if (listeningM)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket is already listening to port %d.  Cannot listen again.", 
				localListenPortM);
		}

		// return - socket is already being used to listen
		return false;
	}

	// initialise port number
	listeningM = false;

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - socket::listen(%d)", localListenPortM);
	}

	// create a socket
	socketFdM = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (socketFdM == INVALID_SOCKET)
	{
		// could not create socket
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot create socket to listen on port %d", localListenPortM);
		}
		socketFdM = 0;
		return false;
	}

	// set socket options
	struct linger sockarg;
	sockarg.l_onoff = 0;
	if (setsockopt(socketFdM, SOL_SOCKET, SO_LINGER, (char*) &sockarg, sizeof(sockarg)) < 0)
	{
		// can't set linger option
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot set 'linger' options for port %d", localListenPortM);
		}
		goto error;
	}

	//struct timeval tv;
	//tv.tv_sec = 30;  /* 30 Secs Timeout */
	//setsockopt(sockid, SOL_SOCKET, SO_RCVTIMEO,(struct timeval *)&tv,sizeof(struct timeval));

	{ // put in block to limit scope of reuse variable (problem with the goto)
		long reuse = 1;
		if (setsockopt(socketFdM, SOL_SOCKET, SO_REUSEADDR, (char*) &reuse, sizeof(reuse)) < 0) 
		{
			// can't set reuse opton
			if (loggerM_ptr) 
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot set 'reuse' options for port %d", localListenPortM);
			}
			goto error;
		}
	}

	// bind the socket to localhost
	sin.sin_family = AF_INET;
	sin.sin_port = htons(localListenPortM);
	sin.sin_addr.s_addr = 0;
	if (bind(socketFdM, (sockaddr*) &sin, sizeof(sin)))
	{
		// can't bind to socket
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot 'bind' socket to port %d", localListenPortM);
		}
		goto error;
	}

	// listen for connections
	if (::listen(socketFdM, SOCKET_BACKLOG))
	{
		// can't listen on socket
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot 'listen' to socket on port %d", localListenPortM);
		}
		goto error;
	}

	// listen socket set up
	listeningM = true;

	// return whether listening or not
	return listeningM;


error:
	// an error occured, cleanup and return
	closesocket(socketFdM);
	
	socketFdM = 0;
	connectedM = false;
	listeningM = false;

	return false;
}

//>>===========================================================================

bool SOCKET_SOCKET_CLASS::accept(BASE_SOCKET_CLASS** acceptedSocket_ptr_ptr)

//  DESCRIPTION     : Accept connection from listen socket.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The returned socket was new'ed so it must be deleted by the caller
//<<===========================================================================
{
	bool connected = false;
	struct sockaddr	sa;
	int	saLength;
	int acceptedSocketFd;
	*acceptedSocket_ptr_ptr = NULL;

	// make sure we are listening to the port
	if (!listeningM)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket is not listening to port %d.  Cannot accept a connection.", 
				localListenPortM);
		}

		return false;
	}

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - socket::accept()");
	}

	// accept a connection
	saLength = sizeof(sa);
	acceptedSocketFd = ::accept(socketFdM, (sockaddr*) &sa, &saLength);
	if (acceptedSocketFd != INVALID_SOCKET)
	{
		// accepted connection
		connected = true;

		// we have a new connection.  Create a socket for it.
		*acceptedSocket_ptr_ptr = new SOCKET_SOCKET_CLASS(*this, acceptedSocketFd);
	}
	else
	{
		// accept problem
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - Cannot 'accept' socket for port %d", localListenPortM);
		}
		close();
	}

	// return whether connected or not
	return connected;
}

//>>===========================================================================

void SOCKET_SOCKET_CLASS::close()

//  DESCRIPTION     : Close socket down.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - socket::close()");
	}

	// close down the socket
	if (socketFdM != 0)
	{
		closesocket(socketFdM);
	}
	
	// final cleanup
	socketFdM = 0;
	connectedM = false;
	listeningM = false;
}

//>>===========================================================================

bool SOCKET_SOCKET_CLASS::isPendingDataInNetworkInputBuffer()

//  DESCRIPTION     : Check for pending data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DWORD available;
	ioctlsocket(socketFdM, FIONREAD, &available);
	if(available > 0) 
		return true;
	else
		return false;
}

//>>===========================================================================

bool SOCKET_SOCKET_CLASS::writeBinary(const BYTE *buffer_ptr, UINT length)

//  DESCRIPTION     : Write given data to socket.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT bytes;
	bool result;

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - socket::write(%d)", length);
	}

	if (!connectedM) 
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Not connected to peer - can't write data");
		}

		return false;
	}

	if ((length == 1) && (loggerM_ptr))
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Socket 1st byte %02X", buffer_ptr[0]);
	}

	// write the buffer contents to socket
	bytes = send(socketFdM, (char*) buffer_ptr, length, 0);

	result = (bytes == length);
	if (!result && loggerM_ptr)
	{
		if (bytes == SOCKET_ERROR)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket write error, error code %d", WSAGetLastError());
		}
		else
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket did not write full buffer.  %d bytes written, %d bytes in buffer",
				bytes, length);
		}
	}

	return result;
}
		
//>>===========================================================================

INT	SOCKET_SOCKET_CLASS::readBinary(BYTE *buffer_ptr, UINT length)

//  DESCRIPTION     : Read data from socket to given buffer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Blocks until all of the requested data is read or a timeout occurs.
//<<===========================================================================
{
	struct fd_set fds;
	struct timeval tv = {socketTimeoutM, 0};
	UINT read_bytes = 0; // number of bytes read from the socket

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "TCP/IP - socket::read(%d)", length);
	}

	if (!connectedM) 
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Not connected to peer - can't read data");
		}

		return -1;
	}

	FD_ZERO(&fds);
	FD_SET(socketFdM, &fds);

	while (read_bytes < length)
	{
		int sel;

		// wait for something to read
		sel = select(socketFdM + 1, &fds, NULL, NULL, &tv);
		if (sel == 1)
		{
			INT rv = recv(socketFdM, (char *)(buffer_ptr + read_bytes), (length - read_bytes), 0);
			if (rv > 0)
			{
				// read some data
				read_bytes += rv;
			}
			else if (rv == 0)
			{
				// socket closed
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket closed during socket read");
				}
				return -1;
			}
			else if (rv == SOCKET_ERROR)
			{
				// read error
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Socket read error (error code %d)", WSAGetLastError());
				}
				return -1;
			}
			else
			{
				// unknown read error
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "TCP/IP - Unkown socket read error (read returned %d)", rv);
				}
				return -1;
			}
		}
		else if (sel == 0)
		{
			// no data at the end of the timeout
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "TCP/IP - Connection timed-out while waiting to receive data");
				return -1;
			}
		}
		else if (sel == SOCKET_ERROR)
		{
			// socket error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "TCP/IP - Error waiting to receive data (error code %d)", WSAGetLastError());
				return -1;
			}
		}
		else
		{
			// unknown error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "TCP/IP - Unkown error while waiting to receive data (select returned %d)", sel);
				return -1;
			}
		}
	}

	return read_bytes;
}


