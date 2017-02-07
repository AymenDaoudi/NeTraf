# NeTraf
Linux utility for monitoring Bandwidth consumption by process name


## Description

NeTraf is a linux application that helps you monitor the bandwidth consumption of a specific process.
NeTraf is written in **C#** on **.Net-Core** and uses the following linux utilities : **[Netstat](http://netstat.net/)**, **[Iptraf](http://iptraf.seul.org/)** and **[Gnuplot](http://www.gnuplot.info/)**.

## Prerequisites

Make sur you have the following tools installed before running **NeTraf** :
1. *Netstat* : this utility comes installed on some linux distributions. To install it (Debian/Ubuntu) : `# apt-get install net-tools`
2. *Iptraf* : Download and install this utility from [here](ftp://iptraf.seul.org/pub/iptraf/iptraf-3.0.0.bin.i386.tar.gz), then follow these steps to install it:

    i. Decompress the .tar.gz file by entering `tar zxvf iptraf-x.y.z.tar.gz`.
    
    ii. If your tar doesn't support the z option, you can separately decompress the tar.gz then extract the resulting .tar archive.
        `gunzip iptraf-x.y.z.tar.gz`
        `tar xvf iptraf-x.y.z.tar`
    
    iii. This will decompress the sources into a directory called iptraf-x.y.z.
    
    iv. x.y.z here should be the IPTraf version number you're installing, like 3.0.0.
    
    v. Change to the src directory. It already contains ready-to-run distribution binaries for IPTraf and the accompanying rvnamed daemon.

    vi. To install the software, enter : `make install`.

while you are logged in as "root". This will install the distribution binary in the /usr/local/bin directory. The necessary working directory /var/local/iptraf will also be created.

3. *GnuPlot* : `apt-get install gnuplot`
4. *.Net Core* : Get .Net Core and the toolings [here](https://www.microsoft.com/net/core#linuxubuntu).

## Compilation and running

1. Download the project [here](https://github.com/AymenDaoudi/NeTraf/archive/master.zip).
2. Navigate to the project's location `../NeTraf`.
3. Enter `dotnet restore` to restore the Nuget dependencies.
4. Enter `dotnet run` + args :
    i. 1st argument : **Network interface name**.
    ii. 2nd argument : **Process name**.
    iii. 3rd argument : **Monitoring interval** (in minutes).
    iv. 4th argument : **Output directory** (in minutes).
    v. Example : `dotnet run ens33 skype 15 ../Desktop/OutputResult`.

## Results

Once the profiling is finished, navigate to the output file you precised in the beginning, there you'll find tow directories :
1. RawOutput files : these contain csv formatted output files describing the input/output bytes, packets and rate.
2. GraphicalOutput files : contain plots describing the input/output bytes, packets and rate, like follows :

Inline-style: 
![alt text](https://github.com/AymenDaoudi/NeTraf/blob/master/Images/Incoming_Traffic_Rate.png "Incoming traffic rate")

## License

This project is under the [Creative Commons Attribution NonCommercial NoDerivs (CC-NC-ND)] (https://tldrlegal.com/license/creative-commons-attribution-noncommercial-noderivs-(cc-nc-nd)#summary) fully described in the (c.f. [License.txt](License.txt)) file.

![CC-NC-ND](http://i.creativecommons.org/l/by-nc-nd/3.0/88x31.png)


