namespace Nautilus.zlib
{
	public class ZStreamException:System.IO.IOException
	{
		public ZStreamException()
		{
		}
		public ZStreamException(System.String s):base(s)
		{
		}
	}
}