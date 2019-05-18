using System;
using System.Diagnostics;
using System.Web;
using System.Threading;
using System.Collections.Generic;
using Hyt.Util;



namespace Hyt.BLL.ScheduledEvents
{
	/// <summary>
	/// EventManager is called from the EventHttpModule (or another means of scheduling a Timer). Its sole purpose
	/// is to iterate over an array of Events and deterimine of the Event's IEvent should be processed. All events are
	/// added to the managed threadpool. 
	/// </summary>
	public class EventManager
	{
        public static string RootPath;

        private static Event[] items;
		private EventManager()
		{
		}

		public static readonly int TimerMinutesInterval = 5;
        static EventManager()
        {
            
            if (Config.Config.Instance.GetScheduleConfig().TimerMinutesInterval > 0)
            {
                TimerMinutesInterval = Config.Config.Instance.GetScheduleConfig().TimerMinutesInterval;
            }
        }

        public static void UpdateTimeByKeyAndDealerSysNo(string key, int dealerSysNo)
        {
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (key == items[i].Key && items[i].DealerSysNo == dealerSysNo)
                    {
                        items[i].LastCompleted = DateTime.Now;
                        items[i].dateWasSet = true;
                    }
                                          
                }
            }
        }
		public static void Execute()
        {
            Hyt.Model.Common.Event[] simpleItems = Config.Config.Instance.GetScheduleConfig().Events;
            //Event[] items;

            if (items == null)
            {
                List<Event> list = new List<Event>();

                var allDealer = Hyt.BLL.Stores.StoresBo.Instance.GetAllStores();
                foreach (var d in allDealer)
                {
                    if (string.IsNullOrWhiteSpace(d.AppID) || string.IsNullOrWhiteSpace(d.AppSecret))
                        continue;

                    foreach (Hyt.Model.Common.Event newEvent in simpleItems)
                    {
                        if (!newEvent.Enabled)
                        {
                            continue;
                        }
                        Event e = new Event();
                        e.Key = newEvent.Key;
                        e.Minutes = newEvent.Minutes;
                        e.ScheduleType = newEvent.ScheduleType;
                        e.TimeOfDay = newEvent.TimeOfDay;
                        e.DealerSysNo = d.SysNo;
                        list.Add(e);
                    }
                }

                items = list.ToArray();
            }

            Event item = null;
			
			if(items != null)
			{				
				for(int i = 0; i<items.Length; i++)
				{
					item = items[i];
					if(item.ShouldExecute)
					{
						item.UpdateTime();
						IEvent e = item.IEventInstance;
						ManagedThreadPool.QueueUserWorkItem(new WaitCallback(e.Execute),item.DealerSysNo);
					}
				}
			}
		}
	}
}
