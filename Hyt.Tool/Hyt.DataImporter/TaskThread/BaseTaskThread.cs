using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hyt.DataImporter.TaskThread
{
    public delegate void TaskBeginHandler(string name,int total);
    public delegate void TaskGoingHandler(string name, int total, int progress);

   

    public abstract class BaseTaskThread
    {
        public abstract int order { get; }
        public abstract string name {get;}

        private int total = 0;
        private int progress=0;

        private ThreadStart start;
        private Thread thread;

        private TaskBeginHandler OnTaskBegin;
        private TaskGoingHandler OnTaskGoing;

      
        public BaseTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
        {
            this.OnTaskBegin = OnTaskBegin;
            this.OnTaskGoing = OnTaskGoing;
        }

        public void Run()
        {
            start = new ThreadStart(BeginImport);
            thread = new Thread(start);
            thread.Start();
        }

    
        protected virtual void BeginImport()
        {
            total=GetTotal();

            if(OnTaskBegin!=null)
                OnTaskBegin(name, total);

            while(progress<total)
            {
                //progress++;

                if (OnTaskGoing != null)
                    OnTaskGoing(name, total, progress);

                Write(progress);

                progress++;

                Thread.Sleep(100 * 1);
            }
        }

       

        protected abstract int GetTotal();

        protected abstract void Read();

        protected abstract void Write(int rowIndex);
    }
}
