namespace Hyt.Service.Task.Core
{
    /// <summary>
    /// 任务执行接口
    /// </summary>
    /// <remarks>2013-10-11 杨浩 创建</remarks>
    public interface ITask
    {
        /// <summary>
        /// 执行任务的方法
        /// </summary>
        /// <param name="state">执行参数</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 创建</remarks>
        void Execute(object state);
    }
}
