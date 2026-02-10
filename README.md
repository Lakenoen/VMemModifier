# VMemModifier
![Static Badge](https://img.shields.io/badge/C%23-Green) ![Static Badge](https://img.shields.io/badge/C%2B%2B-blue) 
![Static Badge](https://img.shields.io/badge/.NET-gray?link=https%3A%2F%2Fdotnet.microsoft.com%2Fen-us) 
![Static Badge](https://img.shields.io/badge/Win32%20API-purple)
![Static Badge](https://img.shields.io/badge/x64-white)
![Static Badge](https://img.shields.io/badge/CMake-orange?label=build)
![Static Badge](https://img.shields.io/badge/Windows-red?label=platform)

## Description
An application for searching, reading and writing to virtual memory, as well as for dll injection (Windows Only).
The application consists of a main console application, a GUI shell and a native DLL.

## Functional
- Search in process virtual memory
- Reading data from virtual memory
- Reading data from virtual memory
- DLL injection into another process's memory
- Creation and destruction of processes

## Usage
[Download](https://github.com/Lakenoen/VMemModifier/releases/download/v1.0.0/VMemModifier.7z) the archive, drop the dll into System32. Run ```VMemModifierConsole.exe``` or ```VMemModifierGUI.exe```.

## VMemModifierConsole Usage
command: `search` - Searches at virtual memory (arg1 - ID, arg2 - Data) or (arg1 - ID, arg2 - Data, arg3 - start address, arg4 - end address)  
flag: bin - The format of the desired data  
flag: hex - The format of the desired data, it can be combined with flags: int, long, short, byte, float, doable, str  
flag: str - The format of the desired data, it can be combined with flags: ascii, utf8, unicode  
flag: utf8 - The format of the desired data, it can be combined with flags: str  
flag: unicode - The format of the desired data, it can be combined with flags: str  
flag: ascii - The format of the desired data, it can be combined with flags: str  
flag: byte - The format of the desired data, it can be combined with flags: hex  
flag: int - The format of the desired data, it can be combined with flags: hex  
flag: float - The format of the desired data, it can be combined with flags: hex  
flag: double - The format of the desired data, it can be combined with flags: hex  
flag: short - The format of the desired data, it can be combined with flags: hex  
flag: long - The format of the desired data, it can be combined with flags: hex  
flag: reg - Regular expression  

command: `read` - Reading data at the address (arg1 - ID, arg2 - Address, arg3 - Size)  
flag: bin - Data output format  
flag: hex - Data output format, it can be combined with flags: int, long, short, byte, float, doable, str  
flag: str - Data output format, it can be combined with flags: ascii, utf8, unicode  
flag: utf8 - Data output format, it can be combined with flags: str  
flag: unicode - Data output format, it can be combined with flags: str  
flag: ascii - Data output format, it can be combined with flags: str  
flag: byte - Data output format, it can be combined with flags: hex  
flag: int - Data output format, it can be combined with flags: hex  
flag: float - Data output format, it can be combined with flags: hex  
flag: double - Data output format, it can be combined with flags: hex  
flag: short - Data output format, it can be combined with flags: hex  
flag: long - Data output format, it can be combined with flags: hex  

command: `write` - Enters data at the specified address (arg1 - ID, arg2 - Address, arg3 - Data)  
flag: bin - The format of the entered data  
flag: hex - The format of the entered data, it can be combined with flags: int, long, short, byte, float, doable, str  
flag: str - The format of the entered data, it can be combined with flags: ascii, utf8, unicode  
flag: utf8 - The format of the entered data, it can be combined with flags: str  
flag: unicode - The format of the entered data, it can be combined with flags: str  
flag: ascii - The format of the entered data, it can be combined with flags: str  
flag: byte - The format of the entered data, it can be combined with flags: hex  
flag: int - The format of the entered data, it can be combined with flags: hex  
flag: float - The format of the entered data, it can be combined with flags: hex  
flag: double - The format of the entered data, it can be combined with flags: hex  
flag: short - The format of the entered data, it can be combined with flags: hex  
flag: long - The format of the entered data, it can be combined with flags: hex  

command: `inject` - Inject DLL to the specified process (arg1 - ID, arg2 - Path to DLL)  

command: `create` - Launches the specified process (arg1 - path to exe file)  

command: `close` - Closes the specified process (arg1 - id)  

command: `dump` - Save dump to file (arg1 - id, arg2 - path to new dump file)  

command: `help` - Provides information about commands and flags (arg1 - [ empty | flag | name ])  
flag: name - Search by command name  
flag: flag - Search by flag name  

## VMemModifierGUI interface
<img width="1428" height="752" alt="Снимок" src="https://github.com/user-attachments/assets/f6a0eb2f-d173-41a2-8ffb-9578014fefb7" />

