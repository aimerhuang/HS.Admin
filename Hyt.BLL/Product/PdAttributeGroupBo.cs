using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品属性组维护
    /// </summary>
    /// <remarks>2013-06-24 唐永勤 创建</remarks>  
    public class PdAttributeGroupBo : BOBase<PdAttributeGroupBo>
    {
        
        /// <summary>
        /// 保存属性组信息
        /// </summary>
        /// <param name="model">属性组实体信息</param>
        /// <param name="listAttributeSysNo">属性组包含属性列表</param>
        /// <returns>保存的结果信息</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public Result SavePdAttributeGroup(PdAttributeGroup model, IList<int> listAttributeSysNo)
        {
            Result result = new Result();
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                result.StatusCode = -1;
            }
            else
            {
                //数据重复性检测
                bool isExists = IPdAttributeGroupDao.Instance.IsExists(model.Name, model.BackEndName, model.SysNo);
                if (isExists)
                {
                    result.StatusCode = -2;
                    result.Message = "属性组名称和后台显示名称已存在";
                    return result;
                }
               
                //数据操作
                if (model.SysNo > 0)
                {
                    result.StatusCode = (IPdAttributeGroupDao.Instance.Update(model) == true ? 1 : 0);                   
                }
                else
                {
                    model.Status = 0;//初始化
                    model.SysNo = IPdAttributeGroupDao.Instance.Create(model);
                    if (model.SysNo > 0)
                    {
                        result.StatusCode = 1; 
                    }
                }

                

                if (result.StatusCode == 1)
                {
                    if (SetAttributes(model.SysNo, listAttributeSysNo) == false)
                    {
                        result.StatusCode = -3;
                        result.Message = "属性组属性项保存失败";
                    }
                    else
                    {
                        result.Status = true;
                    }
                }
            }
            return result;
           
        }

        /// <summary>
        /// 获取指定ID的属性组信息
        /// </summary>
        /// <param name="brandSysNo">属性组编号</param>
        /// <returns>属性组实体信息</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        public PdAttributeGroup GetEntity(int brandSysNo)
        {
            return IPdAttributeGroupDao.Instance.GetEntity(brandSysNo);
        }

       
        /// <summary>
        /// 更新属性组状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">属性组编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.商品属性分组状态 status, List<int> listSysNo)
        {
            return IPdAttributeGroupDao.Instance.UpdateStatus(status, listSysNo);
        }

        /// <summary>
        /// 获取属性组列表
        /// </summary>
        /// <param name="pager">属性组查询条件</param>
        /// <returns>属性组列表</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        public Pager<PdAttributeGroup> GetPdAttributeGroupList(Pager<PdAttributeGroup> pager)
        {
            return IPdAttributeGroupDao.Instance.GetPdAttributeGroupList(pager);
        }

        /// <summary>
        /// 获取属性组所有属性
        /// </summary>
        /// <param name="attributeGroupSysNo">属性组编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public IList<PdAttribute> GetAttributes(int attributeGroupSysNo)
        {
            return IPdAttributeGroupDao.Instance.GetAttributes(attributeGroupSysNo);
        }

        /// <summary>
        /// 设置属性组属性
        /// </summary>
        /// <param name="sysno">属性组编号</param>
        /// <param name="listAttributeSysNo">属性列表</param>
        /// <returns>成功返回true，失败返回false</returns>       
        /// <remarks>2013-06-28 唐永勤 创建</remarks>
        public bool SetAttributes(int sysno, IList<int> listAttributeSysNo)
        {
            IList<PdAttributeGroupAssociation> list = new List<PdAttributeGroupAssociation>();
            int order = 0;
            foreach (int attributeSysno in listAttributeSysNo)
            {
                order++;
                PdAttributeGroupAssociation entity = new PdAttributeGroupAssociation
                {
                    AttributeSysNo = attributeSysno,
                    CreatedDate = DateTime.Now,
                    DisplayOrder = order,
                    CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    LastUpdateDate = DateTime.Now
                };
                list.Add(entity);
            }
            return IPdAttributeGroupDao.Instance.SetAttributes(sysno, list);
 
        }

        /// <summary>
        /// 读取商品分类对应的属性组
        /// </summary>
        /// <param name="productCategorySysNo">商品分类编号</param>
        /// <returns>商品分类下所有的属性组列表</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        public IList<PdAttributeGroup> GetPdCategoryAttributeGroupList(int pdCategorySysNo)
        {
            return IPdAttributeGroupDao.Instance.GetPdCategoryAttributeGroupList(pdCategorySysNo);
        }

        /// <summary>
        /// 获取已选的属性组列表
        /// </summary>
        /// <param name="listSysNo">属性组编号列表</param>
        /// <returns>属性组列表</returns>
        /// <remarks>2013-07-12 唐永勤 创建</remarks>
        public IList<PdAttributeGroup> GetSelectedAttributeGroups(IList<int> listSysNo)
        {
            return IPdAttributeGroupDao.Instance.GetSelectedAttributeGroups(listSysNo);
        }

    }
}
