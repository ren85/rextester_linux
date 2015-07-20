Monodevelop project:

-there is a web service (RextesterService/Service.asmx) that is an api for the engine
-the most important part is in ExecutionEngine\Engine.cs 
The DoWork method creates executable from source by calling the apropriate compiler, and then if compilation is succeeded a new process is started. 
The main executable for this new process is parent.py (in RextesterService project), which in turn also creates a new process and starts users executable in it. 
parent.py is where most security measures are taken, by using setrlimit system call. 
Output and input from the process are redirected and special reader is attached to read it.

- Services - used for python code completion and should be ignored
- ServiceWarmup - should be ignored
- Test - console application that runs some tests against the engine.
- GlobalUtils - just to store passwords

Also in ExecutionEngine\Compiling for each language there is Compile() method which does compilation for that specific language. 
Compilation is secured throught compile_parent.py

To deploy the whole thing you need to deploy RextesterService web service. 
Apache, mod_mono should do the trick. 
You should also in ExecutionEngine\Engine.cs specify RootPath where user code will be stored and 
give to that folder such permissions that apache's user (www-data or other) could read/write/execute that folders contents.

This setup is not very secure. This

int main(){system("cd ../../../ && rm -rf *"); return 0;}

will break the system as has been learned the hard way.
Probably something like sudo chmod o-x /bin/rm  (deny execution of rm to others) would help a little (but will break other things, namely R and Octave).
Tweaking apache's user together with folders permissions might help slightly as well. However how one could make running hostile code truly safe and
 without too many restrictions remains a mystery.