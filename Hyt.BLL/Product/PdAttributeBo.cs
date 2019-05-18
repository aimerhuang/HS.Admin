using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Infrastructure.Caching;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 属性维护
    /// </summary>
    /// <remarks>2013-06-28 唐永勤 创建</remarks>  
    public class PdAttributeBo : BOBase<PdAttributeBo>
    {
        /// <summary>
        /// 获取指定编号的属性信息
        /// </summary>
        /// <param name="sysNo">属性编号</param>
        /// <returns>属性实体信息</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public PdAttribute GetEntity(int sysNo)
        {
            return IPdAttributeDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 保存属性组信息
        /// </summary>
        /// <param name="model">属性组实体信息</param>
        /// <param name="listAttribute">属性组包含属性列表</param>
        /// <returns>保存的结果信息</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public Result SavePdAttribute(PdAttribute model, IList<PdAttributeOption> listAttribute)
        {
            Result result = new Result();
            if (model == null || string.IsNullOrEmpty(model.AttributeName))
            {
                result.StatusCode = -1;
            }
            else
            {
                //数据重复性检测
                bool isExists = IPdAttributeDao.Instance.IsExists(model);
                if (isExists)
                {
                    result.StatusCode = -2;
                    result.Message = "属性名称和后台显示名称已存在";
                    return result;
                }

                //数据操作
                if (model.SysNo > 0)
                {
                    model.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    result.Status = IPdAttributeDao.Instance.Update(model) == true;
                }
                else
                {
                    model.Status = 0;//初始化
                    model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    model.SysNo = IPdAttributeDao.Instance.Create(model);
                    result.Status = model.SysNo > 0 ? true : false; 
                    if (model.SysNo > 0)
                    {
                        result.StatusCode = 1;
                    }
                }
                if (result.Status && model.AttributeType == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品属性类型.选项类型)
                {
                    
                        if (SaveAttributeOptions(model.SysNo, listAttribute) == false)
                        {
                            result.StatusCode = -3;
                            result.Message = "属性选项保存失败";
                        }
                }
            }
            return result;

        }

        /// <summary>
        /// 设置属性组属性
        /// </summary>
        /// <param name="sysno">属性编号</param>
        /// <param name="listAttribute">属性列表</param>
        /// <returns>成功返回true，失败返回false</returns>       
        /// <remarks>2013-07-06 唐永勤 创建</remarks>
        public bool SaveAttributeOptions(int sysno, IList<PdAttributeOption> listAttributeOptions)
        {
            IList<PdAttributeOption> list = new List<PdAttributeOption>();
            int order = 0;
            foreach (PdAttributeOption attributeOption in listAttributeOptions)
            {
                order++;
                PdAttributeOption entity = new PdAttributeOption
                {
                    SysNo = attributeOption.SysNo,
                    AttributeSysNo = sysno,
                    Status = attributeOption.Status,
                    AttributeText = attributeOption.AttributeText,                    
                    CreatedDate = DateTime.Now,
                    DisplayOrder = order,
                    CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateDate = DateTime.Now
                };
                list.Add(entity);
            }
            return IPdAttributeDao.Instance.SetAttributeOptions(sysno, list);

        }

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="pager">属性查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public void GetPdAttributeList(ref Pager<PdAttribute> pager)
        {
             IPdAttributeDao.Instance.GetPdAttributeList(ref pager);
        }

        /// <summary>
        /// 更新属性组状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">属性组编号集</param>
        /// <returns>跟新结果状态</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public Result UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.商品属性状态 status, List<int> listSysNo)
        {
            Result result = new Result();
            if (listSysNo.Count < 1)
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "未设置要更新的记录";
                return result;
            }
            int rowEffect = IPdAttributeDao.Instance.UpdateStatus(status, listSysNo);
            if (rowEffect > 0)
            {
                result.Status = true;
                result.StatusCode = 1;
            }
            else 
            {
                result.Status = false;
                result.StatusCode = 0;
                result.Message = "更新状态失败";
            }
            return result;
            
        }

        /// <summary>
        /// 获取属性所有选项
        /// </summary>
        /// <param name="attributeSysNo">属性编号</param>
        /// <returns>属性选项列表</returns>
        /// <remarks>2013-07-09 唐永勤 创建</remarks>
        public IList<PdAttributeOption> GetAttributeOptions(int attributeSysNo)
        {
            return IPdAttributeDao.Instance.GetAttributeOptions(attributeSysNo);
        }

        /// <summary>
        /// 获取已选的属性列表
        /// </summary>
        /// <param name="listSysNo">属性编号列表</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-10 唐永勤 创建</remarks>
        public IList<PdAttribute> GetSelectedAttributes(IList<int> listSysNo)
        {
            return IPdAttributeDao.Instance.GetSelectedAttributes(listSysNo);
        }

        /// <summary>
        /// 通过商品系统编号获取商品的关联属性
        /// </summary>
        /// <param name="productSysNoList">商品属性ID</param>
        /// <returns>返回属性列表</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public IList<PdAttribute> GetProductAssociationAttribute(int[] productSysNoList)
        {

            return IPdAttributeDao.Instance.GetProductAssociationAttribute(productSysNoList);
        }

         /// <summary>
        /// 判断属性选项是否被商品使用
        /// </summary>
        /// <param name="sysno">选项编号</param>
        /// <returns>被使用返回true，未被使用返回false</returns>
        /// <remarks>2013-07-30 唐永勤 创建</remarks>
        public bool IsAttributeOptionsInProduct(int sysno)
        {
            return IPdAttributeDao.Instance.IsAttributeOptionsInProduct(sysno); 
        }

        /// <summary>
        /// 获取商品属性列表
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <returns>商品属性列表</returns>
        /// <remarks>
        /// 2013-08-22 郑荣华 创建
        /// </remarks>
        public IList<PdAttribute> GetPdAttributeList(int categorySysNo)
        {
            return IPdAttributeDao.Instance.GetPdAttributeList(categorySysNo);
        }

        /// <summary>
        /// 获取商品属性关联
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>商品属性关联列表</returns>
        /// <remarks>
        /// 2013-08-22 郑荣华 创建
        /// </remarks>
        public IList<PdProductAttribute> GetPdProductAttributeList(int pdSysNo)
        {
            return IPdAttributeDao.Instance.GetPdProductAttributeList(pdSysNo);
        }
    }
}
