using System;

namespace unvell.D2DLib.WinForm
{
	public class FpsCounter
	{
		public int FramesPerSecond { get; private set; }

		private DateTime _lastFpsUpdate = DateTime.Now;
		private int _frameCounter = 0;

		public void Update()
		{
			var now = DateTime.Now;
			var elapsedSeconds = (float)(now - _lastFpsUpdate).TotalSeconds;

			++_frameCounter;
			if (elapsedSeconds < 0.5f)
				return;

			FramesPerSecond = (int)(_frameCounter / elapsedSeconds);

			_frameCounter = 0;
			_lastFpsUpdate = now;
		}
	}
}
