using System.Collections.Generic;
using System.Linq;
using Hyt.BLL.Log;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using System;
using Hyt.Util;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.Warehouse
{

    /// <summary>
    /// 仓库覆盖地区业务处理类
    /// </summary>
    /// <remarks>2013-08-12 周瑜 创建</remarks>
    public class WhWarehouseAreaBo : BOBase<WhWarehouseAreaBo>, IWhWarehouseAreaBo
    {
        /// <summary>
        /// 增加仓库覆盖地区
        /// </summary>
        /// <param name="warehouse">仓库地区关联实体</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public bool Insert(WhWarehouseArea warehouse)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "增加仓库覆盖地区", LogStatus.系统日志目标类型.仓库, warehouse.SysNo);
            //清除仓库地区缓存
            MemoryProvider.Default.Remove(KeyConstant.WhwarehouseAreaList);
            return IWhWarehouseAreaDao.Instance.Insert(warehouse) > 0;
        }

        /// <summary>
        /// 修改仓库覆盖地区
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns>是否修改成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public bool Update(WhWarehouseArea warehouse)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改仓库覆盖地区", LogStatus.系统日志目标类型.仓库, warehouse.SysNo);
            //清除仓库地区缓存
            MemoryProvider.Default.Remove(KeyConstant.WhwarehouseAreaList);
            return IWhWarehouseAreaDao.Instance.Update(warehouse) > 0;
        }

        /// <summary>
        /// 删除仓库覆盖地区
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public bool Delete(int areaSysNo, int warehouseSysNo)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除仓库覆盖地区", LogStatus.系统日志目标类型.仓库, warehouseSysNo);
            //清除仓库地区缓存
            MemoryProvider.Default.Remove(KeyConstant.WhwarehouseAreaList);
            return IWhWarehouseAreaDao.Instance.Delete(areaSysNo, warehouseSysNo) > 0;
        }

        /// <summary>
        /// 将该仓库设为选中地区的默认发货仓库
        /// </summary>
        /// <param name="whWarehouseArea">地区仓库实体</param>
        /// <param name="status">是否默认仓库，默认：是</param>
        /// <returns>是否设置成功</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public bool SetDefault(WhWarehouseArea whWarehouseArea, WarehouseStatus.是否默认仓库 status = WarehouseStatus.是否默认仓库.是)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "将该仓库设为选中地区的默认发货仓库", LogStatus.系统日志目标类型.仓库, whWarehouseArea.WarehouseSysNo);
            //清除仓库地区缓存
            MemoryProvider.Default.Remove(KeyConstant.WhwarehouseAreaList);
            return IWhWarehouseAreaDao.Instance.SetDefault(whWarehouseArea,status) > 0;
        }

        /// <summary>
        /// 根据地区编号获取仓库
        /// </summary>
        /// <param name="areaSysNo">地区编号</param>
        /// <returns>返回仓库集合</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        public IList<CBWhWarehouse> GetWarehouseForArea(int areaSysNo)
        {
            return IWhWarehouseAreaDao.Instance.GetWarehouseForArea(areaSysNo);
        }

        /// <summary>
        /// 获取所有的仓库覆盖地区数据，先从缓存中获取
        /// </summary>
        /// <returns>仓库覆盖地区数据</returns>
        /// <remarks>2014-05-15 朱成果 创建</remarks>
        public IList<WhWarehouseArea> GetAllWhWarehouseArea()
        {
            var list = MemoryProvider.Default.Get<IList<WhWarehouseArea>>(KeyConstant.WhwarehouseAreaList, () => IWhWarehouseAreaDao.Instance.GetAllWhWarehouseArea());
            return list;
        }

        /// <summary>
        /// 仓库覆盖地区增加删除
        /// </summary>
        /// <param name="areaSysNo">传入的地区系统编号 用","链接</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        /// <remarks>2013-11-06 郑荣华 重构 </remarks>
        /// <remarks>2013-11-21 沈强 修改 </remarks>
        public void SaveWarehouseArea(string areaSysNo, int warehouseSysNo,int currentUserSysNo)
        {
            if (areaSysNo == "")
            {
                areaSysNo = "0:0";
            }
            var dtSysNo = areaSysNo.Split(',');

            var model = new WhWarehouseArea
            {
                CreatedBy = currentUserSysNo,
                CreatedDate = DateTime.Now,
                LastUpdateBy = currentUserSysNo,
                LastUpdateDate = DateTime.Now,
                WarehouseSysNo = warehouseSysNo,
                IsDefault = (int)WarehouseStatus.是否默认仓库.否
            };

            //var dicList = dtSysNo.ToDictionary(item => int.Parse(item.Split(':')[0]), item => int.Parse(item.Split(':')[1]));
            var dicList = new Dictionary<int, int>();
            dtSysNo.ForEach(d =>
                {
                    var tmp = d.Split(':');
                    var key = int.Parse(tmp[0]);
                    var value = int.Parse(tmp[1]);
                    if (!dicList.ContainsKey(key))
                    {
                        dicList.Add(key,value);
                    }
                });

            var list = dicList.Select(p => p.Key).ToList();

            var areas = Basic.BasicAreaBo.Instance.GetAreaByWarehouse(warehouseSysNo);

            //var dicHadOwn = Basic.BasicAreaBo.Instance.GetAreaByWarehouse(warehouseSysNo)
            //                            .ToDictionary(item => item.SysNo, item => item.IsDefault);
            var dicHadOwn = new Dictionary<int, int>();
            areas.ForEach(a =>
                {
                    if (!dicHadOwn.ContainsKey(a.SysNo))
                    {
                        dicHadOwn.Add(a.SysNo, a.IsDefault);
                    }
                });

            var listHadOwn = dicHadOwn.Select(p => p.Key).ToList();//当前情况
            var listDel = listHadOwn.Except(list).ToList();//要删除的

            //要添加的
            var dicListAdd = dicList.Where(item => !listHadOwn.Contains(item.Key)).ToDictionary(item => item.Key, item => item.Value);

            if (areaSysNo == "0:0")
            {
                dicListAdd.Remove(0);
            }
            //改状态dicList中不一样的，传入的为基准，去掉新加中未设为默认仓库的，去掉交集中状态相同的
            var dicListExcept = dicList.Except(dicHadOwn).ToDictionary(item => item.Key, item => item.Value);//去掉交集中状态相同的
            var dictemp = dicListAdd.Where(item => item.Value == 0).ToDictionary(item => item.Key, item => item.Value);// 新加中未设为默认仓库的
            dicListExcept = dicListExcept.Except(dictemp).ToDictionary(item => item.Key, item => item.Value);
            // 事务，已经有的不插入、多余的要删除
            foreach (var item in dicListAdd)//添加
            {
                model.AreaSysNo = item.Key;
                Instance.Insert(model);
            }
            foreach (var item in listDel)//删除
            {
                Instance.Delete(item, warehouseSysNo);
            }
            foreach (var item in dicListExcept)//改状态
            {
                var warehouseArea = new WhWarehouseArea
                {
                    WarehouseSysNo = warehouseSysNo,
                    LastUpdateBy = currentUserSysNo,
                    LastUpdateDate = DateTime.Now,
                    AreaSysNo = item.Key
                };
                Instance.SetDefault(warehouseArea, (WarehouseStatus.是否默认仓库)item.Value);
            }
        }
    }
}
