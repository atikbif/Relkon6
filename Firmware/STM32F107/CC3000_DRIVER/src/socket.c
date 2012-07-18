/*****************************************************************************
*
*  socket.c  - CC3000 Host Driver Implementation.
*  Copyright (C) 2011 Texas Instruments Incorporated - http://www.ti.com/
*
*  Redistribution and use in source and binary forms, with or without
*  modification, are permitted provided that the following conditions
*  are met:
*
*    Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
*
*    Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the   
*    distribution.
*
*    Neither the name of Texas Instruments Incorporated nor the names of
*    its contributors may be used to endorse or promote products derived
*    from this software without specific prior written permission.
*
*  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
*  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
*  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
*  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
*  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
*  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
*  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
*  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
*  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
*  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
*  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*
*****************************************************************************/

//*****************************************************************************
//
//! \addtogroup socket_api
//! @{
//
//*****************************************************************************

//#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "hci.h"
#include "socket.h"
#include "evnt_handler.h"


//Enable this flag if and only if you must comply with BSD socket close() function
#ifdef _API_USE_BSD_CLOSE
   #define close(sd) closesocket(sd)
#endif

//Enable this flag if and only if you must comply with BSD socket read() and write() functions
#ifdef _API_USE_BSD_READ_WRITE
              #define read(sd, buf, len, flags) recv(sd, buf, len, flags)
              #define write(sd, buf, len, flags) _send(sd, buf, len, flags)
#endif



///////////////////////////////////////////////////////////////////////////////////////////////////////////
//packed is used for preventing padding before sending the structure over the SPI                       ///
//for every IDE, exist different syntax:          1.   __MSP430FR5739__ for CCS v5                      ///
//                                                2.  __IAR_SYSTEMS_ICC__ for IAR Embedded Workbench    ///
// THIS COMMENT IS VALID FOR EVERY STRUCT DEFENITION.                                                   ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_close_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_close_args_t
#endif
{
    long sd;
}bsd_close_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_accept_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_accept_args_t
#endif
{
    long sd;
}bsd_accept_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_bind_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_bind_args_t
#endif
{
    long sd;
    long offset;
    long addrlen;
    unsigned char  addr[8];
}bsd_bind_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_listen_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_listen_args_t
#endif
{
    long sd;
    long backlog;
}bsd_listen_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_gethostbyname_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_gethostbyname_args_t
#endif
{
    long hostname_offset;
    long hostname_len;
    unsigned char hostnamebuf[1]; // Dynamic length
} bsd_gethostbyname_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_command_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_command_args_t
#endif
{
    long sd;
    long offset;
    long addrlen;
    unsigned char  addr[8];
}bsd_command_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_recvfrom_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_recvfrom_args_t
#endif
{
    long sd;
    long len;
    long flags;
}bsd_recvfrom_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_select_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_select_args_t
#endif
{
    long nfds;
    long rdoffset;
    long wroffset;
    long exoffset;
    long tooffset;
    unsigned long isblock;
    unsigned long rdfd;
    unsigned long wrfd;
    unsigned long exfd;
    unsigned long long timeout;
}bsd_select_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_getsocopt_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_getsocopt_args_t
#endif
{
    long sd;
    long level;
    long option;
}bsd_getsocopt_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_setsocopt_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_setsocopt_args_t
#endif
{
    long sd;
    long level;
    long option;
    long value_offset;
    long value_len;
    unsigned char  value[16];
}bsd_setsocopt_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) bsd_sendto_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_sendto_args_t
#endif
{
    long sd;
    long buff_offset;
    long len;
    long flags;
    long addr_offset;
    long addrlen;
}bsd_sendto_args_t;


#ifdef __CCS__
typedef struct __attribute__ ((__packed__)) _bsd_socket_args_t
#elif __IAR_SYSTEMS_ICC__
#pragma pack(1)
typedef struct _bsd_socket_args_t
#endif
{
    long domaint;
    long type;
    long protocol;
}bsd_socket_args_t;

//
// The legnth of arguments for the SEND command: sd + buff_offset + len + flags, while size of each parameter
// is 32 bit - so the total length is 16 bytes;
//
#define HCI_CMND_SEND_ARG_LENGTH	(16)


#define SELECT_TIMEOUT_MIN_MICRO_SECONDS  5000

#define HEADERS_SIZE_DATA       (SPI_HEADER_SIZE + 5)

#define SIMPLE_LINK_HCI_CMND_TRANSPORT_HEADER_SIZE  (SPI_HEADER_SIZE + SIMPLE_LINK_HCI_CMND_HEADER_SIZE)


