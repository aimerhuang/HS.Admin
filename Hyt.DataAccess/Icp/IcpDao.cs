using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;


namespace Hyt.DataAccess.Icp
{
    /// <summary>
    /// 取商检数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public abstract class IcpDao : DaoBase<IcpDao>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract Pager<CIcp> GoodsQuery(ParaIcpGoodsFilter filter);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract Pager<CIcp> OrderQuery(ParaIcpGoodsFilter filter);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract Pager<CBIcpGoodsItem> IcpGoodsItemQuery(ParaIcpGoodsFilter filter);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract CIcp GetEntity(int sysNo);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="MessageID">编号</param>
        /// <param name="MessageType">类型</param>
        /// <returns>数据实体</returns>
        /// <remarks>2016-03-23  王耀发 创建</remarks>
        public abstract CIcp GetEntityByMessageIDType(string MessageID, string MessageType);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="MessageType">报文类型</param>
        /// <returns></returns>
        public abstract CIcp GetEntityByMType(string IcpType, string MessageType);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract int Insert(CIcp entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract void Update(CIcp entity);

        /// <summary>
        /// 获取明细列表
        /// </summary>
        /// <param name="IcpGoodsSysNo">系统编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2016-03-24  王耀发 创建</remarks>
        public abstract List<CBIcpGoodsItem> GetListByIcpGoodsSysNo(int IcpGoodsSysNo);

        /// <summary>
        /// 删除商检明细数据
        /// </summary>
        /// <param name="IcpGoodsSysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2016-03-24  王耀发 创建</remarks>
        public abstract void DeleteIcpGoodsItem(int IcpGoodsSysNo);
        /// <summary>
        /// 插入商检明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract int InsertIcpGoodsItem(CIcpGoodsItem entity);
        /// <summary>
        /// 更新商检明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract void UpdateIcpGoodsItem(CIcpGoodsItem entity);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract Pager<CBIcpGoodsItem> GetIcpProductList(ParaIcpGoodsItemFilter filter);
        /// <summary>
        /// 更新接收回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="DocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateIcpDocRec(int SysNo, string DocRec);
        /// <summary>
        /// 更新单一窗口平台接收回执
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="DocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdatePlatDocRecByMessageID(string MessageID, string PlatDocRec, string PlatStatus);
        /// <summary>
        /// 更新商检接收回执
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="CiqDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateCiqDocRecByMessageID(string MessageID, string CiqDocRec, string CiqStatus);
        /// <summary>
        /// 更新商检接收状态
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="CiqDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateStatus(string MessageID, int Status);
        /// <summary>
        /// 更新国检审核回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="CiqRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateIcpCiqRec(int SysNo, string CiqRec);
        /// <summary>
        /// 更新国检审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateIcpGoodsItemCIQ(int IcpType, string EntGoodsNo, string CIQGRegStatus, string CIQNotes);

        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="CusRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateIcpCusRec(int SysNo, string CusRec);
        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="OpResult">状态</param>
        /// <param name="CustomsNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateIcpGoodsItemCUS(int IcpType, string EntGoodsNo, string OpResult, string CustomsNotes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract IcpBYJiChangGoodsInfo GetIcpBYJiChangGoodsInfoEntityByPid(int ProductSysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract int InsertIcpBYJiChangGoodsInfo(IcpBYJiChangGoodsInfo entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 更新</remarks>
        public abstract int UpdateIcpBYJiChangGoodsInfoEntity(IcpBYJiChangGoodsInfo entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract IcpGZNanShaGoodsInfo GetIcpGZNanShaGoodsInfoEntityByPid(int ProductSysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract int InsertIcpGZNanShaGoodsInfo(IcpGZNanShaGoodsInfo entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 更新</remarks>
        public abstract int UpdateIcpGZNanShaGoodsInfoEntity(IcpGZNanShaGoodsInfo entity);
        /// <summary>
        /// 更新检验检疫商品备案编号
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateCIQGoodsNo(string EntGoodsNo, string CIQGoodsNo);

        /// <summary>
        /// 更新检验检疫商品备案编号
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateCusGoodsNo(string EntGoodsNo, string CusGoodsNo);

        /// <summary>
        /// 更新南沙检验检疫商品备案编号
        /// </summary>
        /// <param name="Gcode">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public abstract void UpdateNSCIQGoodsNo(string Gcode, string CIQGoodsNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public abstract void UpdateEntGoodsNoByMessageID(string MessageID, string EntGoods);

        /// <summary>
        /// 获取所有商品备案信息
        /// </summary>
        /// <returns>商品备案信息集合</returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public abstract IList<IcpGZNanShaGoodsInfo> GetAllGZNanShaGoodsInfo();

        /// <summary>
        /// 新增商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void CreateExcelGZNanShaGoodsInfo(List<IcpGZNanShaGoodsInfo> models);

        /// <summary>
        /// 更新商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelGZNanShaGoodsInfo(List<IcpGZNanShaGoodsInfo> models);

        /// <summary>
        /// 根据商品ID获取启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public abstract IcpQiBangGoodsInfo GetIcpQiBangGoodsInfoEntityByPid(int ProductSysNo);
        /// <summary>
        /// 新增启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public abstract int InsertIcpBYJiChangGoodsInfo(IcpQiBangGoodsInfo entity);
        /// <summary>
        /// 更新启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public abstract int UpdateIcpBYJiChangGoodsInfoEntity(IcpQiBangGoodsInfo entity);
    }
}