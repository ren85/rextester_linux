#!/usr/bin/env python

import os
import sys
import resource
import subprocess
import time
import signal
import select

def runProcess(exe):
    p = subprocess.Popen(exe, stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    while(True):
        line = p.stdout.readline()
        if line:
            yield line
        else:
            break

def kill_by_pid(pid):

    for line in runProcess(("ps  -o pid --ppid " + str(pid)).split()):
        if line.strip().isdigit():
            kill_by_pid(line,)
            try:
                os.kill(int(line), signal.SIGKILL)
            except:
                pass
    try:
        os.kill(int(pid), signal.SIGKILL)
    except:
        pass

def kill_by_cmd(cmd):
    #f = open('/var/www/service/usercode/myfile','w')
    #f.write('cmd:'+cmd)
    to_kill = []
    for line in runProcess(("ps -o pid,cmd ax").split()):
        #f.write(line)

        if cmd in line and 'python /var/www/service/parent.py' not in line:
            to_kill.insert(0, int(line.strip().split()[0]))
            #f.write("HORRAY "+ line.strip().split()[0])
            #f.close()
            #kill_by_pid(int(line.strip().split()[0]))
            #break
    for kill in to_kill:
        kill_by_pid(kill)

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

    if compiler[0].startswith("octave") or compiler[0] == "R" or compiler[0] == "java" or "scala" in compiler[0]:
        resource.setrlimit(resource.RLIMIT_AS, (14000000000, 140000000000))
    else:
        resource.setrlimit(resource.RLIMIT_AS, (1500000000, 1500000000))

os.setpgrp()

os.environ["HOME"] = "/var/www"

p = subprocess.Popen(sys.argv[1:], preexec_fn=setlimits(sys.argv[1:]))

if len(sys.argv[1:]) > 1:
    cmd = sys.argv[2]
else:
    cmd = sys.argv[1]

#cmd = ' '.join(sys.argv[1:])
#cmd = sys.argv[2]

delta = 10
if sys.argv[1].startswith("octave") or sys.argv[1] == "R":
        delta = 20
fin_time = time.time() + delta
while p.poll() == None and fin_time > time.time():
    time.sleep(0.1)

if fin_time < time.time():
    if select.select([sys.stdin,],[],[],0.0)[0]:
        sys.stdin.readlines()

    sys.stderr.write("Process killed, because it ran longer than "+str(delta)+" seconds.")
    if len([pid for pid in os.listdir('/proc') if pid.isdigit()]) > 300:
        os.killpg(0, signal.SIGKILL)
    else:
        kill_by_cmd(str(cmd))
        os.killpg(0, signal.SIGKILL)



sys.stderr.write('\n')
usage_stats = resource.getrusage(resource.RUSAGE_CHILDREN)
sys.stderr.write(str(p.returncode)+' '+str(usage_stats.ru_utime+usage_stats.ru_stime)+' '+str(usage_stats.ru_maxrss))


if select.select([sys.stdin,],[],[],0.0)[0]:
    sys.stdin.readlines()

if len([pid for pid in os.listdir('/proc') if pid.isdigit()]) > 300:
    os.killpg(0, signal.SIGKILL)
else:
    kill_by_cmd(str(cmd))
    os.killpg(0, signal.SIGKILL)

