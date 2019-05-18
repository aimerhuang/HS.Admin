using System;
using System.Web;

namespace Hyt.BLL.ScheduledEvents
{
	/// <summary>
	/// Interface for defining an event.
	/// </summary>
	public interface IEvent
	{
		void Execute(object state);
	}
}
