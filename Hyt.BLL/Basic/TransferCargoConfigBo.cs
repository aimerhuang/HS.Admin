using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util;
using System.IO;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 调货配置业务
    /// </summary>
    /// <remarks>2016-04-05 杨浩 创建</remarks>
    public class TransferCargoConfigBo : BOBase<TransferCargoConfigBo>
    {
        #region 根据“申请调货仓库编号”获取“配货仓库编号”
        /// <summary>
        /// 根据“申请调货仓库编号”获取“配货仓库编号”
        ///</summary>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>配货仓库编号</returns>
        /// <remarks>2016-04-05 王江 创建</remarks>
        public int GetDeliveryWarehouseSysNoByApplyWarehouseSysNo(int applyWarehouseSysNo)
        {
            return ITransferCargoConfigDao.Instance.GetDeliveryWarehouseSysNoByApplyWarehouseSysNo(applyWarehouseSysNo);
        }
        #endregion

        #region 根据 配货仓库编号 获取 申请调货仓库列表

        /// <summary>
        /// 根据 配货仓库编号 获取 申请调货仓库列表
        ///</summary>
        /// <param name="deliveryWarehouseSysNo">配货仓库编号</param>
        /// <returns>申请调货仓库列表</returns>
        /// <remarks> 2016-04-06 王江 创建</remarks>
        public IList<CBTransferCargoConfig> GetApplyWarehouseListByDeliveryWarehouseSysNo(int deliveryWarehouseSysNo)
        {
            return ITransferCargoConfigDao.Instance.GetApplyWarehouseListByDeliveryWarehouseSysNo(deliveryWarehouseSysNo);
        }

        #endregion

        #region 获取所有已存在的申请调货仓库

        /// <summary>
        /// 获取所有已存在的申请调货仓库
        ///</summary>
        /// <returns>已存在的申请调货仓库结果集</returns>
        /// <remarks> 2016-04-19 王江 创建</remarks>
        public IList<int> GetAllApplyWarehouseSysNo()
        {
            return ITransferCargoConfigDao.Instance.GetAllApplyWarehouseSysNo();
        }

        #endregion

        #region 界面分页列表

        /// <summary>
        /// 界面分页列表
        /// </summary>
        /// <param name="filter">筛选字段</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-10-9 王江 创建</remarks>
        public Pager<CBTransferCargoConfig> GetPagerList(ParaTransferCargoConfigFilter filter)
        {
            return ITransferCargoConfigDao.Instance.GetTransferCargoConfigList(filter);
        }

        #endregion

        #region 调货配置-添加

        /// <summary>
        /// 调货配置-添加
        /// </summary>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号集</param>
        /// <param name="deliveryWarehouseSysNo">配货仓库编号</param>
        /// <param name="currentUserSysNo">当前用户编号</param>
        /// <param name="isExcelImport">是否为Excel导入方式</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2016-04-06 王江 创建</remarks>
        public Result Add(List<int> applyWarehouseSysNo, int deliveryWarehouseSysNo, int currentUserSysNo)
        {
            Result result = new Result();

            if (applyWarehouseSysNo.Contains(deliveryWarehouseSysNo))
            {
                result.Status = false;
                result.Message = "配货仓库不能指定自身";
                return result;
            }
            var deliveryWarehouseDetail = WhWarehouseBo.Instance.GetWarehouse(deliveryWarehouseSysNo);
            if (deliveryWarehouseDetail == null)
            {
                result.Status = false;
                result.Message = "配货仓库不存在";
                return result;
            }

            IList<TransferCargoConfig> transferCargoConfigList = new List<TransferCargoConfig>();

            applyWarehouseSysNo.ForEach(t =>
            {
                transferCargoConfigList.Add(new TransferCargoConfig()
                {
                    ApplyWarehouseSysNo = t,
                    DeliveryWarehouseSysNo = deliveryWarehouseSysNo,
                    Status = 1,
                    CreatedDate = DateTime.Now,
                    CreatedBy = currentUserSysNo,
                    LastUpdateBy = currentUserSysNo,
                    LastUpdateDate = DateTime.Now
                });
            });
            using (var scope = new System.Transactions.TransactionScope())
            {
                bool canInsert = true;
                int exexSuccess = 0;
                foreach (var item in transferCargoConfigList)
                {
                    // 根据申请调货仓库编号判断申请调货仓库是否存在
                    var applyWarehouseDetail = WhWarehouseBo.Instance.GetWarehouse(item.ApplyWarehouseSysNo);
                    if (applyWarehouseDetail == null)
                    {
                        continue;
                    }

                    // 判断是否已添加过
                    var entity = ITransferCargoConfigDao.Instance.QuerySingle(item.DeliveryWarehouseSysNo, item.ApplyWarehouseSysNo);
                    if (entity != null)
                    {
                        var applyWarehouse = WhWarehouseBo.Instance.GetWarehouseEntity(entity.ApplyWarehouseSysNo);
                        var deliveryWarehouse = WhWarehouseBo.Instance.GetWarehouseEntity(entity.DeliveryWarehouseSysNo);
                        result.Status = false;
                        result.Message = string.Format("申请调货仓库：{0}   配货仓库：{1} 关联配置已存在", applyWarehouse.WarehouseName, deliveryWarehouse.WarehouseName);
                        canInsert = false;
                        break;
                    }

                    // 申请调货仓库只能有一个配货仓库
                    var relationdeliveryWarehouse = ITransferCargoConfigDao.Instance.GetDeliveryWarehouseSysNoByApplyWarehouseSysNo(item.ApplyWarehouseSysNo);

                    if (relationdeliveryWarehouse > 0)
                    {
                        var applyWarehouse = WhWarehouseBo.Instance.GetWarehouseEntity(item.ApplyWarehouseSysNo);
                        var deliveryWarehouse = WhWarehouseBo.Instance.GetWarehouseEntity(relationdeliveryWarehouse);
                        result.Status = false;
                        result.Message = string.Format("申请调货仓库：{0} 已关联 配货仓库：{1}", applyWarehouse.WarehouseName, deliveryWarehouse.WarehouseName);
                        canInsert = false;
                        break;
                    }

                    int lastId = ITransferCargoConfigDao.Instance.Insert(item);
                    exexSuccess = lastId > 0 ? ++exexSuccess : exexSuccess;
                }
                if (canInsert)
                {
                    scope.Complete();
                    result.Status = true;
                    result.Message = string.Format("待导入记录共：{0} 条,导入成功：{1} 条", transferCargoConfigList.Count, exexSuccess);
                }
            }
            return result;
        }

        #endregion

        #region 调货配置-删除

        /// <summary>
        /// 调货配置-删除
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>是否删除成功</returns>
        /// <remarks>2015-10-9 王江 创建</remarks>
        public Result Delete(List<int> sysNo)
        {
            Result result = new Result();

            try
            {
                if (sysNo == null || !sysNo.Any())
                {
                    result.Message = "无待删除数据";
                    return result;
                }

                using (var scope = new System.Transactions.TransactionScope())
                {
                    bool deleteStatus = true;
                    foreach (var item in sysNo)
                    {
                        var execResult = ITransferCargoConfigDao.Instance.Delete(item) > 0;
                        if (!execResult)
                        {
                            deleteStatus = false;
                            result.Message = "删除失败";
                            break;
                        }
                    }
                    if (deleteStatus)
                    {
                        scope.Complete();
                        result.Status = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }
            return result;
        }

        #endregion

        #region 验证Excel相关信息及导入

        /// <summary>
        /// 验证Excel相关信息及导入
        /// </summary>
        /// <param name="fileBase">上载文件对象</param>
        /// <param name="currentUserSysNo">当前操作用户</param>
        /// <returns></returns>
        public Result ValidateAndImport(HttpPostedFileBase fileBase, int currentUserSysNo)
        {
            Result result = new Result();

            try
            {
                if (fileBase == null || fileBase.ContentLength <= 0)
                {
                    result.Message = "文件不能为空";
                    return result;
                }

                // 获取上传文件的大小
                int fileSize = fileBase.ContentLength;
                int maxSize = 3 * 1024 * 1024;
                if (fileSize >= maxSize)
                {
                    result.Message = "Excel文件不能超过3M";
                    return result;
                }

                string fileName = Path.GetFileName(fileBase.FileName);
                string fileType = ".xls";//定义上传文件的类型字符串
                string fileExtensionName = Path.GetExtension(fileName); // 获取上传文件的扩展名
                if (fileType != fileExtensionName)
                {
                    result.Message = "文件类型错误,Excel文件类型 .xls";
                    return result;
                }

                // 读取并导入Excel数据
                result = this.ImportData(fileBase.InputStream, currentUserSysNo);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }

            return result;
        }

        /// <summary>
        /// Excel Stream
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="currentUserSysNo">当前操作用户</param>
        /// <returns>导入结果</returns>
        /// <remarks>2016-04-20 王江 创建</remarks>
        private Result ImportData(Stream stream, int currentUserSysNo)
        {
            Result result = new Result();
            DataTable dataTable = null;

            var cols = new Dictionary<string, string>
            {
                {"DeliveryWarehouseSysNo", "配货仓库编号"},
                {"ApplyWarehouseSysNo", "申请调货仓库编号"}
            }.Select(p => p.Value).ToArray();

            try
            {
                dataTable = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
            if (dataTable == null || dataTable.Columns.Count == 0)
            {
                result.Message = "未选择Excel文件";
                return result;
            }

            List<TransferCargoConfig> transferCargoConfigList = new List<TransferCargoConfig>();
            foreach (DataRow dr_item in dataTable.Rows)
            {
                int deliveryWarehouseSysNo = 0;
                int.TryParse(dr_item["配货仓库编号"].ToString().Trim(), out deliveryWarehouseSysNo);

                int applyWarehouseSysNo = 0;
                int.TryParse(dr_item["申请调货仓库编号"].ToString().Trim(), out applyWarehouseSysNo);

                transferCargoConfigList.Add(new TransferCargoConfig()
                {
                    ApplyWarehouseSysNo = applyWarehouseSysNo,
                    DeliveryWarehouseSysNo = deliveryWarehouseSysNo,
                    Status = 1,
                    CreatedDate = DateTime.Now,
                    CreatedBy = currentUserSysNo,
                    LastUpdateDate = DateTime.Now,
                    LastUpdateBy = currentUserSysNo
                });
            }

            result = this.AddNew(transferCargoConfigList);
            return result;
        }

        #endregion

        #region 调货配置-Excel导入添加

        /// <summary>
        /// 调货配置-添加-Excel导入
        /// </summary>
        /// <param name="transferCargoConfig">调货配置</param>
        /// <returns>是否添加成功</returns>
        /// <remarks>2016-04-06 王江 创建</remarks>
        public Result AddNew(List<TransferCargoConfig> transferCargoConfig)
        {
            Result result = new Result();

            using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                int exexSuccess = 0;
                foreach (var item in transferCargoConfig)
                {
                    // 根据申请调货仓库编号判断申请调货仓库是否存在
                    var applyWarehouseDetail = WhWarehouseBo.Instance.GetWarehouse(item.ApplyWarehouseSysNo);
                    if (applyWarehouseDetail == null)
                    {
                        continue;
                    }

                    var deliveryWarehouseDetail = WhWarehouseBo.Instance.GetWarehouse(item.DeliveryWarehouseSysNo);
                    if (deliveryWarehouseDetail == null)
                    {
                        continue;
                    }

                    // 判断是否已添加过
                    var entity = ITransferCargoConfigDao.Instance.QuerySingle(item.DeliveryWarehouseSysNo, item.ApplyWarehouseSysNo);
                    if (entity != null)
                    {
                        continue;
                    }

                    // 申请调货仓库只能有一个配货仓库
                    var relationdeliveryWarehouse = ITransferCargoConfigDao.Instance.GetDeliveryWarehouseSysNoByApplyWarehouseSysNo(item.ApplyWarehouseSysNo);

                    if (relationdeliveryWarehouse > 0)
                    {
                        continue;
                    }

                    int lastId = ITransferCargoConfigDao.Instance.Insert(item);
                    exexSuccess = lastId > 0 ? ++exexSuccess : exexSuccess;
                }
                scope.Complete();
                result.Status = true;
                result.Message = string.Format("待导入记录共：{0} 条,导入成功：{1} 条", transferCargoConfig.Count, exexSuccess);
            }
            return result;
        }

        #endregion

        #region 查询可用的调货配置表
        /// <summary>
        /// 查询可用的调货配置表
        ///</summary>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>调货配置表</returns>
        /// <remarks> 2016-04-05 朱成果 创建</remarks>
        public TransferCargoConfig GetEntityByApplyWarehouseSysNo(int applyWarehouseSysNo)
        {
            return ITransferCargoConfigDao.Instance.GetEntityByApplyWarehouseSysNo(applyWarehouseSysNo);
        }
        #endregion
    }
}