//*****************************************************************************
//
//! int useBuff(buff_mngr_flag_t blocked)
//!
//!  \param  blocked - BUFF_MNGR_BLOCK or BUFF_MNGR_NOBLOCK
//!
//!  \return current number of free buffers, or EFAIL,
//!          if no free buffers present
//!
//!  \brief  if blocked is BUFF_MNGR_BLOCK - block until have free pages,
//!          else return EFAIL and set errno to EAGAIN
//
//*****************************************************************************
int
HostFlowControlConsumeBuff(int sd)
{
    /* wait in busy loop */
    do
    {
        //
        // In case last transmission failed then we will return the last failure reason here
        // Note that the buffer will not be allocated in this case
        //
        if (tSLInformation.slTransmitDataError != 0)
        {
            errno = tSLInformation.slTransmitDataError;
            tSLInformation.slTransmitDataError = 0;
            return errno;
        }

        if(SOCKET_STATUS_ACTIVE != get_socket_active_status(sd))
            return -1;
    } while(0 == tSLInformation.usNumberOfFreeBuffers);

    tSLInformation.usNumberOfFreeBuffers--;

    return 0;
}


//*****************************************************************************
//
//!  Get the name of the peer socket
//!
//!  @param  fd           Specifies a socket descriptor
//!  @param  addr         Specifies the destination addr
//!  @param  addrlen      Specifies the length
//!
//!  @return              If getsockname  succeeds, it returns a 0 . Otherwise,
//!                       it returns a -1 , and sets errno to indicate the type
//!                       of error.
//!
//!  @brief               function retrieves the peer address of the specified
//!                       socket, stores this address in the sockaddr structure
//!                       pointed to by the address argument, and stores the
//!                       length of this address in the object pointed to by the
//!                       address_len argument.
//
//*****************************************************************************
int
getpeername(int fd, sockaddr *addr, socklen_t *len)
{
    // TODO - fill up function
    return(ESUCCESS);
}

/**
 * \brief get host IP by name
 *
 * Obtain the IP Address of machine on network, by her name.
 *  
 * \param[in]  hostname       host name            
 * \param[in]  usNameLen      name length      
 * \param[out] out_ip_addr    This parameter is filled in with 
 *       host IP address. In case that host name is not
 *       resolved, out_ip_addr is zero.
 *
 * \return   On success, positive is returned. On error, 
 *           negative is returned
 *  
 * \sa
 * \note   On this version only, blocking mode is supported.
 * \warning
 */
int
getsockname(int fd, sockaddr *address, socklen_t *address_len)
{
   // memcpy(&(((sockaddr_in *)address)->sin_addr), &(g_Ip_Settings.ip_addr), 4);
    //*address_len = sizeof(sockaddr_in);
    
    return(ESUCCESS);
}

/**
 * \brief create an endpoint for communication
 *
 * The socket function creates a socket that is bound to a specific transport service provider.
 * This function is called by the application layer to obtain a socket handle.
 *
 * \param[in] domain            selects the protocol family which will be used for communication. On this version only AF_INET is supported
 * \param[in] type  			specifies the communication semantics. On this version only SOCK_STREAM, SOCK_DGRAM,  SOCK_RAW are supported
 * \param[in] protocol          specifies a particular protocol to be used with the socket IPPROTO_TCP, IPPROTO_UDP or IPPROTO_RAW are supported
 *
 * \return						On success, socket handle that 
 *              is used for consequent socket operations. On
 *         error, -1 is returned.
 *
 * \sa
 * \note
 * \warning
 */

int
socket(long domain, long type, long protocol)
{
    unsigned  long arg_len;
    long ret;
    unsigned char *ptr;
    bsd_socket_args_t *args;

    arg_len = 0;
    ret = EFAIL;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_socket_args_t *)(ptr + HEADERS_SIZE_CMD);
 
    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_socket_args_t);

    args->domaint = domain;
    args->protocol = protocol;
    args->type = type;

    
	//
    // Initiate a HCI command
    //
	hci_command_send(HCI_CMND_SOCKET, ptr, arg_len);

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_CMND_SOCKET, &ret);
      
    //
    // Process the event 
    //
    errno = ret;
    
    set_socket_active_status(ret, SOCKET_STATUS_ACTIVE);
    
    return(ret);
}
/**
 * \brief gracefully close socket
 *
 * The socket function closes a created socket.
 *
 * \param[in] sd                socket handle.
 *
 * \return	On success, zero is returned. On error, -1 is 
 *         returned.
 *
 * \sa socket
 * \note
 * \warning
 */
long
closesocket(long sd)
{
    unsigned  long arg_len;
    long ret;
    unsigned char *ptr;
    bsd_close_args_t *args;

    arg_len = 0;
    ret = EFAIL;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_close_args_t *)(ptr + HEADERS_SIZE_CMD);

    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_close_args_t);

    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    
   
	//
    // Initiate a HCI command
    //
	hci_command_send(HCI_CMND_CLOSE_SOCKET,
		ptr, arg_len);

	//
	// Since we are in blocking state - wait for event complete
	//
    SimpleLinkWaitEvent(HCI_CMND_CLOSE_SOCKET, &ret);
    errno = ret;

    /* since 'close' call may result in either OK (and then it closed) or error - mark this socket as invalid */
    set_socket_active_status(sd, SOCKET_STATUS_INACTIVE);

    return(ret);
}

