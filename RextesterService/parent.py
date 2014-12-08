#!/usr/bin/env python

import os
import sys
import resource
import subprocess
import time
import signal
import select
#import codecs
#import pwd
#import grp


#import getpass
#import pwd
#os.setuid(1001)
#print getpass.getuser()


#print pwd.getpwuid(os.getuid())[0]

#os.setuid(pwd.getpwnam('nobody').pw_uid)
#os.setgid(grp.getgrnam('nogroup').gr_gid)

#import datetime
#f = open('/home/ren/Desktop/time.txt', 'a')
#f.write('python parent - ' + str(datetime.datetime.now()))
#f.close()
def setlimits(compiler):

		if compiler[0].startswith("octave") or compiler[0] == "R":
			resource.setrlimit(resource.RLIMIT_CPU, (15, 15))
		else:
			resource.setrlimit(resource.RLIMIT_CPU, (5, 5))

		resource.setrlimit(resource.RLIMIT_CORE, (0, 0))
		resource.setrlimit(resource.RLIMIT_DATA, (100000000, 100000000))
		
		if compiler[0].startswith("octave") or compiler[0] == "R":
			resource.setrlimit(resource.RLIMIT_FSIZE, (1000000, 1000000))
		else:
			resource.setrlimit(resource.RLIMIT_FSIZE, (0, 0))		
		resource.setrlimit(resource.RLIMIT_MEMLOCK, (0, 0))		
		resource.setrlimit(resource.RLIMIT_NOFILE, (30, 30))
		resource.setrlimit(resource.RLIMIT_NPROC, (500, 500))
		resource.setrlimit(resource.RLIMIT_STACK, (100000000, 100000000))		
		
		if compiler[0].startswith("octave") or compiler[0] == "R":
			resource.setrlimit(resource.RLIMIT_AS, (4000000000, 40000000000))
		else:
			resource.setrlimit(resource.RLIMIT_AS, (1500000000, 1500000000))		
		
		#resource.setrlimit(resource.RLIMIT_LOCKS, (10, 10))
		#resource.setrlimit(resource.RLIMIT_MSGQUEUE, (100000000, 100000000))
		#resource.setrlimit(resource.RLIMIT_NICE, (0, 0))

#print "hi from parent"

os.setpgrp()

#sys.stdout = codecs.getwriter('utf8')(sys.stdout)
#sys.stderr = codecs.getwriter('utf8')(sys.stderr)

#sys.stdout = codecs.getwriter('utf-16')(sys.stdout)
#sys.stdin = codecs.getreader('utf-16')(sys.stdin)
p = subprocess.Popen(sys.argv[1:], preexec_fn=setlimits(sys.argv[1:]))

delta = 10
if sys.argv[1].startswith("octave") or sys.argv[1] == "R":
	delta = 20
fin_time = time.time() + delta
while p.poll() == None and fin_time > time.time():
        time.sleep(0.1)

if fin_time < time.time():
		#p.kill(p.pid, -9)
		
		#p.terminate()
		#os.killpg(os.getpgid(p.pid), signal.SIGKILL)
		if select.select([sys.stdin,],[],[],0.0)[0]:
		        sys.stdin.readlines()

		sys.stderr.write("Process killed, because it ran longer than "+str(delta)+" seconds.")
		os.killpg(0, signal.SIGKILL)
		#if p.poll() == None:
			#os.kill(p.pid, -1*signal.SIGKILL)
		
sys.stderr.write('\n')
usage_stats = resource.getrusage(resource.RUSAGE_CHILDREN)
sys.stderr.write(str(p.returncode)+' '+str(usage_stats.ru_utime+usage_stats.ru_stime)+' '+str(usage_stats.ru_maxrss))

#f = open('/home/ren/Desktop/time.txt', 'a')
#f.write('python parent before exit - ' + str(datetime.datetime.now()))
#f.close()

if select.select([sys.stdin,],[],[],0.0)[0]:
        sys.stdin.readlines()

os.killpg(0, signal.SIGKILL)
#if p.poll() == None:
	#os.kill(p.pid, -1*signal.SIGKILL)