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
#ifndef SOCKET_H
#define SOCKET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface
#include "base_socket.h"	// Socket base class interface


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************


//>>***************************************************************************

class SOCKET_SOCKET_CLASS : public BASE_SOCKET_CLASS

//  DESCRIPTION     : Class used to handle the standard (non-secure) socket interface.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	int			socketFdM;	
	bool		connectedM;
	bool		listeningM;

	SOCKET_SOCKET_CLASS(const SOCKET_SOCKET_CLASS& socket, int socketFd);

public:
	SOCKET_SOCKET_CLASS();

	SOCKET_SOCKET_CLASS(const SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr);
		
	virtual	~SOCKET_SOCKET_CLASS();		

	virtual bool socketParametersChanged(const SOCKET_PARAMETERS& socketParams);

	virtual bool connect();

	virtual bool listen();

	virtual bool accept(BASE_SOCKET_CLASS** acceptedSocket_ptr_ptr);

	virtual void close();

	virtual bool isPendingDataInNetworkInputBuffer();

	virtual	bool writeBinary(const BYTE*, UINT);
		
	virtual	INT	readBinary(BYTE*, UINT);

	virtual bool isConnected() { return connectedM; }

	virtual bool isSecure() { return false; }
};

#endif /* SOCKET_H */