/**
 * \brief accept a connection on a socket
 *
 * This function is used with connection-based socket types (SOCK_STREAM).
 * It extracts the first connection request on the queue of pending connections, creates a
 * new connected socket, and returns a new file descriptor referring to that socket.
 * The newly created socket is not in the listening state. The 
 * original socket sd is unaffected by this call. 
 * The argument sd is a socket that has been created with 
 * socket(), bound to a local address with bind(), and is 
 * listening for connections after a listen(). The argument \b 
 * \e addr is a pointer to a sockaddr structure. This structure 
 * is filled in with the address of the peer socket, as known to 
 * the communications layer. The exact format of the address 
 * returned addr is determined by the socket's address family. 
 * The \b \e addrlen argument is a value-result argument: it 
 * should initially contain the size of the structure pointed to 
 * by addr, on return it will contain the actual length (in 
 * bytes) of the address returned.
 *
 * \param[in] sd                socket descriptor (handle)
 * \param[out] addr             the argument addr is a pointer 
 *                              to a sockaddr structure. This
 *                              structure is filled in with the
 *                              address of the peer socket, as
 *                              known to the communications
 *                              layer. The exact format of the
 *                              address returned addr is
 *                              determined by the socket's
 *                              address\n
 *                              sockaddr:\n - code for the
 *                              address format. On this version
 *                              only AF_INET is supported.\n -
 *                              socket address, the length
 *                              depends on the code format
 * \param[out] addrlen          the addrlen argument is a 
 *       value-result argument: it should initially contain the
 *       size of the structure pointed to by addr
 *
 * \return	On success, socket handle. on failure negative
 *         value.
 *
 * \sa socket ; bind ; listen
 * \note 
 * \warning
 */
long
accept(long sd, sockaddr *addr, socklen_t *addrlen)
{
    unsigned short int arg_len;
    long ret;
    unsigned char *ptr;
    tBsdReturnParams tAcceptReturnArguments;
    bsd_accept_args_t *args;

    arg_len = 0;
    ret = EFAIL;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_accept_args_t *)(ptr + HEADERS_SIZE_CMD);

    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_accept_args_t);

    //
    // Fill in temporary command buffer
    //
    args->sd = sd;

    //
    // Initiate a HCI command
    //
	hci_command_send(HCI_CMND_ACCEPT,
		ptr, arg_len);

	//
	// Since we are in blocking state - wait for event complete
	//
    SimpleLinkWaitEvent(HCI_CMND_ACCEPT, &tAcceptReturnArguments);
	
    
	// need specify return parameters!!!
	memcpy(addr, &tAcceptReturnArguments.tSocketAddress, ASIC_ADDR_LEN);
	*addrlen = ASIC_ADDR_LEN;
    errno = tAcceptReturnArguments.iStatus; 
    ret = errno;

    /* if succeeded, iStatus = new socket descriptor. otherwise - error number (negative value ?) */
    if(M_IS_VALID_SD(ret))
	{
        set_socket_active_status(ret, SOCKET_STATUS_ACTIVE);
	}
    else
	{
        set_socket_active_status(sd, SOCKET_STATUS_INACTIVE);
	}

    return(ret);
}

/**
 * \brief assign a name to a socket
 *
 * This function gives the socket the local address addr.
 * addr is addrlen bytes long. Traditionally, this is called
 * When a socket is created with socket, it exists in a name
 * space (address family) but has no name assigned.
 * It is necessary to assign a local address before a SOCK_STREAM
 * socket may receive connections.
 *
 * \param[in] sd                socket descriptor (handle)
 * \param[in] addr              specifies the destination 
 *                              addrs\n sockaddr:\n - code for
 *                              the address format. On this
 *                              version only AF_INET is
 *                              supported.\n - socket address,
 *                              the length depends on the code
 *                              format
 * \param[in] addrlen           contains the size of the 
 *       structure pointed to by addr
 *
 * \return						On success, zero is returned. On error, -1 is returned.
 *
 * \sa socket   accept   listen
 * \note
 * \warning
 */

