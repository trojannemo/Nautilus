namespace Nautilus.zlib
{
	public class ZInputStream:System.IO.BinaryReader
	{
		internal void  InitBlock()
		{
			flush = zlibConst.Z_NO_FLUSH;
			buf = new byte[bufsize];
		}
		virtual public int FlushMode
		{
			get
			{
				return (flush);
			}
			
			set
			{
				flush = value;
			}
			
		}
		/// <summary> Returns the total number of bytes input so far.</summary>
		virtual public long TotalIn
		{
			get
			{
				return z.total_in;
			}
			
		}
		/// <summary> Returns the total number of bytes output so far.</summary>
		virtual public long TotalOut
		{
			get
			{
				return z.total_out;
			}
			
		}
		
		protected ZStream z = new ZStream();
		protected int bufsize = 512;		
		protected int flush;		
		protected byte[] buf, buf1 = new byte[1];
		protected bool compress;
		
		internal System.IO.Stream in_Renamed = null;
		
		public ZInputStream(System.IO.Stream in_Renamed):base(in_Renamed)
		{
			InitBlock();
			this.in_Renamed = in_Renamed;
			z.inflateInit();
			compress = false;
			z.next_in = buf;
			z.next_in_index = 0;
			z.avail_in = 0;
		}
		
		public ZInputStream(System.IO.Stream in_Renamed, int level):base(in_Renamed)
		{
			InitBlock();
			this.in_Renamed = in_Renamed;
			z.deflateInit(level);
			compress = true;
			z.next_in = buf;
			z.next_in_index = 0;
			z.avail_in = 0;
		}
		
		public  override int Read()
		{
			if (read(buf1, 0, 1) == - 1)
				return (- 1);
			return (buf1[0] & 0xFF);
		}
		
		internal bool nomoreinput = false;
				
		public int read(byte[] b, int off, int len)
		{
			if (len == 0)
				return (0);
			int err;
			z.next_out = b;
			z.next_out_index = off;
			z.avail_out = len;
			do 
			{
				if ((z.avail_in == 0) && (!nomoreinput))
				{
					// if buffer is empty and more input is avaiable, refill it
					z.next_in_index = 0;
					z.avail_in = SupportClass.ReadInput(in_Renamed, buf, 0, bufsize); //(bufsize<z.avail_out ? bufsize : z.avail_out));
					if (z.avail_in == - 1)
					{
						z.avail_in = 0;
						nomoreinput = true;
					}
				}
				err = compress ? z.deflate(flush) : z.inflate(flush);
				if (nomoreinput && (err == zlibConst.Z_BUF_ERROR))
					return (- 1);
				if (err != zlibConst.Z_OK && err != zlibConst.Z_STREAM_END)
					throw new ZStreamException((compress?"de":"in") + "flating: " + z.msg);
				if (nomoreinput && (z.avail_out == len))
					return (- 1);
			}
			while (z.avail_out == len && err == zlibConst.Z_OK);
			return (len - z.avail_out);
		}
				
		public long skip(long n)
		{
			var len = 512;
			if (n < len)
				len = (int) n;
			var tmp = new byte[len];
			return SupportClass.ReadInput(BaseStream, tmp, 0, tmp.Length);
		}
		
		public override void  Close()
		{
			in_Renamed.Close();
		}
	}
}