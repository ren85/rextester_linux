import os
import datetime

os.system("rm /home/ren/out.txt")

os.system('curl -H "Content-Type: text/xml; charset=utf-8" -X POST -d \'<?xml version="1.0" encoding="utf-8"?><soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><soap:Body><DoWork xmlns="http://rextester.com/"><Program>print "Hello, world!"</Program><Language>Python</Language><user>awesome</user><pass>day</pass><bytes>true</bytes><programCompressed>false</programCompressed><inputCompressed>false</inputCompressed></DoWork></soap:Body></soap:Envelope>\' http://localhost/Service.asmx > /home/ren/out.txt')

if 'SABlAGwAbABvACwAIAB3AG8AcgBsAGQAIQAKAA==' not in open('/home/ren/out.txt').read():
    
    os.system("find /var/www/service/usercode/ -print0 | sudo xargs -0 rm -r")
    os.system("mkdir /var/www/service/usercode")
    os.system("chmod 777 /var/www/service/usercode")
    os.system("find /var/www/service/diff/ -print0 | sudo xargs -0 rm -r")
    os.system("mkdir /var/www/service/diff")
    os.system("chmod 777 /var/www/service/diff")
    
    with open('/home/ren/last_reboot.txt', 'a') as f1:
        f1.write(datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S") + os.linesep)
    os.system("/sbin/reboot")