long
bind(long sd, const sockaddr *addr, long addrlen)
{
    unsigned short int arg_len;
    long ret;
    unsigned char *ptr;
    bsd_bind_args_t *args;

    arg_len = 0;
    ret = EFAIL;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_bind_args_t *)(ptr + HEADERS_SIZE_CMD);


    addrlen = ASIC_ADDR_LEN;

    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_bind_args_t);
   
    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->offset = 0x00000008;
    args->addrlen = addrlen;
    memcpy(args->addr, addr, addrlen);

	
	//
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_BIND,
	   ptr, arg_len);

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_CMND_BIND, &ret);

	errno = ret;
  
    return(ret);
}
/**
 * \brief listen for connections on a socket
 *
 * The willingness to accept incoming connections and a queue
 * limit for incoming connections are specified with listen(),
 * and then the connections are accepted with accept.
 * The listen() call applies only to sockets of type SOCK_STREAM
 * The backlog parameter defines the maximum length the queue of
 * pending connections may grow to. 
 *
 * \param[in] sd                socket descriptor (handle)
 * \param[in] backlog        pecifies the listen queue 
 *       depth. On this version backlog is not supported
 *
 * \return	On success, zero is returned. On 
 *             error, -1 is returned.
 *
 * \sa socket  accept  bind
 * \note On this version, backlog is not supported
 * \warning
 */

long

listen(long sd, long backlog)
{
    unsigned short int arg_len;
    long ret;
    unsigned char *ptr;
    bsd_listen_args_t *args;

    arg_len = 0;
    ret = EFAIL;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_listen_args_t *)(ptr + HEADERS_SIZE_CMD);

    
    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_listen_args_t);
    
    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->backlog = backlog;
  
	//
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_LISTEN,
	   ptr, arg_len);

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_CMND_LISTEN, &ret);
    errno = ret;
 

    return(ret);
}

/**
 * \brief get host IP by name
 *
 * Obtain the IP Address of machine on network, by its name.
 *  
 * \param[in]  hostname       host name            
 * \param[in]  usNameLen      name length      
 * \param[out] out_ip_addr    This parameter is filled in with 
 *       host IP address. In case that host name is not
 *       resolved, out_ip_addr is zero.
 *
 * \return   On success, positive is returned. On error, 
 *           negative is returned
 *  
 * \sa
 * \note   On this version only, blocking mode is supported. Also note that
 *		the function requires DNS server to be configured prior to its usage.
 * \warning
 */

int 
gethostbyname(char * hostname, unsigned short usNameLen, unsigned long* out_ip_addr)
{
    tBsdGethostbynameParams ret;
    unsigned char *ptr;
    bsd_gethostbyname_args_t *args;
    
    errno = EFAIL;

	if (usNameLen > HOSTNAME_MAX_LENGTH)
	{
        return errno;
	}
	
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_gethostbyname_args_t *)(ptr + SIMPLE_LINK_HCI_CMND_TRANSPORT_HEADER_SIZE);
    
    //
    // Fill in HCI packet structure
    //
    args->hostname_offset = 8;
    args->hostname_len = usNameLen;
    
    memcpy(args->hostnamebuf, hostname, usNameLen); 

    //
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_GETHOSTNAME, ptr, sizeof(bsd_gethostbyname_args_t) + usNameLen - 1);

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_EVNT_BSD_GETHOSTBYNAME, &ret);
	
    errno = ret.retVal;    
     (*((long*)out_ip_addr)) = ret.outputAddress;
    
    return (errno);

}
/**
 * \brief initiate a connection on a socket 
 *  
 * Function connects the socket referred to by the socket 
 * descriptor sd, to the address specified by addr. The addrlen 
 * argument specifies the size of addr. The format of the 
 * address in addr is determined by the address space of the 
 * socket. If it is of type SOCK_DGRAM, this call specifies the 
 * peer with which the socket is to be associated; this address 
 * is that to which datagrams are to be sent, and the only 
 * address from which datagrams are to be received.  If the 
 * socket is of type SOCK_STREAM, this call attempts to make a 
 * connection to another socket. The other socket is specified 
 * by address, which is an address in the com- munications space 
 * of the socket. Note that the function implements only blocking
 * bheavior thus the caller will be waiting either for the connection 
 * establishement or for the connection establishement failure.
 *  
 *  
 * \param[in] sd                socket descriptor (handle)
 * \param[in] addr              specifies the destination addr\n
 *                              sockaddr:\n - code for the
 *                              address format. On this version
 *                              only AF_INET is supported.\n -
 *                              socket address, the length
 *                              depends on the code format
 *  
 * \param[in] addrlen           contains the size of the 
 *       structure pointed to by addr
 *
 * \return   On success, zero is returned. On error, -1 is 
 *              returned
 *
 * \sa socket
 * \note
 * \warning
 */

