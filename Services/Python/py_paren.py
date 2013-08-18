import jedi
import sys

path = sys.argv[1]
line = int(sys.argv[2])
column = int(sys.argv[3])

jedi.settings.cache_directory = '/var/www/service/Python/cache'
#jedi.settings.use_filesystem_cache = False

with open(path, 'r') as content_file:
	source = content_file.read()

script = jedi.Script(source, line, column, 'rextester.py')


for i in script.call_signatures():
    full = ""
    for j in i.params:
        if full != "":
            full += ", "
        full += j.get_code().rstrip()
    if full != "":
        print full
