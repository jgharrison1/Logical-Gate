using System;

[Flags]
public enum BinaryRepresentation
{
    UnsignedMagnitude = 1 << 0,
    SignedMagnitude = 1 << 1,
    TwosComplement = 1 << 2
}