long
connect(long sd, const sockaddr *addr, long addrlen)
{
    unsigned short int arg_len;
    long int ret;
    unsigned char *ptr;
    bsd_command_args_t *args;

    arg_len = 0;
    ret = EFAIL;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_command_args_t *)(ptr + SIMPLE_LINK_HCI_CMND_TRANSPORT_HEADER_SIZE);


    addrlen = 8;
    
    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_command_args_t);
    
    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->offset = 0x00000008;
    args->addrlen = addrlen;
    memcpy(args->addr, addr, addrlen);
    
   	//
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_CONNECT,
	   ptr, arg_len);

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_CMND_CONNECT, &ret);
	
    errno = ret;

    return((long)ret);
}

/**
 * \brief Monitor socket activity
 *  
 * Select allow a program to monitor multiple file descriptors,
 * waiting until one or more of the file descriptors become 
 * "ready" for some class of I/O operation 
 *  
 *  
 * \param[in] nfds           the highest-numbered file descriptor in any of the
 *                           three sets, plus 1.
 * \param[out] writesds      socket descriptors list for write 
 *       monitoring
 * \param[out] readsds       socket descriptors list for read 
 *       monitoring
 * \param[out] exceptsds     socket descriptors list for 
 *       exception monitoring
 * \param[in] timeout        is an upper bound on the amount of time elapsed
 *                           before select() returns. Null means infinity 
 *                           timeout. The minimum timeout is 5 milliseconds,
 *                           less than 5 milliseconds will be set
 *                           automatically to 5 milliseconds.
 *  
 * \return						On success, select()  returns the number of
 *                      file descriptors contained in the three returned
 *                      descriptor sets (that is, the total number of bits that
 *                      are set in readfds, writefds, exceptfds) which may be
 *                      zero if the timeout expires before anything interesting
 *                      happens. On error, -1 is returned.
 *                      readsds - return the sockets on which Read request will
 *                                return without delay with valid data.
 *                      writesds - return the sockets on which Write request 
 *                                 will return without delay.
 *                      exceptsds - return the sockets wich closed recently. 
 *
 * \sa socket
 * \note  If the timeout value set to less than 5ms it will 
 *  automatically set to 5ms to prevent overload of the system
 *  
 * \warning
 */

int
select(long nfds, fd_set *readsds, fd_set *writesds, fd_set *exceptsds, 
       struct timeval *timeout)
{
    unsigned short int arg_len;
    unsigned char *ptr;
    tBsdSelectRecvParams tParams;
    bsd_select_args_t *args;
    unsigned long is_blocking;
    if( timeout == NULL)
    {
        is_blocking = 1; /* blocing , infinity timeout */
    }
    else
    {
        is_blocking = 0; /* no blocking, timeout */
    }
    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_select_args_t);
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_select_args_t *)(ptr + HEADERS_SIZE_CMD);

    //
    // Fill in temporary command buffer
    //
    args->nfds = nfds;
    args->rdoffset = 0x00000014;
    args->wroffset = 0x00000014;
    args->exoffset = 0x00000014;
    args->tooffset = 0x00000014;
    args->isblock = is_blocking; /* (timeout) ? 0 : 1 */
    args->rdfd = (readsds) ? *(unsigned long*)readsds : 0;
    args->wrfd = (writesds) ? *(unsigned long*)writesds : 0;
    args->exfd = (exceptsds) ? *(unsigned long*)exceptsds : 0;

	if (timeout)
	{
        if ( 0 == timeout->tv_sec && timeout->tv_usec < SELECT_TIMEOUT_MIN_MICRO_SECONDS)
        {
            timeout->tv_usec = SELECT_TIMEOUT_MIN_MICRO_SECONDS;
        }
        
  		memcpy(&(args->timeout), timeout, sizeof(struct timeval));
    }
    
	//
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_BSD_SELECT, ptr, arg_len);
     
 	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_EVNT_SELECT, &tParams);

	//
	// Update actually read FD
	//
	if (tParams.iStatus >= 0)
	{
		if (readsds)
		{
			memcpy(readsds, &tParams.uiRdfd, sizeof(tParams.uiRdfd));
		}

		
		if (writesds)
		{
        	memcpy(writesds, &tParams.uiWrfd, sizeof(tParams.uiWrfd)); 
		}

		
		if (exceptsds)
		{
         	memcpy(exceptsds, &tParams.uiExfd, sizeof(tParams.uiExfd)); 
		}
                
                return(tParams.iStatus);

	}
	else
	{
    	  errno = tParams.iStatus;
          return(-1);
	}
}

