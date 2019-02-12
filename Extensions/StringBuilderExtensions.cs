using System.Linq;
using System.Text;

namespace Ideine.LogsSender.Extensions
{
	internal static class StringBuilderExtensions
	{
		public static int NumberOfLines(this StringBuilder builder)
		{
			var str = builder.ToString();

			return str.Count(c => c == '\n');
		}
	}
}