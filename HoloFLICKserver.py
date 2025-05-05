# -*- coding: utf-8 -*-
"""
Created on Mon May  5 19:00:47 2025

This script starts a multi-threaded TCP server that listens for incoming client connections 
on given 5000. It extracts the stream name and marker from the data and sends the marker as 
a sample through an LSL stream. The server supports clean shutdowns and logs connection activity.


@author: Abin Jacob
         Carl von Ossietzky University Oldenburg
         abin.jacob@uni-oldenburg.de
"""

import socket
import threading
from datetime import datetime
import struct
from pylsl import StreamInfo, StreamOutlet

# address and port
HOST = '0.0.0.0'
PORT = 5000

# thread-safe flag to control server state
server_running = threading.Event()
server_running.set()

# set up LSL stream
info = StreamInfo('MarkerStream', 'Markers', 1, 0, 'string', 'message_stream_id')
outlet = StreamOutlet(info)

def clientHandler(conn, addr):
    print(f"[NEW CONNECTION] {addr} connected.")

    # periodic checks for connection shutdown
    conn.settimeout(1.0) 
    while server_running.is_set():
        try:
            # receive data
            data = conn.recv(1024)
            if not data:
                break
            # fetch stream name 
            streamName = data[13:25].decode('ascii')
            # check stream type
            if streamName == 'MarkerStream':
            # fetch marker
                markerText = data[29:32].decode('ascii')
                print(f"[{datetime.now()}] [{streamName}]: {markerText}")
                outlet.push_sample([f"{markerText}"])
                
        except socket.timeout:
            continue  # Check for shutdown flag again
        except ConnectionResetError:
            print(f"[DISCONNECTED] {addr} forcibly closed the connection.")
            break
        except Exception as e:
            print(f"[ERROR] {e}")
            break

    # close connection
    conn.close()
    print(f"[CONNECTION CLOSED] {addr}")

def startServer():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind((HOST, PORT))
    server.listen()
    print(f"[STARTING] TCP server on {HOST}:{PORT}")
    try:
        while server_running.is_set():
            server.settimeout(1.0)
            try:
                conn, addr = server.accept()
                thread = threading.Thread(target=clientHandler, args=(conn, addr), daemon=True)
                thread.start()
                print(f"[ACTIVE CONNECTIONS] {threading.active_count() - 1}")
            except socket.timeout:
                continue
    except KeyboardInterrupt:
        print("\n[SERVER SHUTDOWN] Interrupt received, shutting down...")
        server_running.clear()
    finally:
        server.close()
        print("[SERVER CLOSED]")

if __name__ == "__main__":
    startServer()