/**
 * \brief set socket options
 *
 * This function manipulate the options associated with a socket.
 * Options may exist at multiple protocol levels; they are always
 * present at the uppermost socket level.
 *
 * When manipulating socket options the level at which the option resides
 * and the name of the option must be specified.  To manipulate options at
 * the socket level, level is specified as SOL_SOCKET.  To manipulate
 * options at any other level the protocol number of the appropriate proto-
 * col controlling the option is supplied.  For example, to indicate that an
 * option is to be interpreted by the TCP protocol, level should be set to
 * the protocol number of TCP; 
 *
 * The parameters optval and optlen are used to access optval - 
 * ues for setsockopt().  For getsockopt() they identify a 
 * buffer in which the value for the requested option(s) are to 
 * be returned.  For getsockopt(), optlen is a value-result 
 * parameter, initially contain- ing the size of the buffer 
 * pointed to by option_value, and modified on return to 
 * indicate the actual size of the value returned.  If no option 
 * value is to be supplied or returned, option_value may be 
 *  NULL.
 *  
 * \param[in] sd                socket handle
 * \param[in] level             defines the protocol level for this option
 * \param[in] optname           defines the option name to interogate
 * \param[in] optval            specifies a value for the option
 * \param[in] optlen            lspecifies the length of the 
 *       option value
 *
 * \return   On success, zero is returned. On error, -1 is 
 *            returned 
 * \sa getsockopt
 * \note On this version only SOL_SOCKET 
 *         (level) and SOCKOPT_RECV_TIMEOUT (optname) is
 *         enabled. SOCKOPT_RECV_TIMEOUT configures recv and
 *         recvfrom timeout. In that case optval should be
 *         pointer to unsigned long
 *        
 * \warning
 */

int
setsockopt(long sd, long level, long optname, const void *optval, socklen_t optlen)
{
    int ret;
    unsigned char *ptr;
    bsd_setsocopt_args_t *args;

    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_setsocopt_args_t *)(ptr + HEADERS_SIZE_CMD);

    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->level = level;
    args->option = optname;
    args->value_offset = 0x00000008;
    args->value_len = optlen;
    memcpy(args->value, optval, optlen);

    //
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_SETSOCKOPT,
	   ptr, sizeof(bsd_setsocopt_args_t) - sizeof(args->value) + optlen);

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_CMND_SETSOCKOPT, &ret);

	if (ret >= 0)
    {
    	return (0);
    }
	else
	{
		errno = ret;
		return (-1);
	}
}
/**
 * \brief get socket options
 *
 * This function manipulate the options associated with a socket.
 * Options may exist at multiple protocol levels; they are always
 * present at the uppermost socket level.
 *
 * When manipulating socket options, the level at which the option resides
 * and the name of the option must be specified.  To manipulate options at
 * the socket level, level is specified as SOL_SOCKET.  To manipulate
 * options at any other level the protocol number of the appropriate proto-
 * col controlling the option is supplied.  For example, to indicate that an
 * option is to be interpreted by the TCP protocol, level should be set to
 * the protocol number of TCP; 
 *
 * The parameters optval and optlen are used to access optval - 
 * ues for setsockopt().  For getsockopt() they identify a 
 * buffer in which the value for the requested option(s) are to 
 * be returned.  For getsockopt(), optlen is a value-result 
 * parameter, initially contain- ing the size of the buffer 
 * pointed to by option_value, and modified on return to 
 * indicate the actual size of the value returned.  If no option 
 * value is to be supplied or returned, option_value may be 
 * NULL. 
 *
 *
 * \param[in] sd                socket handle
 * \param[in] level             defines the protocol level for this option
 * \param[in] optname           defines the option name to interogate
 * \param[out] optval           specifies a value for the option
 * \param[out] optlen           specifies the length of the 
 *       option value
 *
 * \return		On success, zero is returned. On error, -1 is 
 *            returned 
 * \sa setsockopt
 * \note On this version only SOL_SOCKET 
 *         (level) and SOCKOPT_RECV_TIMEOUT (optname) is
 *         enabled. SOCKOPT_RECV_TIMEOUT configure recv and
 *         recvfrom timeout. In that case optval should be
 *         pointer to unsigned long
 * \warning
 */

int
getsockopt (long sd, long level, long optname, void *optval, socklen_t *optlen)
{
    unsigned char *ptr;
    bsd_getsocopt_args_t *args;
	tBsdGetSockOptReturnParams  tRetParams;

    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_getsocopt_args_t *)(ptr + HEADERS_SIZE_CMD);

    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->level = level;
    args->option = optname;
      
	//
	// Initiate a HCI command
	//
	hci_command_send(HCI_CMND_GETSOCKOPT,
	   ptr, sizeof(bsd_getsocopt_args_t));

	//
	// Since we are in blocking state - wait for event complete
	//
	SimpleLinkWaitEvent(HCI_CMND_GETSOCKOPT, &tRetParams);

    if (((signed char)tRetParams.iStatus) >= 0)
    {
    	*optlen = 4;
		memcpy(optval, tRetParams.ucOptValue, 4);
		return (0);
    }
	else
	{
		errno = tRetParams.iStatus;
		return (-1);
	}
}

