using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGadgets.Open.Framework.Threading
{
	public class BackgroundMaintenanceProcessor
	{
		/// <summary>Called immediately before defined process is executed.</summary>
		public event EventHandler BeforeProcessCalled;
		/// <summary>Called immediately after defined process is executed.</summary>
		public event EventHandler AfterProcessCalled;
		/// <summary>Called every time the timer ticks.</summary>
		public event EventHandler IntervalElapsed;
		/// <summary>The System.Threading.Timer running the clock.</summary>
		public Timer Timer { get; private set; }
		/// <summary>The user defined method to execute at every interval.</summary>
		public Action ProcessMethod { get; set; }
		/// <summary>The interval in milliseconds between each execution of the ProcessMethod.</summary>
		public Int32 Interval { get; set; }
		/// <summary>The initial wait time in milliseconds before the first execution of the ProcessMethod.</summary>
		public Int32 InitialDelay { get; set; }

		public long Metric_IntervalElapsedCount;
		public long Metric_ProcessExecutedCount;
		public long Metric_ProcessSkippedCount;
		private int _IsProcessing;

		/// <summary>Constructor</summary>
		/// <param name="processMethod">User defined method to execute at every interval.</param>
		/// <param name="interval_ms">The interval in milliseconds between each execution of the ProcessMethod.</param>
		/// <param name="initialDelay_ms">The initial wait time in milliseconds before the first execution of the ProcessMethod.</param>
		private BackgroundMaintenanceProcessor(Action processMethod, Int32 interval_ms, Int32 initialDelay_ms)
			: this(interval_ms, initialDelay_ms)
		{
			this.ProcessMethod = processMethod;
		}
		protected BackgroundMaintenanceProcessor(Int32 interval_ms, Int32 initialDelay_ms)
		{
			// Timer is instantiated, and the callback is set, but the timer is disabled.
			this.Timer = new Timer(new TimerCallback(TimerIntervalElapsed), null, Timeout.Infinite, Timeout.Infinite);
			this.Interval = interval_ms;
			this.InitialDelay = initialDelay_ms;
		}

		/* System.Threading.Timer.Change() Behavior
		// Due Time = 0; Period = 0: Invoke Immediately, Timer Disabled
		// Due Time = 0; Period = Inf: Invoke Immediately, Timer Disabled
		// Due Time = 0; Period < Inf: Invoke Immediately, Periodic  on Period
		// Due Time < Inf; Period = 0: Invoke after Due Time, Timer Disabled
		// Due Time < Inf; Period < Inf: Invoke after Due Time, Periodic on Period
		// Due Time < Inf; Period = Inf: Invoke after Due Time, Timer Disabled
		// Due Time = Inf: NEVER INVOKE: Timer Disabled */

		/// <summary>Starts the timer using the current initial delay and interval values.</summary>
		public void Start() => this.Timer.Change(this.InitialDelay, this.Interval);
		/// <summary>Executes the ProcessMethod immediately and only once unless it is currently executing, then stops.</summary>
		public void ExecuteNowAndStop() => this.Timer.Change(0, Timeout.Infinite);
		/// <summary>Starts the timer using no initial delay and the current interval value.</summary>
		public void Resume() => this.Timer.Change(0, this.Interval);
		/// <summary>Stops the timer. If the ProcessMethod is currently executing, it will continue, but no further executions will occur.</summary>
		public void Stop() => this.Timer.Change(Timeout.Infinite, Timeout.Infinite);

		public string GetMetrics()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($@"Interval Elapsed Count: {Metric_IntervalElapsedCount}");
			sb.AppendLine($@"Process Executed Count: {Metric_ProcessExecutedCount}");
			sb.AppendLine($@"Process Skipped Count: {Metric_ProcessSkippedCount}");
			return sb.ToString();
		}

		protected virtual void OnIntervalElapsed() => IntervalElapsed?.Invoke(this, EventArgs.Empty);
		protected virtual void OnAfterProcessCalled() => AfterProcessCalled?.Invoke(this, EventArgs.Empty);
		protected virtual void OnBeforeProcessCalled() => BeforeProcessCalled?.Invoke(this, EventArgs.Empty);

		private void TimerIntervalElapsed(object state)
		{
			// Update Metric
			Interlocked.Increment(ref Metric_IntervalElapsedCount);
			// Send Interval Elapsed everytime the timer Ticks
			OnIntervalElapsed();
			// Interlocked operation to prevent running more than one operation at a time.
			if (Interlocked.CompareExchange(ref _IsProcessing, 1, 0) == 0)
			{
				Metric_ProcessExecutedCount++;
				try { ProcessMethodExecute(); }
				finally { _IsProcessing = 0; }
			}
			else Interlocked.Increment(ref Metric_ProcessSkippedCount);
		}
		protected virtual void ProcessMethodExecute()
		{
			var pMethod = this.ProcessMethod;
			if (pMethod != null)
			{
				// Only send Before/After calls if the method is actually called
				OnBeforeProcessCalled();
				this.ProcessMethod();
				OnAfterProcessCalled();
			}
		}

		/// <summary>Factory Creator</summary>
		/// <param name="processMethod">User defined method to execute at every interval.</param>
		/// <param name="interval_ms">The interval in milliseconds between each execution of the ProcessMethod.</param>
		/// <param name="initialDelay_ms">The initial wait time in milliseconds before the first execution of the ProcessMethod.</param>
		public static BackgroundMaintenanceProcessor Create(Action processMethod, Int32 interval_ms, Int32 initialDelay_ms) =>
			 new BackgroundMaintenanceProcessor(processMethod, interval_ms, initialDelay_ms);
	}

	public class BackgroundMaintenanceProcessor<T> : BackgroundMaintenanceProcessor
	{
		public new Action<T> ProcessMethod { get; set; }
		public T Data { get; set; }
		private BackgroundMaintenanceProcessor(Action<T> processMethod, T data, Int32 interval_ms, Int32 initialDelay_ms = 0)
			: base(interval_ms, initialDelay_ms)
		{
			this.ProcessMethod = processMethod;
			this.Data = data;
		}
		protected override void ProcessMethodExecute()
		{
			var pMethod = this.ProcessMethod;
			if (pMethod != null)
			{
				// Only send Before/After calls if the method is actually called
				OnBeforeProcessCalled();
				this.ProcessMethod(this.Data);
				OnAfterProcessCalled();
			}
		}

		/// <summary>Factory Creator</summary>
		/// <param name="processMethod">User defined method to execute at every interval.</param>
		/// <param name="data">User defined data that is passed to ProcessMethod at each execution</param>
		/// <param name="interval_ms">The interval in milliseconds between each execution of the ProcessMethod.</param>
		/// <param name="initialDelay_ms">The initial wait time in milliseconds before the first execution of the ProcessMethod.</param>
		public static BackgroundMaintenanceProcessor<T> Create(Action<T> processMethod, T data, Int32 interval_ms, Int32 initialDelay_ms) =>
			new BackgroundMaintenanceProcessor<T>(processMethod, data, interval_ms, initialDelay_ms);
	}
}
