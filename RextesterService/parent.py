#!/usr/bin/env python

import os
import sys
import resource
import subprocess
import time
import signal
import select

def TotalFiles():
    batcmd="ls -fR /var/www/service/usercode | wc -l"
    result = subprocess.check_output(batcmd, shell=True)
    usercode = int(result)
                                        
    batcmd="ls -fR /var/www/service/diff | wc -l"
    result = subprocess.check_output(batcmd, shell=True)
    diff = int(result)                                                          

    batcmd="ls -fR /tmp | wc -l"
    result = subprocess.check_output(batcmd, shell=True)
    tmp = int(result)

    return usercode + diff + tmp

def setlimits(compiler):

    if compiler[0].startswith("octave") or compiler[0] == "R" or "kotlin" in compiler[0]:
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

    if compiler[0].startswith("erl")  or compiler[0].startswith("elixir") or compiler[0].startswith("octave") or compiler[0] == "R" or compiler[0] == "java" or "kotlin" in compiler[0] or "scala" in compiler[0]:
        resource.setrlimit(resource.RLIMIT_AS, (14000000000, 140000000000))
    else:
        resource.setrlimit(resource.RLIMIT_AS, (1500000000, 1500000000))

os.setpgrp()

os.environ["HOME"] = "/var/www"
os.environ["NODE_PATH"] = "/usr/local/lib/node_modules/"

if not sys.argv[1].startswith("erl"):
    os.chdir("/var/www/service/usercode")

total_files_before = TotalFiles()
file_threshold = 100

p = subprocess.Popen(sys.argv[1:], preexec_fn=setlimits(sys.argv[1:]))

if len(sys.argv[1:]) > 1:
    cmd = sys.argv[2]
else:
    cmd = sys.argv[1]

#cmd = ' '.join(sys.argv[1:])
#cmd = sys.argv[2]


delta = 10
if sys.argv[1].startswith("octave") or sys.argv[1] == "R" or "kotlin" in sys.argv[1]:
        delta = 20
fin_time = time.time() + delta
while p.poll() == None and fin_time > time.time():
    new_file_nr = TotalFiles()
    if new_file_nr - total_files_before > file_threshold:
        sys.stderr.write("Process killed, because sudden increase in number of files detected.")
        os.killpg(0, signal.SIGKILL)
    time.sleep(0.01)

if fin_time < time.time():
    if select.select([sys.stdin,],[],[],0.0)[0]:
        sys.stdin.readlines()

    sys.stderr.write("Process killed, because it ran longer than "+str(delta)+" seconds. Is your code waiting for keyboard input which is not supplied?")
    os.killpg(0, signal.SIGKILL)



sys.stderr.write('\n')
usage_stats = resource.getrusage(resource.RUSAGE_CHILDREN)
sys.stderr.write(str(p.returncode)+' '+str(usage_stats.ru_utime+usage_stats.ru_stime)+' '+str(usage_stats.ru_maxrss))


if select.select([sys.stdin,],[],[],0.0)[0]:
    sys.stdin.readlines()

os.killpg(0, signal.SIGKILL)

