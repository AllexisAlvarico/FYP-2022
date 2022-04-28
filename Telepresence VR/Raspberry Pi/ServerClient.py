import sys
from threading import Thread
import socket
import time
import RPi.GPIO as GPIO



VERBOSE = False

IP_PORT = 8000

P_BUTTON = 18


def setup():
	GPIO.setmode(GPIO.BCM)
	GPIO.setwarnings(False)
	GPIO.setup(P_BUTTON,GPIO.OUT)

def	debug(text):
	if VERBOSE:
		print("Debug:---",text)


class SocketHandler(Thread):
	def __init__(self,conn):
		Thread.__init__(self)
		self.conn = conn

	def run(self):
		global isConnected
		debug("SocketHandler started")
		while True:
			cmd = ""
			try:
				debug("Calling blocking conn.recv()")
				cmd = str(self.conn.recv(1024).decode('utf-8'))
			except:
				debug("Exception in conn.recv()")
				print("Connection reset from the peer")
				break
			if len(cmd) == 0:
				break
			self.executeCommand(cmd)
		conn.close()
		print("Client disconnected. Waiting for next client...")
		isConnected = False
		debug("SocketHandler terminated")

	def executeCommand(self,cmd):
		self.conn.send(cmd.encode('utf-8'))

		if str(cmd[:2]) == 'on':
			print("Reporting State: ", "Camera On!") 
			#find a way to give the camera back
			#to unity
			
		elif str(cmd[:2]) == 'off':
			print("Reporting State:","Camera Off!")
			#turn off the camera
		else:
			print("Reporting State",str(cmd[:-2]))


		if str(cmd[:2]) == 'LeftCam':
			print ("Reporting State:","Moved camera left!")
		elif str(cmd[:2]) == 'RightCam':
			print ("Reporting State:","Moved camera right!")
		else:
			print ("Reporting State:",str(cmd[:-2]))

		# testing purpose
		if str(cmd[:2]) == 'Jump':
			print("Reporting State:","Player Jump")
		else:
			print ("Reporting State:",str(cmd[:-2]))



setup()

serverSocket = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
serverSocket.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)

debug("Socket created")

HOSTNAME = ""

try:
	serverSocket.bind((HOSTNAME,IP_PORT))
except socket.error as msg:
	print("Bind failed",msg[0],msg[1])
	sys.exit()

serverSocket.listen(10)

print("Waiting for a connecting client")

isConnected = False

while True:
	debug("Callubg blocking accept()...")

	conn, addr = serverSocket.accept()

	print("Connected with client at " + addr[0])

	isConnected = True

	socketHandler = SocketHandler(conn)

	socketHandler.setDaemon(True)

	socketHandler.start()

	t = 0

	while isConnected:
		print("Server connected at ",t,"s")
		time.sleep(10)
		t+=10