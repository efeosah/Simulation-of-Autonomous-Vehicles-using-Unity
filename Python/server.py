from io import BytesIO
import io
import socket
import struct
import traceback
import logging
import time
import base64


from PIL import Image
from numpy import bytes_

def sending_and_reciveing():
    s = socket.socket()
    socket.setdefaulttimeout(None)
    print('socket created ')
    port = 60000
    s.bind(('127.0.0.1', port)) #local host
    s.listen(30) #listening for connection for 30 sec?
    print('socket listensing ... ')
    while True:
        try:
            c, addr = s.accept() #when port connected
            bytes_received = c.recv(4000000) #received bytes
            print(bytes_received)
            #array_received = np.frombuffer(bytes_received, dtype=np.float32) #converting into float array

            #nn_output = return_prediction(array_received) #NN prediction (e.g. model.predict())

            #bytes_to_send = struct.pack('%sf' % len(nn_output), *nn_output) #converting float to byte
            # image = Image.open(BytesIO(base64.b64decode(bytes_received)))
            image = Image.open(BytesIO(bytes_received))
            image.show()
            c.sendall(bytes_received) #sending back
            # c.close()
        except Exception as e:
            logging.error(traceback.format_exc())
            print("error")
            c.sendall("recieved")
            c.close()
            break

sending_and_reciveing() 