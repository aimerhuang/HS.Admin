using Hyt.DataAccess.Sys;
using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 升级数据库
    /// </summary>
    /// <remarks>2017-1-10 杨浩 创建</remarks>
    public class SyUpgradeTheDatabaseBo
    {    
        /// <summary>
        /// 更新升级配置文件
        /// </summary>
        /// <param name="upgradeConfig"></param>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        private static void UpdateUpgradeConfig(decimal? version)
        {
            var _config = new UpgradeConfig()
            {
                Version = version,
                SqlContent = ""
            };
            BLL.Config.Config.Instance.UpdateConfig(_config,"Upgrade.config");                         
        }
        /// <summary>
        /// 升级
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public static bool Upgrade()
        {
            try
            {
                var config =(new BLL.Config.Config()).GetUpgradeConfig();

                if (config.Version == null)               
                    config.Version = 0;
                
                decimal? version=ISyUpgradeTheDatabaseDao.Instance.Version;
                if (config.Version < version)
                {
                    if(ISyUpgradeTheDatabaseDao.Instance.Upgrade())
                        UpdateUpgradeConfig(version);
                }
                    
            }
            catch (Exception ex)
            {
                BLL.Log.LocalLogBo.Instance.Write(ex, "UpgradeLog");
                return false;
            }
           
            return true;
        }
    }
}
