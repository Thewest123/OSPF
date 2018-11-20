# OSPF Calculator
Calculates routing tables of all nodes based on their initial neighbors

## How to use
 1. Using the "**OSPFInputGenerator**" exe program, create input network
	 1. Enter the node name/ID *(string)*
	 2. Enter the node's neighbor name/ID *(string)*
	 3. Enter the route cost from the node to the neighbor *(int)*
	 4. Press "**enter**" key and repeat steps ii. - iv. (2-4)
	 5. Press "**enter**" key again to save node and repeat steps i. - v. (1-5) for another node
	 6. Press "**enter**" key again to save all to the file "*input.json*"

 2. Using the "**OSPFCalculator**" exe program, generate routing tables for all nodes
	 1. Enter the input file location (leave blank for default "*input.json*")
	 2. Enter the name/ID of the node that you want to see on console
	 3. All other routing tables are calculated in the background and saved to the file "*output.json*"

**Existing files "*input.json*" or "*output.json*" will be overwritten every time the programs are run!**

## Pre-created network
In the file "*input.json*" there is already pre-created network (nodes, their neighbors and route cost to the neighbors) according to the "*input-schema.png*" network schema. You can just run the "**OSPFCalculator**" exe program, enter the node's ID and view calculation of node's routing table.

![Pre-created network schema](https://raw.githubusercontent.com/Thewest123/OSPF/master/input-schema.png)
