#!/usr/bin/env python

import os
import sys
import resource
import subprocess
import time
import signal

os.setpgrp()

if sys.argv[1] == "ghc":
    os.environ["HOME"] = "/var/www"

p = subprocess.Popen(sys.argv[1:])

delta = 30
fin_time = time.time() + delta
while p.poll() == None and fin_time > time.time():
        time.sleep(0.1)

if fin_time < time.time():
		sys.stderr.write("Compilation terminated after "+str(delta)+" seconds.")
		os.killpg(0, signal.SIGKILL)
		
#os.killpg(0, signal.SIGKILL)
