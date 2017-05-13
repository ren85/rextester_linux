import os
import time

while True:
    time.sleep(1)
    if len([pid for pid in os.listdir('/proc') if pid.isdigit()]) > 300:
        print("killing")
        os.system("ps -ef | grep usercode | grep -v grep | awk '{print $2}' | xargs kill -9")