//*****************************************************************************
//
//!  Read data from socket
//!
//!  @param sd       socket handle
//!  @param buf      read buffer
//!  @param len      buffer length
//!  @param flags    indicates blocking or non-blocking operation
//!  @param from     pointer to an address structure indicating source address
//!  @param fromlen  source address strcutre size
//!
//!  @return         Return the number of bytes received, or -1 if an error
//!                  occurred
//!
//!  @brief          Return the length of the message on successful completion.
//!                  If a message is too long to fit in the supplied buffer,
//!                  excess bytes may be discarded depending on the type of
//!                  socket the message is received from
//
//*****************************************************************************
int
simple_link_recv(long sd, void *buf, long len, long flags, sockaddr *from,
                socklen_t *fromlen, long opcode)
{
    unsigned short int arg_len;
    unsigned char *ptr;
    bsd_recvfrom_args_t *args;
	tBsdReadReturnParams tSocketReadEvent;

    arg_len = 0;
    ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_recvfrom_args_t *)(ptr + HEADERS_SIZE_CMD);
 
    //
    // Fill in HCI packet structure
    //
    arg_len = sizeof(bsd_recvfrom_args_t);

    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->len = len;
    args->flags = flags;

    //
    // Generate the read command, and wait for the 
    //
    hci_command_send(opcode,  ptr, arg_len);

    //
    // Since we are in blocking state - wait for event complete
    //
    SimpleLinkWaitEvent(opcode, &tSocketReadEvent);

    //
    // In case the number of bytes is more then zero - read data
    //
    if (tSocketReadEvent.iNumberOfBytes > 0)
    {
	//
	// Wait for the data in a synchronous way. Here we assume that the bug is big enough
	// to store also parameters of receive from too....
	//
	SimpleLinkWaitData(buf, (unsigned char *)from, (unsigned char *)fromlen);
    }

    errno = tSocketReadEvent.iNumberOfBytes;

    return(tSocketReadEvent.iNumberOfBytes);
}
/**
 * \brief read data from TCP socket
 *  
 * function receives a message from a connection-mode socket
 *  
 * \param[in] sd                socket handle
 * \param[out] buf              Points to the buffer where the 
 *       message should be stored.
 * \param[in] len               Specifies the length in bytes of 
 *       the buffer pointed to by the buffer argument.
 * \param[in] flags             Specifies the type of message 
 *       reception. On this version, this parameter is not
 *       supported.
 *
 * \return   return the number of bytes received, or -1 if an 
 *           error occurred
 *
 * \sa   recvfrom
 * \note   On this version, only blocking mode is supported.
 * \warning
 */

int
recv(long sd, void *buf, long len, long flags)
{
    return(simple_link_recv(sd, buf, len, flags, NULL, NULL, HCI_CMND_RECV));
}
/**
 * \brief read data from socket
 *
 * function receives a message from a connection-mode or
 * connectionless-mode socket. Note that raw sockets are not
 * supported.
 * 
 * \param[in] sd                socket handle 
 * \param[out] buf              Points to the buffer where the message should be stored.
 * \param[in] len               Specifies the length in bytes of the buffer pointed to by the buffer argument. 
 * \param[in] flags             Specifies the type of message
 *       reception. On this version, this parameter is not
 *       supported.
 * \param[in] from              pointer to an address structure 
 *                              indicating the source
 *                              address.\n sockaddr:\n - code
 *                              for the address format. On this
 *                              version only AF_INET is
 *                              supported.\n - socket address,
 *                              the length depends on the code
 *                              format
 * \param[in] fromlen           source address strcutre
 *       size
 * 
 *
 * \return   return the number of bytes received, 0 if timeout 
 *           occurred or 1 if an error occurred
 *
 * \sa   recv
 * \note   On this version, only blocking mode is supported.
 * \warning
 */

int
recvfrom(long sd, void *buf, long len, long flags, sockaddr *from,
         socklen_t *fromlen)
{
    return(simple_link_recv(sd, buf, len, flags, from, fromlen,
           HCI_CMND_RECVFROM));
}

