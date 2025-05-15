using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConceptMenu : MonoBehaviour
{
    public TextMeshProUGUI conceptHeader;
    public TextMeshProUGUI body;

    public void lg1() {
        conceptHeader.text = "And gate";
        body.text = "AND gate: Outputs 1 only if both inputs are 1.\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg2() {
        conceptHeader.text = "Or gate";
        body.text = "Outputs 1 if at least one input is 1.\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg3() {
        conceptHeader.text = "Xor gate";
        body.text = "Exclusive or, XOR for short. Outputs 1 only if inputs are different (1 and 0, or 0 and 1).\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg4() {
        conceptHeader.text = "Nand Gate";
        body.text = "A NAND gate gives the opposite result of an AND gate. It outputs 0 only when all inputs are 1; otherwise, it outputs 1.\nNAND gates are functionally complete, meaning you can build any other logic gate using just NAND gates.\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg5() {
        conceptHeader.text = "Nor Gate";
        body.text = "A NOR gate gives the opposite result of an OR gate. It outputs 1 only when all inputs are 0.\nNOR Gates are a functionally complete gate — any logic function can be built using only NOR gates.\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg6() {
        conceptHeader.text = "XNor Gate";
        body.text = "XNOR is also known as the equivalence gate because it checks for equality\nIf a == b, output is 1\nOften used in comparators, error detection, and parity checking\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg7() {
        conceptHeader.text = "Not Gate";
        body.text = "A not gate is a simple logic gate that negates its' input as the output\nIf input is 1, output is 0\nIf input is 0, output is 1\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits.";
    }
    public void lg8() {
        conceptHeader.text = "Buffer Gate";
        body.text = "A buffer is a simple logic gate that passes its' input to the output unchanged\nIf input is 1, output is 1\nIf input is 0, output is 0\nLogic gates are fundamental electronic components that perform basic logical operations on one or more binary inputs. They output a binary value (0 or 1) depending on the inputs and the gate type. These are the building blocks of all digital circuits";
    }
    public void ba1() {
        conceptHeader.text = "Base-10 & Base-2";
        body.text = "Decimal (Base 10): Uses digits 0 through 9.\nFor example: 375 = 3x100 + 7x10 + 5x1\n Each digit represents a power of 10.\nBinary (Base 2): Uses only digits 0 and 1. Each digit represents a power of 2.\nFor example: 1011 = 1x8 + 0x4 + 1x2 + 1x1 = 11";
    }
    public void ba2() {
        conceptHeader.text = "Binary Addition";
        body.text = "";
    }
    /*
    public void ba3() {
        conceptHeader.text = "Bits & Bytes";
        body.text = "";
    }
    */
    public void ba4() {
        conceptHeader.text = "Rep Types";
        body.text = "How a number is represented affects what values it can store:\nUnsigned Integers (AKA Unsigned Magnitude): Only store non-negative numbers.\n Signed Magnitude: Same as Unsigned Magnitude, except that the most significant bit will signal positive or negative, 1 for negative, 0 for positive.\nTwo's Complement: Can store negative and positive numbers and does not contain a redundant representation for 0 (i.e. -0).";
    }
    public void ba5() {
        conceptHeader.text = "Rep Ranges";
        body.text = "let N = the number of bits in a binary number\nUnsigned Magnitude: range is 0 to 2^(n-1)\nWith 8 bits: values range from 0 to 255.\nSigned Magnitude: range is -(2^(n-1)-1) to (2^(n-1)-1).\nWith 8-bits, range is from -127 to 127.\nAlso includes a representation for -0, which can just be interpreted as 0. \nTwo's Complement: Range is -(2^(n-1)) to 2^(n-1)-1\nWith 8 bits: range is -128 to +127.\nThe first bit indicates the sign (0 = positive, 1 = negative).";
    }
    public void ba6() {
        conceptHeader.text = "Overflow";
        body.text = "Overflow occurs when a number is too big (or too small) to fit in the number of bits you have.\nUnsigned Overflow: If you try to store 300 in an 8-bit unsigned number, it wraps around to 44 (because 300 - 256 = 44).\nSigned Overflow: If you add 100 and 50 using 8-bit signed two's complement, the result would be -106 instead of 150 due to overflow.\nMost programming languages and CPUs have flags or signals to indicate when overflow has occurred, because it can cause serious logic errors.";
    }
    public void mm1() {
        conceptHeader.text = "FCFS";
        body.text = "First Come First Serve - A very basic CPU scheduling algorithm where processes are executed in the order they arrive in the queue.\nNon-preemptive: Once a process starts, it runs until it finishes.\nSimple and fair, but has poor performance if a long job blocks shorter ones behind it (known as the convoy effect).\nSuitable for batch systems but not good for interactive systems.";
    }
    public void mm2() {
        conceptHeader.text = "SJF";
        body.text = "Shortest Job First - This algorithm picks the process with the shortest total execution time next.\nNon-preemptive: Once selected, the job runs to completion.\nMinimizes average waiting time compared to FCFS.\nDownside: It requires knowing the execution time of each process ahead of time, which is not always possible.\nMay lead to starvation for longer jobs if short jobs keep arriving\n Preemptive version of SJF is SRT - Shortest Remaining Time - The CPU always switches to the process with the least time left to finish.\nGood for interactive systems but involves more context switching. Like SJF, it risks starvation for longer tasks.";
    }
    public void mm3() {
        conceptHeader.text = "LRTF";
        body.text = "An uncommon and mostly theoretical scheduling algorithm. Picks the process with the longest remaining time to complete.\nPreemptive: if a new job with longer time arrives, it can take over.\nLeads to poor responsiveness and starvation of shorter jobs.\nRarely used in practice but useful in academic examples or simulations.";
    }
    public void mm4() {
        conceptHeader.text = "Round Robin";
        body.text = "A popular and fair preemptive scheduling method for time-sharing systems.\nEach process gets a time slice (e.g., 10 milliseconds).\nIf it doesn’t finish in time, it goes to the back of the queue.\nPrevents any one process from monopolizing the CPU.\nWorks well for interactive systems, like multitasking operating systems.\nDownside: more context switching, which adds overhead.";
    }
    public void mm5() {
        conceptHeader.text = "Fragmentation";
        body.text = "Fragmentation refers to wasted memory space that occurs when memory is allocated inefficiently. It happens when the total free memory is enough to satisfy a request, but it’s not contiguous (not all in one place) or not usable due to the way it was allocated. There two main types are internal and external fragmentation.\nInternal fragmentation occurs when the system allocates fixed-size memory blocks (like pages or segments), and a process doesn't use the entire block.\n External Fragmentation occurs when free memory exists, but it's scattered in small blocks, so it can't be used efficiently.";
    }
    public void mm6() {
        conceptHeader.text = "Paging";
        body.text = "Paging is a memory management technique designed to eliminate external fragmentation, and nearly eliminate internal fragmentation as well. \nAddresses in secondary memory(AKA Virtual addresses or Logical Addresses) generated by the CPU are split into two parts:\nPage number: used to index into the page table to find the corresponding frame number.\nOffset: specifies the exact location within the page.\n Pages in secondary memory(harddrive or harddisk) are then mapped to frames in main memory(RAM). Operating system keeps track of which page goes to which frame with a page table.";
    }
    public void mm7() {
        conceptHeader.text = "Memory Sizes";
        body.text = "Memory size for secondary memory and physical memory can be determined by simple equations involving the number of bits with the 'p','d', and 'f' parts of a logical and/or physical address\nor any given address in a process:\nLogical Address = 'p' + 'd' (concatenated, not summed)\n Physical Address = 'f'+'d'(concatenated, not summed)";
    }
    public void mm8() {
        conceptHeader.text = "Multi-level Paging";
        body.text = "Multi-level paging breaks the page table into multiple layers (e.g., a page directory that points to page tables). This reduces the memory used for page tables by only creating tables when needed.";
    }

    
}