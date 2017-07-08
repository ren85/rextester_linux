import os

os.system("find /var/www/service/usercode/ -print0 | sudo xargs -0 rm -r")
os.system("mkdir /var/www/service/usercode")
os.system("chmod 777 /var/www/service/usercode")
os.system("find /var/www/service/diff/ -print0 | sudo xargs -0 rm -r")
os.system("mkdir /var/www/service/diff")
os.system("chmod 777 /var/www/service/diff")
os.system("/sbin/reboot")