//*****************************************************************************
//
//!  Send data to ASIC
//!
//!  @param sd       socket handle
//!  @param buf      write buffer
//!  @param len      buffer length
//!  @param flags    indicates blocking or non-blocking operation
//!  @param to       pointer to an address structure indicating destination
//!                  address
//!  @param tolen    destination address strcutre size
//!
//!  @return         Return the number of bytes transmited, or -1 if an error
//!                  occurred
//!
//!  @brief          This function is used to transmit a message to another
//!                  socket
//
//*****************************************************************************
int
simple_link_send(long sd, const void *buf, long len, long flags,
              const sockaddr *to, long tolen, long opcode)
{    
    unsigned char uArgSize;
    unsigned char *ptr, *pDataPtr;
    bsd_sendto_args_t *args;
    int res;

    //
    // Check the bsd_arguments
    //
    // TODO - need add checking of flags ...
    if (0 != (res = HostFlowControlConsumeBuff(sd)))
	{
        return res;
	}

    //
	// Allocate a buffer and construct a packet and send it over spi
	//
	ptr = tSLInformation.pucTxCommandBuffer;
    args = (bsd_sendto_args_t *)(ptr + HEADERS_SIZE_DATA);
  
	//
	// Update the offset of data and parameters according to the command
    switch(opcode)
    { 
        case HCI_CMND_SENDTO:
        {
			args->addr_offset = len + sizeof(len) + sizeof(len);
       		args->addrlen = 8;
			uArgSize = sizeof(bsd_sendto_args_t);
			pDataPtr = ptr + HEADERS_SIZE_DATA + sizeof(bsd_sendto_args_t);
            break;
        }
        
        case HCI_CMND_SEND:
        {
			tolen = 0;
			to = NULL;
            uArgSize = HCI_CMND_SEND_ARG_LENGTH;
			pDataPtr = ptr + HEADERS_SIZE_DATA + HCI_CMND_SEND_ARG_LENGTH;
            break;
        }
        
        case HCI_CMND_WRITE:
        {
			tolen = 0;
			to = NULL;
            uArgSize =  HCI_CMND_SEND_ARG_LENGTH - sizeof(flags);
			pDataPtr = ptr + HEADERS_SIZE_DATA + uArgSize;
            break;
        }
        
        default:
        {
        	return 0;
            break;
        }
    }

    //
    // Fill in temporary command buffer
    //
    args->sd = sd;
    args->buff_offset = uArgSize - sizeof(sd);
    args->len = len;

	//
	// Write command has no flags
	//
    if(opcode != HCI_CMND_WRITE)
    {
        args->flags = flags;
    }

	//
	// Copy the data received from user into the TX Buffer
	//
	memcpy(pDataPtr, buf, len);

	//
	// In case we are using SendTo, copy the to parameters
	//
	if (opcode == HCI_CMND_SENDTO)
	{
		pDataPtr += len;
		memcpy(pDataPtr, to, tolen);
	}
	
    //
    // Initiate a HCI command
    //
    hci_data_send(opcode, ptr, uArgSize, len,
                                         (unsigned char*)to, tolen);

    return	(len);
}


/**
 * \brief write data to TCP socket
 * 
 * This function is used to transmit a message to another socket.
 *  
 * \param[in] sd                socket handle
 * \param[in] buf               Points to a buffer containing 
 *       the message to be sent
 * \param[in] len             message size in bytes
 *
 * \return   return the number of bytes transmitted, 0 if 
 *           timeout occurred or 1 if an error occurred
 *
 * \sa   send, sendto
 * \note   On this version, only blocking mode is supported.
 * \warning
 */






/**
 * \brief write data to TCP socket
 * 
 * This function is used to transmit a message to another socket
 * (connection less socket). 
 *  
 * \param[in] sd                socket handle
 * \param[in] buf               Points to a buffer containing 
 *       the message to be sent
 * \param[in] len               message size in bytes
 * \param[in] flags             Specifies the type of message 
 *       transmission. On this version, this parameter is not
 *       supported
 * 
 *
 * \return   Return the number of bytes transmitted, or -1 if an
 *           error occurred
 *
 * \sa   sendto write
 * \note   On this version, only blocking mode is supported.
 * \warning   
 */

int
_send(long sd, const void *buf, long len, long flags)
{
    return(simple_link_send(sd, buf, len, flags, NULL, 0, HCI_CMND_SEND));
}

/**
 * \brief write data to socket
 *
 * This function is used to transmit a message to another socket
 * (connection less socket SOCK_DGRAM,  SOCK_RAW). 
 *
 * \param[in] sd                socket handle
 * \param[in] buf               Points to a buffer containing 
 *       the message to be sent
 * \param[in] len               message size in bytes
 * \param[in] flags             Specifies the type of message 
 *       transmission. On this version, this parameter is not
 *       supported 
 * \param[in] to                pointer to an address structure 
 *                              indicating the destination
 *                              address.\n sockaddr:\n - code
 *                              for the address format. On this
 *                              version only AF_INET is
 *                              supported.\n - socket address,
 *                              the length depends on the code
 *                              format
 * \param[in] tolen             destination address strcutre size 
 *
 * \return   Return the number of transmitted bytes, or
 *           -1 if an error occurred
 *
 * \sa   send write
 * \note   On this version only, blocking mode is supported.
 * \warning
 */

int
sendto(long sd, const void *buf, long len, long flags, const sockaddr *to,
       socklen_t tolen)
{
    return(simple_link_send(sd, buf, len, flags, to, tolen, HCI_CMND_SENDTO));
}
