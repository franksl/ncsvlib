# NCsvLib

## Introduction

NCsvLib is a .NET library for writing 'structured' text files/streams.
An example of structured file is CSV (comma separated values), but the library
can handle more complex structures.
Basically you define a 'schema' that describes size and format of various
fields that the library then reads from a data source (ie. database) and
writes their values to an output stream (ie. text file or http stream).
Its architecture makes possible to use various methods for 
storing data (ie. on database) and define file schema (default is XML 
file).


## Installation

Simply copy NCsvLib.dll in the same directory of your project executables.
The file LICENSE should be deployed with NCsvLib.dll.
