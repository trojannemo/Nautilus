namespace Nautilus.zlib
{
	sealed class Adler32
	{
		
		// largest prime smaller than 65536
		private const int BASE = 65521;
		// NMAX is the largest n such that 255n(n+1)/2 + (n+1)(BASE-1) <= 2^32-1
		private const int NMAX = 5552;
		
		internal long adler32(long adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1L;
			}
			
			var s1 = adler & 0xffff;
			var s2 = (adler >> 16) & 0xffff;

		    while (len > 0)
			{
				var k = len < NMAX?len:NMAX;
				len -= k;
				while (k >= 16)
				{
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					s1 += (buf[index++] & 0xff); s2 += s1;
					k -= 16;
				}
				if (k != 0)
				{
					do 
					{
						s1 += (buf[index++] & 0xff); s2 += s1;
					}
					while (--k != 0);
				}
				s1 %= BASE;
				s2 %= BASE;
			}
			return (s2 << 16) | s1;
		}
		
	}
}