import os
import time

while True:
    time.sleep(0.1)
    if len([pid for pid in os.listdir('/proc') if pid.isdigit()]) > 300:
        print("killing")
        os.system("ps -ef | grep usercode | grep -v grep | awk '{print $2}' | xargs kill -9")
        os.system("ps -ef | grep diff | grep -v grep | awk '{print $2}' | xargs kill -9") 
        os.system("ps -ef | grep tmp | grep -v mono | grep -v grep | awk '{print $2}' | xargs kill -9")
        os.system("apache2ctl -k restart")
