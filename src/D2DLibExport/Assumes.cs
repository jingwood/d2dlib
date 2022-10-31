using System.Diagnostics.CodeAnalysis;

namespace unvell.D2DLib;

internal static class Assumes
{
	public static void NotNull<T>([NotNull] T? value)
	{
		if (value is null)
			throw new Exception($"Value of type {typeof(T).Name} cannot be null.");
	}
}
