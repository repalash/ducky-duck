import socket
HOST = '0.0.0.0'    
PORT = 9126         
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(1)
conn, addr = s.accept()

print 'Phone connected from ', addr

s2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s2.bind((HOST, PORT+1))
s2.listen(1)
conn2, addr = s2.accept()
print 'Screen connected from ', addr

while 1:
	data = conn.recv(1024).split('|')[1:-1]
	if not data:
		break;
	x = 0.
	y= 0.
	n=0
	print data
	for i in data:
		if 'S' in i:
			conn2.send("Shoot")
		else:
			i = i.split(',')
			x += float(i[0])
			y += float(i[1])
			n+=1
	if n!=0:
		conn2.send(str(x/n)+","+str(y/n))
		print "Sent: ", str(x/n)+","+str(y/n)

conn.close()