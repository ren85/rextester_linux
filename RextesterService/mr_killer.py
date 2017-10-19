import os
import time
import subprocess

os.nice(-19)

count_sum = 0.0
total = 0.0
avg = 0.0

while True:
    time.sleep(0.1)
    
    procs = subprocess.check_output(['ps', 'uaxw']).splitlines()
    www_procs = [proc for proc in procs if 'www-data' in proc]
    count = len(www_procs)

    print count

    #count_sum = count_sum + count
    #total = total + 1 
    #avg = count_sum / total

    #if count > 100 or count > 3*avg:
    if count > 100:
        print("killing")
        #os.system("ps -ef | grep usercode | grep -v grep | awk '{print $2}' | xargs kill -9")
        #os.system("ps -ef | grep diff | grep -v grep | awk '{print $2}' | xargs kill -9") 
        #os.system("ps -ef | grep tmp | grep -v mono | grep -v grep | awk '{print $2}' | xargs kill -9")
        #os.system("ps -ef | grep www-data | grep -v grep | awk '{print $2}' | xargs kill -9")
        os.system("killall -STOP -u www-data")
        os.system("killall -KILL -u www-data")
        #count_sum = 0.0
        #total = 0.0
        #avg = 0.0
        print("done")
        #os.system("apache2ctl -k restart")
        #time.sleep(3)
        