#if UseSingularityPDB

///////////////////////////////////////////////////////////////////////////////
//
//  Microsoft Research Singularity PDB Info Library
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//  File:   MsfDirectory.cs
//
using System;

namespace Microsoft.Singularity.PdbInfo.Features
{
    public class MsfDirectory
    {
        public MsfDirectory(PdbReader reader, PdbFileHeader head, BitAccess bits)
        {
            bits.MinCapacity(head.directorySize);
            int pages = reader.PagesFromSize(head.directorySize);

            // 0..n in page of directory pages.
            reader.Seek(head.directoryRoot, 0);
            bits.FillBuffer(reader.reader, pages * 4);

            DataStream stream = new DataStream(head.directorySize, bits, pages);
            bits.MinCapacity(head.directorySize);
            stream.Read(reader, bits);

            // 0..3 in directory pages
            int count;
            bits.ReadInt32(out count);

            // 4..n
            int[] sizes = new int[count];
            bits.ReadInt32(sizes);

            // n..m
            streams = new DataStream[count];
            for (int i = 0; i < count; i++) {
                if (sizes[i] <= 0) {
                    streams[i] = new DataStream();
                }
                else {
                    streams[i] = new DataStream(sizes[i], bits,
                                                reader.PagesFromSize(sizes[i]));
                }
            }
        }

        public DataStream[] streams;
    }

}
#endif