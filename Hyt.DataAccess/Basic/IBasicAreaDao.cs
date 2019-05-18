using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.BaseInfo
{
    /// <summary>
    /// 地区信息数据访问抽象类
    /// </summary>
    /// <remarks>
    /// 2013-06-09 朱成果 创建
    /// </remarks>
    public abstract class IBsAreaDao : DaoBase<IBsAreaDao>
    {
        #region 操作

        /// <summary>
        /// 创建地区信息
        /// </summary>
        /// <param name="model">地区信息实体</param>
        /// <returns>创建的地区信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Create(BsArea model);
        public abstract List<BsArea> GetAllCity();
        /// <summary>
        /// 更新地区信息
        /// </summary>
        /// <param name="model">地区信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Update(BsArea model);

        /// <summary>
        /// 删除地区信息
        /// </summary>
        /// <param name="sysNo">要删除的地区信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int sysNo);
        
        #endregion

        #region 查询

        /// <summary>
        /// 根据地区名称模糊递归出地区列表(指定参数可递归到顶级父节点、默认只递归子节点)
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <param name="getParent">是否获取父级节点</param>
        /// <returns>地区列表</returns>
        /// <remarks>2013-12-16 周唐炬 创建</remarks>
        public abstract List<BsArea> QueryRecursive(string name, bool getParent); 

        /// <summary>
        /// 获取最大的显示顺序,如果为空则显示为0
        /// </summary>
        /// <returns>最大显示顺序,如果为空则显示为0</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int GetMaxDisplayOrder();

        /// <summary>
        /// 获取所有地区
        /// </summary>
        /// <returns>所有地区的集合</returns>
        /// <remarks>
        /// 2013-06-20 何方 创建
        /// </remarks>
        public abstract List<BsArea> GetAll();

        /// <summary>
        /// 获取所有子级地区
        /// </summary>
        /// <param name="parentSysNo">父级地区编号</param>
        /// <returns>所有子级地区</returns>
        /// <remarks> 
        /// 2013-08-12 郑荣华 创建
        /// </remarks>
        public abstract IList<BsArea> GetAreaList(int parentSysNo);
        
        /// <summary>
        /// 模糊查询字段地区名称,代码,,拼音
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <returns>
        /// 所有地区的集合
        /// </returns>
        /// <remarks>
        /// 2013-06-20 何方 创建
        /// </remarks>
        public abstract IList<BsArea> Search(string keyword);

        /// <summary>
        /// 获取省
        /// </summary>
        /// <returns>省列表</returns>
        /// <remarks>
        /// 2013-06-08 朱成果 创建
        /// </remarks>
        public abstract IList<BsArea> LoadProvince();

        /// <summary>
        /// 获取市
        /// </summary>
        /// <returns>市列表</returns>
        /// <remarks>
        /// 2013-06-08 朱成果 创建
        /// </remarks>
        public abstract IList<BsArea> LoadCity(int provinceSysNo);

        /// <summary>
        /// 获取区
        /// </summary>
        /// <returns>地区列表</returns>
        /// <remarks>
        /// 2013-06-08 朱成果 创建
        /// </remarks>
        public abstract IList<BsArea> LoadArea(int citySysNo);

        /// <summary>
        /// 获取省市区数据
        /// </summary>
        /// <param name="parentSysNo">地区父级系统号</param>
        /// <returns>省市区数据</returns>
        /// <remarks>
        /// 2013-08-02 杨晗 创建
        /// </remarks>
        public abstract IList<BsArea> SelectArea(int parentSysNo);

        /// <summary>
        /// 查询下级地区，排除没有仓库的地区
        /// </summary>
        /// <param name="parentSysNo">上级地区编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <returns>省市区数据</returns>
        /// <remarks> 2013-07-02 朱成果 创建 </remarks>
        /// <remarks> 2013-10-08 黄志勇 修改 </remarks>
        public abstract IList<BsArea> SelectAreaWithWarehouse(int parentSysNo, int? warehouseType, int? deliveryTypeSysNo);

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-08-02 杨晗 创建
        /// </remarks>
        public abstract BsArea GetArea(int sysNo);

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-08-02 杨晗 创建
        /// </remarks>
        public abstract CBBsArea2 GetCbArea(int sysNo);

        /// <summary>
        /// 根据地区名称获取地区列表 模糊查询
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>地区列表</returns>
        /// <remarks>
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [Obsolete("已弃用")]
        public abstract IList<BsArea> GetArea(string areaName);

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <param name="status">地区状态</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public abstract BsArea GetArea(int sysNo, BasicStatus.地区状态 status);

        /// <summary>
        /// 根据地区编号，获取省市区信息
        /// </summary>
        /// <param name="sysNo">地区编号</param>
        /// <param name="cityEntity">城市信息</param>
        /// <param name="areaEntity">地区信息</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-14 朱成果 创建
        /// </remarks>
        [Obsolete("已弃用")]
        public abstract BsArea GetProvinceEntity(int sysNo, out BsArea cityEntity,
                                                       out BsArea areaEntity);

        #endregion

        /// <summary>
        /// 根据区县编号判定是否支持百城当日达
        /// </summary>
        /// <param name="sysNo">区县编号</param>
        /// <returns>是：支持，否：不支持</returns>
        /// <remarks>2013-9-20 周瑜 创建</remarks>
        public abstract bool InDeliveryArea(int sysNo);

        /// <summary>
        /// 获取所有地区,用于构建地区树
        /// </summary>
        /// <param></param>
        /// <returns>所有地区</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public abstract IList<BsArea> GetAllAreaForTree();

        /// <summary>
        /// 获取仓库的覆盖地区
        /// </summary>
        /// <param name="warehouseSysNo">创编系统编号</param>
        /// <returns>仓库的覆盖地区</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public abstract IList<CBBsArea2> GetAreaByWarehouse(int warehouseSysNo);

        /// <summary>
        /// 修改地区状态
        /// </summary>
        /// <param name="area">地区实体</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        public abstract int UpdateStatus(BsArea area);

        /// <summary>
        /// 淘宝地区与匹配商城地区
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="districtName">地区名称</param>
        /// <returns></returns>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public abstract BsArea GetMatchDistrict(string cityName, string districtName);
        
    }
}
