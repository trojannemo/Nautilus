using System.Text;

/*
    RB3ESongID.cs by Emma (ipg.gay), 2024
    granted to you under the public domain - feel free to use and modify
    
    for ideal usage: *always* use the song ID from the songs.dta if it is an integer.
    otherwise, use the symbol from the song_id DataNode. don't assume the shortname at
    the start of songs.dta is always the same as the song_id on these broken customs
*/

namespace Nautilus
{
    public class SongIDCorrector
    {
        // only make one CRC32 table for the whole class
        private uint[] crc32_table;

        // public domain CRC32 algorithm
        // from http://home.thep.lu.se/~bjorn/crc/crc32_simple.c (see web.archive.org)
        private static uint crc32_for_byte(uint r)
        {
            int j;
            for (j = 0; j < 8; ++j)
                r = ((r & 1) == 1 ? 0 : (uint)0xEDB88320L) ^ r >> 1;
            return r ^ (uint)0xFF000000L;
        }
        private uint crc32(byte[] data)
        {
            uint crc = 0;
            for (int i = 0; i < data.Length; i++)
                crc = crc32_table[(byte)crc ^ data[i]] ^ crc >> 8;
            return crc;
        }

        public SongIDCorrector()
        {
            // generate the CRC-32 table for this class instance
            crc32_table = new uint[0x100];
            for (uint i = 0; i < 0x100; i++)
                crc32_table[i] = crc32_for_byte(i);
        }

        public uint ShortnameToSongID(string shortname)
        {
            byte[] buffer = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes(shortname);
            // run a CRC32 sum over the whole length of the string
            uint checksum = crc32(buffer);
            // move it around a bit just to make things more consistent
            // risks introducing more collisions, BAD CUSTOMS ARE STILL BAD!!
            checksum %= 9999999;
            checksum += 2130000000;
            return checksum;
        }
    } 
}
