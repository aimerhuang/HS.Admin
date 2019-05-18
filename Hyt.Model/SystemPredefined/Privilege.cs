namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 权限代码 预设值
    /// 数据表：SyPrivilege
    /// </summary>
    /// <remarks>2013-06-18 吴文强 创建</remarks>
    public enum PrivilegeCode
    {
        #region 公共

        /// <summary>
        /// 标记为空权限
        /// </summary>
        None,
        ///// <summary>
        ///// 忽略Action验证(针对公共页面,例如:登录页面)
        ///// </summary>
        IgnoreAction,
        /// <summary>
        /// 公共-商品选择组件
        /// </summary>
        CM1005801,
        /// <summary>
        /// 订单详情组件
        /// </summary>
        CM1005802,
        /// <summary>
        /// 订单选择组件
        /// </summary>
        CM1005803,
        /// <summary>
        /// 仓库多选组件
        /// </summary>
        CM1005804,
        /// <summary>
        /// 选择促销规则组件
        /// </summary>
        CM1005805,
        /// <summary>
        /// 选择促销组件
        /// </summary>
        CM1005806,
        /// <summary>
        /// 促销商品选择组件
        /// </summary>
        CM1005807,
        /// <summary>
        /// 选仓库组件
        /// </summary>
        CM1005808,
        ///// <summary>
        ///// 创建用户组件
        ///// </summary>
        //CM1005809,
        /// <summary>
        /// 品牌选择组件
        /// </summary>
        CM1010002,
        /// <summary>
        /// 商品属性组选择组件
        /// </summary>
        CM1010003,
        /// <summary>
        /// 商品基础属性选择组件
        /// </summary>
        CM1010004,
        /// <summary>
        /// 上传文件组件
        /// </summary>
        CM1010001,
        /// <summary>
        /// 公司公告
        /// </summary>
        CM10046601,
        /// <summary>
        /// 系统公告
        /// </summary>
        CM10046602,

        /// <summary>
        /// 我的菜单
        /// </summary>
        CM1005155,
        /// <summary>
        /// 获取运费模板
        /// </summary>
        CM1006001,
        /// <summary>
        /// 获取仓库物流运费模板关联详情
        /// </summary>
        CM1007001,
        /// <summary>
        /// 更新仓库配送方式关联运费模板
        /// </summary>
        CM1007002,
        #region AjaxController
        /// <summary>
        /// 获取配送方式
        /// </summary>
        CM1005901,
        /// <summary>
        /// 支付方式
        /// </summary>
        CM1005902,
        /// <summary>
        /// 根据配送方式获取支付方式
        /// </summary>
        CM1005903,
        /// <summary>
        /// 根据收货地址编号查询收货地址列表
        /// </summary>
        CM1005904,
        /// <summary>
        /// 根据会员ID查询收货地址列表
        /// </summary>
        CM1005905,
        /// <summary>
        /// 根据区县编号获取省市区全称
        /// </summary>
        CM1005906,
        /// <summary>
        /// 根据会员ID查询会员默认收货地址
        /// </summary>
        CM1005907,
        /// <summary>
        /// 根据SysNo查询收货地址
        /// </summary>
        CM1005908,
        /// <summary>
        /// 查询会员
        /// </summary>
        CM1005909,
        /// <summary>
        /// 查询会员(模糊)
        /// </summary>
        CM1005910,
        /// <summary>
        /// 检查帐号名是否存在
        /// </summary>
        CM1005911,
        /// <summary>
        /// 省市区联动
        /// </summary>
        CM1005912,
        /// <summary>
        /// 该区域支持的配送方式信息
        /// </summary>
        CM1005913,
        /// <summary>
        /// 地区仓库/门店
        /// </summary>
        CM1005914,
        /// <summary>
        /// 获取商品分类
        /// </summary>
        CM1005915,
        /// <summary>
        /// 获取商品价格
        /// </summary>
        CM1005916,
        /// <summary>
        /// 获取价格用户等级（用户包括前台会员，分销商，配送员等非系统管理员）
        /// </summary>
        CM1005917,
        /// <summary>
        /// 获取订单来源列表
        /// </summary>
        CM1005918,
        /// <summary>
        /// 获取订单状态列表
        /// </summary>
        CM1005919,
        /// <summary>
        /// 获取取件方式
        /// </summary>
        CM1005920,
        /// <summary>
        /// 获取RMA状态列表
        /// </summary>
        CM1005921,
        /// <summary>
        /// 获取RMA类型列表
        /// </summary>
        CM1005922,
        /// <summary>
        /// 获取退款方式
        /// </summary>
        CM1005923,
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        CM1005924,

        #endregion

        #endregion

        #region 系统
        /// <summary>
        /// 获取菜单树
        /// </summary>
        SY1006101,

        /// <summary>
        /// 新增/编辑菜单
        /// </summary>
        SY1006201,

        /// <summary>
        /// 全文索引查询
        /// </summary>
        SY1001101,

        /// <summary>
        /// 批量生成索引
        /// </summary>
        SY1001301,

        /// <summary>
        /// 删除索引
        /// </summary>
        SY1001401,

        /// <summary>
        /// 内存键值对查询
        /// </summary>
        SY1002101,

        /// <summary>
        /// 内存键值对删除
        /// </summary>
        SY1002401,

        /// <summary>
        /// 内存数据库查询
        /// </summary>
        SY1003101,

        /// <summary>
        /// 禁用/启用菜单
        /// </summary>
        SY1006701,

        /// <summary>
        /// 删除菜单
        /// </summary>
        SY1006401,

        /// <summary>
        /// 查看角色
        /// </summary>
        SY1004101,

        /// <summary>
        /// 新增/编辑角色
        /// </summary>
        SY1004201,

        /// <summary>
        /// 禁用/启用角色
        /// </summary>
        SY1004701,

        /// <summary>
        /// 修改角色
        /// </summary>
        SY1004301,

        /// <summary>
        /// 删除角色
        /// </summary>
        SY1004401,

        /// <summary>
        /// 查看权限
        /// </summary>
        SY1005101,
        /// <summary>
        /// 新增/编辑权限
        /// </summary>
        SY1005201,
        /// <summary>
        /// 禁用/启用权限
        /// </summary>
        SY1005701,
        /// <summary>
        /// 修改权限
        /// </summary>
        SY1005301,
        /// <summary>
        /// 删除权限
        /// </summary>
        SY1005401,
        /// <summary>
        /// 查看用户组
        /// </summary>
        SY1007101,
        /// <summary>
        /// 新增/编辑用户组
        /// </summary>
        SY1007201,
        /// <summary>
        /// 禁用/启用用户组
        /// </summary>
        SY1007701,
        /// <summary>
        /// 删除用户组
        /// </summary>
        SY1007401,
        /// <summary>
        /// 查看用户
        /// </summary>
        SY1008101,
        /// <summary>
        /// 新增/编辑用户
        /// </summary>
        SY1008201,
        /// <summary>
        /// 禁用/启用用户
        /// </summary>
        SY1008701,

        /// <summary>
        /// 系统日志查询
        /// </summary>
        SY1009101,

        /// <summary>
        /// 新增/编辑任务计划
        /// </summary>
        SY1011201,

        /// <summary>
        /// 禁用/启用任务计划
        /// </summary>
        SY1011701,

        /// <summary>
        /// 系统配置查看
        /// </summary>
        SY1012101,

        /// <summary>
        /// 系统配置新增
        /// </summary>
        SY1012201,

        /// <summary>
        /// 系统配置编辑
        /// </summary>
        SY1012202,

        /// <summary>
        /// 升舱订单自动处理配置
        /// </summary>
        SY1012203,
        #endregion

        #region 基础管理

        #region 组织机构管理
        /// <summary>
        /// 组织机构查看
        /// </summary>
        BS1004101,
        /// <summary>
        /// 组织机构添加
        /// </summary>
        BS1004201,
        /// <summary>
        /// 组织机构添加关联仓库
        /// </summary>
        BS1004202,
        /// <summary>
        /// 组织机构修改
        /// </summary>
        BS1004301,
        /// <summary>
        /// 组织机构删除
        /// </summary>
        BS1004401,
        /// <summary>
        /// 组织机构删除关联仓库
        /// </summary>
        BS1004402,
        #endregion

        #region 配送方式
        /// <summary>
        /// 查看配送方式
        /// </summary>
        LG1001101,

        /// <summary>
        /// 添加配送方式
        /// </summary>
        LG1001201,

        /// <summary>
        /// 修改配送方式
        /// </summary>
        LG1001301,

        /// <summary>
        /// 删除配送方式
        /// </summary>
        LG1001401,

        /// <summary>
        /// 打印模板配置
        /// </summary>
        LG1001901,

        /// <summary>
        /// 修改打印模板
        /// </summary>
        LG1001902,

        #endregion

        #region 配送员信用
        /// <summary>
        /// 配送员信用
        /// </summary>
        LG1007,
        /// <summary>
        /// 查看配送员信用
        /// </summary>
        LG1007101,
        /// <summary>
        /// 添加配送员信用
        /// </summary>
        LG1007201,
        /// <summary>
        /// 修改配送员信用
        /// </summary>
        LG1007301,
        /// <summary>
        /// 删除配送员信用
        /// </summary>
        LG1007401,

        #endregion

        #region 支付方式

        /// <summary>
        /// 支付方式查看
        /// </summary>
        BS1003101,

        /// <summary>
        /// 添加支付方式
        /// </summary>
        BS1003201,

        /// <summary>
        /// 支付方式修改
        /// </summary>
        BS1003301,

        /// <summary>
        /// 支付方式启用/禁用
        /// </summary>
        BS1003701,

        #endregion

        #region 地区信息

        /// <summary>
        /// 地区信息管理
        /// </summary>
        BS1001,

        /// <summary>
        /// 查看地区信息
        /// </summary>
        BS1001101,

        /// <summary>
        /// 编辑地区信息
        /// </summary>
        BS1001201,

        #endregion

        #region 配送支付

        /// <summary>
        /// 配送支付管理
        /// </summary>
        BS1002,

        /// <summary>
        /// 查看配送支付
        /// </summary>
        BS1002101,

        /// <summary>
        /// 设置配送支付
        /// </summary>
        BS1002201,

        /// <summary>
        /// 删除配送支付
        /// </summary>
        BS1002401,

        #endregion

        #region 码表管理
        /// <summary>
        /// 码表列表查看
        /// </summary>
        BS1005001,

        /// <summary>
        /// 码表信息修改
        /// </summary>
        BS1005002,

        /// <summary>
        /// 码表启用/禁用
        /// </summary>
        BS1005003,

        #endregion

        #region 百城当日达范围

        /// <summary>
        /// 百城当日达范围
        /// </summary>
        LG1002,

        /// <summary>
        /// 查看百城当日达范围
        /// </summary>
        LG1002101,

        /// <summary>
        /// 添加百城当日达范围
        /// </summary>
        LG1002201,

        /// <summary>
        /// 删除百城当日达范围
        /// </summary>
        LG1002401,

        /// <summary>
        /// 仓库当日达区域管理
        /// </summary>
        LG2001,

        #endregion

        #region 国家
        /// <summary>
        /// 国家
        /// </summary>
        OR1001,
        #endregion

        /// <summary>
        /// 常用快递维护
        /// </summary>
        WH1005,

        #region 仓库配送取件

        /// <summary>
        /// 仓库配送取件
        /// </summary>
        WH1007,

        /// <summary>
        /// 仓库配送取件方式高级权限（有权限可以维护所有配送方式和取件方式，无权限默认只能维护第三方快递。）
        /// </summary>
        WH1007801,

        /// <summary>
        /// 查看仓库配送取件
        /// </summary>
        WH1007101,

        /// <summary>
        /// 设置仓库配送取件
        /// </summary>
        WH1007201,

        #endregion

        /// <summary>
        /// 搜索配送路径
        /// </summary>
        LG1008101,

        /// <summary>
        /// 搜索配送员定位
        /// </summary>
        LG1009101,
        /// <summary>
        /// 地区仓库管理_查看
        /// </summary>
        WH1001101,

        /// <summary>
        /// 仓库信息管理查看
        /// </summary>
        WH1002101,
        /// <summary>
        /// 仓库信息管理新增
        /// </summary>
        WH1002201,

        /// <summary>
        /// 仓库信息管理修改
        /// </summary>
        WH1002301,

        /// <summary>
        /// 仓库管理商品入库
        /// </summary>
        WH1002302,

        /// <summary>
        /// 仓库管理商品库存
        /// </summary>
        WH1002303,


        /// <summary>
        /// 查看仓库列表
        /// </summary>
        WH1002313,
        /// <summary>
        /// 修改仓库数量
        /// </summary>
        WH1002314,

        /// <summary>
        /// 仓库库位管理
        /// </summary>
        WH1002304,

        /// <summary>
        /// 地区仓库管理修改
        /// </summary>
        WH1001301,

        /// <summary>
        /// EAS同步日志查看
        /// </summary>
        EAS1001101,

        /// <summary>
        /// EAS重新同步
        /// </summary>
        EAS1001108,

        /// <summary>
        /// EAS作废
        /// </summary>
        EAS1001109,

        /// <summary>
        /// EAS出库可传保存状态
        /// </summary>
        EAS1001110,

        #endregion

        #region 前台管理

        #region 文章管理
        /// <summary>
        /// 新增/修改文章分类
        /// </summary>
        FE1004201,

        /// <summary>
        /// 文章分类_通过/取消审核分类
        /// </summary>
        FE1004601,

        /// <summary>
        /// 作废文章分类
        /// </summary>
        FE1004501,

        /// <summary>
        /// 查看文章列表
        /// </summary>
        FE1005001,
        /// <summary>
        /// 查看公司公告
        /// </summary>
        FE1005102,
        /// <summary>
        /// 新增/修改文章
        /// </summary>
        FE1005201,

        /// <summary>
        /// 文章管理_通过/取消审核分类文章
        /// </summary>
        FE1005601,

        /// <summary>
        /// 作废文章
        /// </summary>
        FE1005501,

        /// <summary>
        /// 查看帮助文章分类列表
        /// </summary>
        FE1006001,

        /// <summary>
        /// 新增/修改帮助文章
        /// </summary>
        FE1006201,

        /// <summary>
        /// 帮助管理_通过/取消审核帮助中心文章
        /// </summary>
        FE1006601,

        /// <summary>
        /// 作废帮助文章
        /// </summary>
        FE1006501,

        /// <summary>
        /// 查看文章分类列表
        /// </summary>
        FE1004001,

        /// <summary>
        /// 查看帮助分类列表
        /// </summary>
        FE1007001,

        /// <summary>
        /// 新增/修改帮助文章分类
        /// </summary>
        FE1007201,

        /// <summary>
        /// 帮助分类_通过/取消审核帮助文章分类
        /// </summary>
        FE1007601,

        /// <summary>
        /// 作废帮助文章分类
        /// </summary>
        FE1007501,
        #endregion

        #region 搜索关键字
        /// <summary>
        /// 搜索关键字管理_前台显示/取消显示关键字
        /// </summary>
        FE0007,

        /// <summary>
        /// 查看关键字列表
        /// </summary>
        FE1003001,

        /// <summary>
        /// 添加/修改搜索关键字
        /// </summary>
        FE1003201,

        /// <summary>
        /// 删除搜索关键字
        /// </summary>
        FE1003401,

        /// <summary>
        /// 设置前台显示/隐藏
        /// </summary>
        FE1003601,
        #endregion

        #region 评价/晒单
        /// <summary>
        /// 查看评论列表
        /// </summary>
        FE1001001,

        /// <summary>
        /// 查看评论详情
        /// </summary>
        FE1001002,

        /// <summary>
        /// 查看全部评论回复
        /// </summary>
        FE1001003,

        /// <summary>
        /// 设为精华/商品评论
        /// </summary>
        FE1001601,

        /// <summary>
        /// 设为置顶/商品评论
        /// </summary>
        FE1001602,

        /// <summary>
        /// (审核/取消审核)评论/评论回复
        /// </summary>
        FE1001603,

        /// <summary>
        /// 作废评论/评论回复
        /// </summary>
        FE1001401,

        /// <summary>
        /// 设为精华/商品晒单
        /// </summary>
        FE1002601,

        /// <summary>
        /// 查看晒单列表
        /// </summary>
        FE1002001,

        /// <summary>
        /// 查看晒单详情
        /// </summary>
        FE1002002,

        /// <summary>
        /// 查看全部晒单回复
        /// </summary>
        FE1002003,

        /// <summary>
        /// 设为置顶/商品晒单
        /// </summary>
        FE1002602,

        /// <summary>
        /// (审核/取消审核)晒单/晒单图片/晒单回复
        /// </summary>
        FE1002603,

        /// <summary>
        /// 作废晒单/晒单图片/晒单回复
        /// </summary>
        FE1002501,
        #endregion

        #region 广告展示
        /// <summary>
        /// 查看广告组列表
        /// </summary>
        FE1008001,
        /// <summary>
        /// 广告项管理（查看广告项）
        /// </summary>
        FE1008002,
        /// <summary>
        /// 添加/修改广告组
        /// </summary>
        FE1008201,
        /// <summary>
        /// 启用/禁用广告组
        /// </summary>
        FE1008601,
        /// <summary>
        /// 添加/修改广告项
        /// </summary>
        FE1008202,
        /// <summary>
        /// 审核/取消审核广告项
        /// </summary>
        FE1008602,
        /// <summary>
        /// 作废广告项
        /// </summary>
        FE1008501,
        /// <summary>
        /// 同步广告项
        /// </summary>
        FE1008701,
        #endregion

        #region 商品展示
        /// <summary>
        /// 查看商品项列表
        /// </summary>
        FE1009001,
        /// <summary>
        /// 商品项管理（查看商品项）
        /// </summary>
        FE1009002,
        /// <summary>
        /// 添加/修改商品组
        /// </summary>
        FE1009201,
        /// <summary>
        /// 启用/禁用商品组
        /// </summary>
        FE1009601,
        /// <summary>
        /// 添加/修改商品项
        /// </summary>
        FE1009202,
        /// <summary>
        /// 审核/取消审核商品项
        /// </summary>
        FE1009602,
        /// <summary>
        /// 作废商品项
        /// </summary>
        FE1009501,
        /// <summary>
        /// 同步商品项
        /// </summary>
        FE1009701,
        #endregion

        #region 客户投诉
        /// <summary>
        /// 查看客户投诉列表
        /// </summary>
        CR1001001,

        /// <summary>
        /// 查看客户投诉详情
        /// </summary>
        CR1001002,

        /// <summary>
        /// 回复客户投诉
        /// </summary>
        CR1001301,

        /// <summary>
        /// 关闭/作废客户投诉
        /// </summary>
        CR1001501,
        #endregion

        #region 商品咨询
        /// <summary>
        /// 查看咨询列表
        /// </summary>
        CR1002001,

        /// <summary>
        /// 查看咨询详情
        /// </summary>
        CR1002002,

        /// <summary>
        /// 回复咨询
        /// </summary>
        CR1002601,

        /// <summary>
        /// 作废咨询
        /// </summary>
        CR1002501,
        #endregion

        #region app内容管理
        /// <summary>
        /// 查看app版本
        /// </summary>
        AP1001101,
        /// <summary>
        /// 添加/修改APP版本
        /// </summary>
        AP1001201,
        /// <summary>
        /// 删除APP版本
        /// </summary>
        AP1001401,

        /// <summary>
        /// 查看APP推送信息
        /// </summary>
        AP1002101,

        /// <summary>
        /// 添加/修改APP推送信息
        /// </summary>
        AP1002102,

        /// <summary>
        /// 审核推送/作废APP推送信息
        /// </summary>
        AP1002103,


        #endregion

        #region 商品浏览历史记录
        /// <summary>
        /// 查看商品浏览历史记录
        /// </summary>
        CR1003101,
        /// <summary>
        /// 删除商品浏览历史记录
        /// </summary>
        CR1003401,

        #endregion

        #region 友情链接
        /// <summary>
        /// 友情链接列表
        /// </summary>
        FE1010001,

        /// <summary>
        /// 友情链接添加/修改
        /// </summary>
        FE1010002,

        /// <summary>
        /// 友情链接审核
        /// </summary>
        FE1010003,
        #endregion

        #region 新闻管理
        /// <summary>
        /// 查看新闻分类列表
        /// </summary>
        FE1013101,

        /// <summary>
        /// 添加/修改新闻分类
        /// </summary>
        FE1013102,

        /// <summary>
        /// 审核/取消审核新闻分类
        /// </summary>
        FE1013103,

        /// <summary>
        /// 作废新闻分类
        /// </summary>
        FE1013104,
        /// <summary>
        /// 查看新闻列表
        /// </summary>
        FE1013001,

        /// <summary>
        /// 添加/修改新闻
        /// </summary>
        FE1013002,

        /// <summary>
        /// 审核/取消审核新闻
        /// </summary>
        FE1013003,

        /// <summary>
        /// 作废新闻
        /// </summary>
        FE1013004,

        /// <summary>
        /// 查看新闻新闻联商品列表
        /// </summary>
        FE1013005,

        /// <summary>
        /// 添加/修改新闻新闻联商品
        /// </summary>
        FE1013006,

        /// <summary>
        /// 删除新闻新闻联商品
        /// </summary>
        FE1013007,

        #endregion

        #region 软件下载

        /// <summary>
        /// 查看软件分类列表
        /// </summary>
        FE1011001,

        /// <summary>
        /// 新增/修改软件分类
        /// </summary>
        FE1011201,

        /// <summary>
        /// 启用/禁用软件分类
        /// </summary>
        FE1011601,

        /// <summary>
        /// 查看软件下载列表
        /// </summary>
        FE1012001,

        /// <summary>
        /// 新增/修改软件下载
        /// </summary>
        FE1012201,

        /// <summary>
        /// 作废软件下载
        /// </summary>
        FE1012301,

        /// <summary>
        /// 审核通过/取消审核软件下载
        /// </summary>
        FE1012601,

        #endregion

        #endregion

        #region 商品管理

        /// <summary>
        /// 属性组管理_查看产品属性组
        /// </summary>
        PD1004001,

        /// <summary>
        /// 属性组管理_新增/修改产品属性组
        /// </summary>
        PD1004201,

        /// <summary>
        /// 属性组管理_启用/禁用属性组
        /// </summary>
        PD1004601,

        /// <summary>
        /// 品牌管理_查看品牌列表
        /// </summary>
        PD1005001,

        /// <summary>
        /// 品牌管理_新增/修改品牌
        /// </summary>
        PD1005201,

        /// <summary>
        /// 品牌管理_启用/禁用品牌
        /// </summary>
        PD1005601,

        /// <summary>
        /// 查看商品分类列表
        /// </summary>
        PD1007001,

        /// <summary>
        /// 添加/修改(内容/排序)商品分类
        /// </summary>
        PD1007201,

        /// <summary>
        /// 商品分类管理_启用/禁用商品分类
        /// </summary>
        PD1007601,

        /// <summary>
        /// 产品列表_查看商品列表（包含商品详情/价格列表）
        /// </summary>
        PD1001001,

        /// <summary>
        /// 产品列表_添加(克隆)/修改产品
        /// </summary>
        PD1001201,

        /// <summary>
        /// 产品列表_上架/下架商品
        /// </summary>
        PD1001601,

        /// <summary>
        /// 产品列表_作废/启用商品
        /// </summary>
        PD1001701,

        /// <summary>
        /// 产品列表_产品调价申请
        /// </summary>
        PD1001202,

        /// <summary>
        /// 产品列表_商品导入Excel
        /// </summary>
        PD1001203,

        /// <summary>
        /// 产品属性管理_查看属性列表
        /// </summary>
        PD1002001,

        /// <summary>
        /// 产品属性管理_新增/修改产品属性
        /// </summary>
        PD1002201,

        /// <summary>
        /// 产品属性管理_启用/禁用商品属性
        /// </summary>
        PD1002601,

        /// <summary>
        ///  查看调价申请列表
        /// </summary>
        PD1003001,

        /// <summary>
        /// 商品调价申请管理_审核调价申请
        /// </summary>
        PD1003601,

        /// <summary>
        /// 查看模版/模块列表
        /// </summary>
        PD1006001,

        /// <summary>
        /// 新增/修改商品(模板/模块)
        /// </summary>
        PD1006201,

        /// <summary>
        /// 删除(模版/模块)
        /// </summary>
        PD1006401,

        /// <summary>
        /// 选择模版组件
        /// </summary>
        CM1010005,

        /// <summary>
        /// 查看模版/模块列表
        /// </summary>
        PD1006503,

        /// <summary>
        /// 添加/查看商品条码
        /// </summary>
        PD1006504,

        /// <summary>
        /// 供应链商品列表
        /// </summary>
        PD1006202,
        #endregion

        #region 订单管理

        /// <summary>
        /// 查看订单
        /// </summary>
        SO1001101,

        /// <summary>
        ///  新增/编辑订单
        /// </summary>
        SO1002201,

        /// <summary>
        /// 查看我的订单
        /// </summary>
        SO1003101,

        /// <summary>
        /// eg：订单审核
        /// </summary>
        SO1003601,

        /// <summary>
        /// 作废订单
        /// </summary>
        SO1003501,

        /// <summary>
        /// 订单调价
        /// </summary>
        SO1003785,

        #region 订单池
        /// <summary>
        /// 查看任务池
        /// </summary>
        SY1010101,
        /// <summary>
        /// 编辑任务池
        /// </summary>
        SY1010201,

        /// <summary>
        /// 工作池管理_工作分配
        /// 可以分配/收回/锁定工作池中的工作.
        /// </summary>
        SY1010801,

        /// <summary>
        /// 当日达未处理订单
        /// </summary>
        SY1010901,
        #endregion

        #region 任务池优先级
        /// <summary>
        /// 查看任务池优先级
        /// </summary>
        SP1010101,
        /// <summary>
        /// 编辑任务池优先级
        /// </summary>
        SP1010201,

        /// <summary>
        /// 删除任务池优先级
        /// </summary>
        SP1010401,

        #endregion

        #region 我的工作
        /// <summary>
        /// 查看我的工作
        /// </summary>
        SY1018101,
        /// <summary>
        /// 编辑我的工作
        /// </summary>
        SY1018201,

        /// <summary>
        /// 可以申领/解除我的工作.
        /// </summary>
        SY1018801,

        #endregion

        #endregion

        #region 推送订单返回管理
        SOR1001,
        #endregion
        #region 一号仓包裹追踪
        SOR1002,
        #endregion
        #region 仓库管理

        /// <summary>
        /// 借货单管理_强制完结
        /// </summary>
        WH0011,

        /// <summary>
        /// 借货单管理_超额强制放行
        /// </summary>
        WH0012,

        /// <summary>
        /// 发票维护查看
        /// </summary>
        FN1001101,
        /// <summary>
        /// 发票维护开票
        /// </summary>
        FN1001301,
        /// <summary>
        /// 借货单查看
        /// </summary>
        WH1004101,
        /// <summary>
        /// 创建借货单
        /// </summary>
        WH1004201,
        /// <summary>
        /// 商品还货
        /// </summary>
        WH1004202,
        /// <summary>
        /// 借货单导出Excel
        /// </summary>
        WH1004102,
        /// <summary>
        /// 借货单打印
        /// </summary>
        WH1004103,

        /// <summary>
        /// 出库单管理_查看
        /// </summary>
        WH1003101,
        /// <summary>
        /// 出库单管理出库
        /// </summary>
        WH1003301,
        /// <summary>
        /// 出库单管理作废
        /// </summary>
        WH1003401,
        /// <summary>
        /// 出库单管理打印
        /// </summary>
        WH1003601,
        /// <summary>
        /// 业务员库存查询
        /// </summary>
        WH1005101,
        /// <summary>
        /// 入库单查看
        /// </summary>
        WH1006101,
        /// <summary>
        /// 入库单入库
        /// </summary>
        WH1006301,
        /// <summary>
        /// 入库单作废
        /// </summary>
        WH1006501,
        /// <summary>
        /// 入库单打印
        /// </summary>
        WH1006102,
        /// <summary>
        /// 入库单批量打印
        /// </summary>
        WH1006103,

        /// <summary>
        /// 预收现金收款单管理
        /// </summary>
        WH1008101,
        /// <summary>
        /// 调货单管理
        /// </summary>
        WH1009101,
        /// <summary>
        /// 采购退货出库单查看
        /// </summary>
        WH1010101,
        /// <summary>
        /// 采购退货出库单出库
        /// </summary>
        WH1010102,
        /// <summary>
        /// 采购退货出库单作废
        /// </summary>
        WH1010103,
        #endregion

        #region 物流管理
        /// <summary>
        /// 取件单查看
        /// </summary>
        LG1010101,
        #region 结算单管理
        /// <summary>
        /// 结算单管理打印
        /// </summary>
        LG1003101,
        /// <summary>
        /// 结算单明细作废
        /// </summary>
        LG1003105,
        /// <summary>
        /// 结算单明细查看
        /// </summary>
        LG1003102,
        #endregion
        #region 创建结算单
        /// <summary>
        /// 生成结算单(对应按钮)
        /// </summary>
        LG1005201,
        #endregion
        #region 配送单管理
        /// <summary>
        /// 导出配送单
        /// </summary>
        LG1006101,
        /// <summary>
        /// 新增配送单
        /// </summary>
        LG1006201,

        /// <summary>
        /// 打印
        /// </summary>
        LG1006103,
        /// <summary>
        /// 作废
        /// </summary>
        LG1006501,
        /// <summary>
        /// 查看配送单权限
        /// </summary>
        LG1006104,
        #endregion
        #region 业务员补单

        /// <summary>
        /// 补单查看
        /// </summary>
        LG1004101,
        #endregion

        #region 物流号刷单
        /// <summary>
        /// 物流号查看
        /// </summary>
        LG1005101,
        #endregion

        #region 运费模板
        /// <summary>
        /// 运费模板查看
        /// </summary>
        LG1015101,
        #endregion
        #endregion
        #region 退换货管理

        /// <summary>
        /// 新建退换货单_批准超时退换货
        /// </summary>
        RC1001801,

        /// <summary>
        /// 退换货单管理_退货金额变更
        /// </summary>
        RC1001802,

        /// <summary>
        /// 退换货单管理_退换货单审核
        /// </summary>
        RC1002601,

        /// <summary>
        /// [门店]新建退换货单_批准超时退换货
        /// </summary>
        RC1003801,

        /// <summary>
        /// [门店]退换货单管理_退货金额变更
        /// </summary>
        RC1003802,

        /// <summary>
        /// [门店]退换货单管理_退换货单审核
        /// </summary>
        RC1004601,

        /// <summary>
        /// 查看新建退换货
        /// </summary>
        RC1001101,

        /// <summary>
        /// 新增退换货
        /// </summary>
        RC1001201,

        /// <summary>
        /// 编辑退换货单
        /// </summary>
        RC1002201,

        /// <summary>
        /// 查看退换货单维护
        /// </summary>
        RC1002101,

        /// <summary>
        /// 查看门店新建退换货
        /// </summary>
        RC1003101,

        /// <summary>
        ///新增门店退换货 
        /// </summary>
        RC1003201,

        /// <summary>
        /// 查看门店退换货单维护
        /// </summary>
        RC1004101,
        /// <summary>
        /// 编辑门店退换货单
        /// </summary>
        RC1004201,

        /// <summary>
        /// 门店退换货打印小票
        /// </summary>
        RC1004801,

        /// <summary>
        /// 查看退款维护
        /// </summary>
        RC1005101,
        #endregion

        #region 财务管理
        /// <summary>
        /// 分销商资金交易明细查看
        /// </summary>
        DS1008101,
        /// <summary>
        /// 分销商资金管理_充值
        /// </summary>
        DS1008301,

        /// <summary>
        /// 分销商资金管理_提现
        /// </summary>
        DS1008302,

        /// <summary>
        /// 查看收款单
        /// </summary>
        FN1002101,
        /// <summary>
        /// 编辑收款单
        /// </summary>
        FN1002201,

        /// <summary>
        /// 查看在线支付
        /// </summary>
        FN1003101,

        /// <summary>
        /// 新增在线支付
        /// </summary>
        FN1003201,

        /// <summary>
        /// 查看付款单
        /// </summary>
        FN1004101,

        /// <summary>
        /// 编辑付款单
        /// </summary>
        FN1004201,

        /// <summary>
        /// 加盟商对账
        /// </summary>
        FN1002202,

        #region 收款科目管理 huangwei
        /// <summary>
        /// 查看收款科目
        /// </summary>
        FN1005101,
        /// <summary>
        /// 导入修改收款科目
        /// </summary>
        FN1005301,
        /// <summary>
        /// 新增收款科目
        /// </summary>
        FN1005201,
        /// <summary>
        /// 删除收款科目
        /// </summary>
        FN1005401,

        #endregion

        #endregion

        #region 分销商EAS关联管理
        /// <summary>
        /// 查看分销商EAS关联
        /// </summary>
        DS100901,

        /// <summary>
        /// 添加分销商EAS关联
        /// </summary>
        DS100902,

        /// <summary>
        /// 修改分销商EAS关联
        /// </summary>
        DS100903,

        /// <summary>
        /// 删除分销商EAS关联
        /// </summary>
        DS100904,
        #endregion

        #region 门店管理
        /// <summary>
        /// 查看门店订单
        /// </summary>
        SO1004101,
        /// <summary>
        /// 新建门店订单
        /// </summary>
        SO1004201,
        /// <summary>
        /// 查看门店自提
        /// </summary>
        SO1005101,
        /// <summary>
        /// 确认门店自提
        /// </summary>
        SO1005201,
        /// <summary>
        /// 提货门店自提
        /// </summary>
        SO1005601,

        /// <summary>
        /// 门店自提打印小票
        /// </summary>
        SO1005801,

        /// <summary>
        /// 分销商店铺仓库商品
        /// </summary>
        WH1006801,

        #endregion

        #region 分销管理
        #region 分销商特殊价格管理
        /// <summary>
        /// 分销商特殊价格查看
        /// </summary>
        DS1001101,
        /// <summary>
        /// 分销商特殊价格商品查看
        /// </summary>
        DS1001102,
        /// <summary>
        /// 分销商特殊价格新增
        /// </summary>
        DS1001201,

        /// <summary>
        /// 分销商特殊价格修改
        /// </summary>
        DS1001301,
        #endregion

        #region 分销商等级价格管理
        /// <summary>
        /// 分销商等级价格查看
        /// </summary>
        DS1005101,

        /// <summary>
        /// 分销商等级价格新增
        /// </summary>
        DS1005201,

        /// <summary>
        /// 分销商等级价格修改
        /// </summary>
        DS1005301,
        #endregion
        #region 分销商等级价格审核
        /// <summary>
        /// 分销商等级价格审核查看
        /// </summary>
        DS1007101,
        /// <summary>
        /// 分销商等级价格审核审核
        /// </summary>
        DS1007301,
        #endregion

        #region  分销商

        /// <summary>
        /// 查看分销商
        /// </summary>
        DS1002101,

        /// <summary>
        /// 添加分销商
        /// </summary>
        DS1002201,

        /// <summary>
        /// 修改分销商
        /// </summary>
        DS1002301,

        /// <summary>
        /// 是否批发商按钮可见
        /// </summary>
        DS1002302,

        /// <summary>
        /// 分销商关系图
        /// </summary>
        DS1002401,

        /// <summary>
        /// 启用/禁用分销商
        /// </summary>
        DS1002701,

        /// <summary>
        /// 分销商帐号管理
        /// </summary>
        DS1003001,

        /// <summary>
        /// 分销商返利记录
        /// </summary>
        DS1003002,

        /// <summary>
        /// 分销商会员关系图
        /// </summary>
        DS1003004,

        /// <summary>
        /// 分销商详情统计报表
        /// </summary>
        DS1003005,

        #endregion

        #region  分销商微信菜单
        /// <summary>
        /// 微信菜单查看
        /// </summary>
        DS1017101,
        /// <summary>
        /// 微信菜单新增
        /// </summary>
        DS1017102,
        /// <summary>
        /// 微信菜单修改
        /// </summary>
        DS1017103,
        /// <summary>
        /// 微信菜单删除
        /// </summary>
        DS1017104,
        /// <summary>
        /// 分销商微信子级菜单查看
        /// </summary>
        DS1017105,
        /// <summary>
        /// 分销商微信子级菜单新增
        /// </summary>
        DS1017106,
        /// <summary>
        /// 分销商微信子级菜单修改
        /// </summary>
        DS1017107,
        /// <summary>
        /// 分销商微信子级菜单删除
        /// </summary>
        DS1017108,
        #endregion

        #region  分销商等级

        /// <summary>
        /// 查看分销商等级
        /// </summary>
        DS1003101,

        /// <summary>
        /// 添加分销商等级
        /// </summary>
        DS1003201,

        /// <summary>
        /// 修改分销商等级
        /// </summary>
        DS1003301,

        /// <summary>
        /// 启用/禁用分销商等级
        /// </summary>
        DS1003701,
        #endregion

        #region  分销商商城

        /// <summary>
        /// 查看分销商商城
        /// </summary>
        DS1006101,

        /// <summary>
        /// 添加分销商商城
        /// </summary>
        DS1006201,

        /// <summary>
        /// 修改分销商商城
        /// </summary>
        DS1006301,

        /// <summary>
        /// 启用/禁用分销商商城
        /// </summary>
        DS1006701,
        #endregion

        #region  分销商城类型

        /// <summary>
        /// 查看分销商城类型
        /// </summary>
        DS1004101,

        /// <summary>
        /// 添加分销商城类型
        /// </summary>
        DS1004201,

        /// <summary>
        /// 修改分销商城类型
        /// </summary>
        DS1004301,

        /// <summary>
        /// 启用/禁用分销商城类型
        /// </summary>
        DS1004701,
        #endregion

        #region  分销商App管理

        /// <summary>
        /// 查看分销商App
        /// </summary>
        DS1010101,

        /// <summary>
        /// 添加分销商App
        /// </summary>
        DS1010201,

        /// <summary>
        /// 修改分销商App
        /// </summary>
        DS1010301,

        /// <summary>
        /// 启用/禁用分销商App
        /// </summary>
        DS1010701,
        #endregion

        #region 禁止升舱商品管理
        /// <summary>
        /// 查看禁止升舱商品管理
        /// </summary>
        DS1009101,

        /// <summary>
        /// 添加禁止升舱商品
        /// </summary>
        DS1009201,

        /// <summary>
        /// 删除禁止升舱商品
        /// </summary>
        DS1009401,
        #endregion

        /// <summary>
        /// 分销商升舱错误日志
        /// </summary>
        DS1009501,

        /// <summary>
        /// 升舱关系维护
        /// </summary>
        Ds1009502,

        #region 商城地区关联维护

        /// <summary>
        /// 查看商城地区关联
        /// </summary>
        DS1011101,

        /// <summary>
        /// 删除商城地区关联
        /// </summary>
        DS1011401,

        #endregion

        #region 分销商商品管理

        /// <summary>
        /// 查看分销商商品
        /// </summary>
        DS1011501,

        /// <summary>
        /// 删除分销商商品
        /// </summary>
        DS1011502,
        /// <summary>
        /// 同步总部
        /// </summary>
        DS1011503,
        /// <summary>
        /// 分销商批量下架
        /// </summary>
        DS1011504,
        /// <summary>
        ///分销商产品列表_上架/下架商品
        /// </summary>
        DS1011505,
        /// <summary>
        /// 分销商内存键值删除
        /// </summary>
        DS1011506,
        #endregion

        #region 分销商全文索引
        /// <summary>
        /// 分销商批量生成索引
        /// </summary>
        DS1011507,
        /// <summary>
        /// 分销商索引优化
        /// </summary>
        DS1011508,
        /// <summary>
        /// 分销商更新索引
        /// </summary>
        DS1011509,
        /// <summary>
        /// 分销商删除索引
        /// </summary>
        DS1011510,
        /// <summary>
        /// 分销商索引查询
        /// </summary>
        DS1011511,
        /// <summary>
        /// 分销商索引查看
        /// </summary>
        DS1011512,
        /// <summary>
        /// 分销商查看索引详情
        /// </summary>
        DS1011513,
        #endregion

        #region 分销商订单权限
        /// <summary>
        /// 分销商订单
        /// </summary>
        DS1009801,
        #endregion

        #region 会员分销商申请
        /// <summary>
        /// 查看分销商会员申请表
        /// </summary>
        CR1008801,
        /// <summary>
        /// 会员申请分销商审核权限
        /// </summary>
        CR1008802,
        #endregion

        #endregion

        #region 客户管理

        #region 客户管理
        /// <summary>
        /// 查看会员列表
        /// </summary>
        CR1004001,

        /// <summary>
        /// 查看等级变更日志
        /// </summary>
        CR1004002,

        /// <summary>
        /// 查看等级积分日志
        /// </summary>
        CR1004003,

        /// <summary>
        /// 查看经验积分日志
        /// </summary>
        CR1004004,

        /// <summary>
        /// 查看惠源币日志
        /// </summary>
        CR1004005,

        /// <summary>
        /// 添加/修改新会员
        /// </summary>
        CR1004201,
        /// <summary>
        /// 修改会员分销等级
        /// </summary>
        CR1004211,

        /// <summary>
        /// 重置密码
        /// </summary>
        CR1004202,

        /// <summary>
        /// 设置帐号有效/无效
        /// </summary>
        CR1004601,

        /// <summary>
        /// 查看会员详细信息
        /// </summary>
        CR1004101,

        /// <summary>
        /// 调整等级积分
        /// </summary>
        CR1004203,

        /// <summary>
        /// 调整经验积分
        /// </summary>
        CR1004204,

        /// <summary>
        /// 调整汇源币
        /// </summary>
        CR1004205,

        /// <summary>
        /// 查看历史订单
        /// </summary>
        CR1004102,

        /// <summary>
        /// 查看大宗采购列表
        /// </summary>
        CR1004081,

        /// <summary>
        /// 处理大宗采购信息
        /// </summary>
        CR1004082,

        /// <summary>
        /// 短信查询
        /// </summary>
        CR1004083,

        /// <summary>
        /// 充值记录
        /// </summary>
        CR1004084,

        #region 短信咨询管理

        /// <summary>
        /// 短信咨询查看/管理
        /// </summary>
        CR1004091,

        /// <summary>
        /// 短信咨询作废
        /// </summary>
        CR1004092,

        /// <summary>
        /// 短信咨询回复
        /// </summary>
        CR1004093,

        /// <summary>
        /// 短信发送
        /// </summary>
        CR1004094,
        #endregion

        #region 到货通知
        /// <summary>
        /// 到货通知
        /// </summary>
        CR1004095,
        #endregion

        #region 订单微信通知
           CR1005084,
        #endregion

        #endregion

        #region 客户等级
        /// <summary>
        /// 查看客户等级列表
        /// </summary>
        CR1005001,

        /// <summary>
        /// 添加/修改客户等级
        /// </summary>
        CR1005201,
        #endregion

        #endregion

        #region 报表管理
        /// <summary>
        /// 查看市场部赠送明细
        /// </summary>
        RT1001101,
        /// <summary>
        /// 查看升舱明细
        /// </summary>
        RT1002101,
        /// <summary>
        /// 查看销售明细
        /// </summary>
        RT1003101,
        /// <summary>
        /// 销售排行统计
        /// </summary>
        RT1003102,

        /// <summary>
        /// 查看运营综述日报
        /// </summary>
        RT1004101,

        /// <summary>
        /// 查看运营综述月报
        /// </summary>
        RT1004102,

        /// <summary>
        /// 查看退换货明细
        /// </summary>
        RT1005101,
        /// <summary>
        /// 仓库内勤绩效报表
        /// </summary>
        RT1006101,
        /// <summary>
        /// 电商中心查询
        /// </summary>
        RP1001101,

        /// <summary>
        /// 查看门店会员消费
        /// </summary>
        RT1009101,

        /// <summary>
        /// 门店会员消费导出
        /// </summary>
        RT1009801,

        /// <summary>
        /// 电商中心查询导出
        /// </summary>
        RP1001801,
        /// <summary>
        /// 客服绩效查询
        /// </summary>
        RP1002101,
        /// <summary>
        /// 客服绩效查询导出
        /// </summary>
        RP1002801,
        /// <summary>
        /// 门店新增会员查询
        /// </summary>
        RP1003101,
        /// <summary>
        /// 门店新增会员查询导出
        /// </summary>
        RP1003801,
        /// <summary>
        /// 门店新增会员明细查询
        /// </summary>
        RP1005101,
        /// <summary>
        /// 门店新增会员明细导出
        /// </summary>
        RP1005801,
        /// <summary>
        /// 仓库内勤绩效导出
        /// </summary>
        RP1004801,
        /// <summary>
        /// 业务员绩效查看导出
        /// </summary>
        RT1007101,
        /// <summary>
        /// 办事处绩效查看导出
        /// </summary>
        RT1008101,

        /// <summary>
        /// 网站流量分析报表查看
        /// </summary>
        RT101101,

        /// <summary>
        /// 配送单报表导出
        /// </summary>
        RT101109,

        /// <summary>
        /// 优惠卡报表统计
        /// </summary>
        RT101201,

        /// <summary>
        /// 仓库商品销售排行统计报表
        /// </summary>
        RT101202,

        /// <summary>
        /// 销量统计报表
        /// </summary>
        RT101203,

        /// <summary>
        /// 升舱销量统计报表
        /// </summary>
        RT101204,

        /// <summary>
        /// 快递100报表
        /// </summary>
        RT101205,

        /// <summary>
        /// 区域销售统计报表
        /// </summary>
        RT101206,

        /// <summary>
        /// 加盟商当日达对账报表
        /// </summary>
        RT101207,

        /// <summary>
        /// 加盟商报表
        /// </summary>
        RT101208,
        /// <summary>
        /// 加盟商退换货对账报表
        /// </summary>
        RT101209,
        /// <summary>
        /// 二次销售订单业务员对账报表
        /// </summary>
        RT101301,

        /// <summary>
        /// 办事处快递发货量统计报表
        /// </summary>
        RT101210,

        /// <summary>
        /// 会员涨势统计报表
        /// </summary>
        RT101410,

        /// <summary>
        /// 返利记录统计报表
        /// </summary>
        RT105410,

        /// <summary>
        /// 分销商返利记录统计报表
        /// </summary>
        RT106410,

        /// <summary>
        /// 经销商总销售量排名
        /// </summary>
        RT104410,
        /// <summary>
        /// 经销商商品销售排行
        /// </summary>
        RT104411,
        RT101401,
        /// <summary>
        /// 实体店销售统计
        /// </summary>
        RT101501,
        /// <summary>
        /// 客户购买量统计
        /// </summary>
        RT101601,
        /// <summary>
        /// 分销商购买量统计
        /// </summary>
        RT101701,
        /// <summary>
        /// 网上购买量统计
        /// </summary>
        RT101801,
        /// <summary>
        /// 保税购买量统计
        /// </summary>
        RT101901,
        /// <summary>
        /// 会员注册情况统计表
        /// </summary>
        RT103001,
        #endregion

        #region 坐席系统
        /// <summary>
        /// 查看呼叫中心
        /// </summary>
        CC1001101,

        #endregion

        #region 促销管理

        /// <summary>
        /// 团购管理_审核/作废
        /// </summary>
        SP1002601,

        /// <summary>
        /// 优惠券管理_审核/作废
        /// </summary>
        SP1004601,

        /// <summary>
        /// 促销管理_审核/作废
        /// </summary>
        SP1001601,

        /// <summary>
        /// 审核促销规则
        /// </summary>
        SP1003601,

        /// <summary>
        /// 查看促销管理
        /// </summary>
        SP1001101,
        /// <summary>
        /// 新增/编辑促销
        /// </summary>
        SP1001201,
        /// <summary>
        /// 新增/编辑秒杀促销
        /// </summary>
        SP1001401,
        /// <summary>
        /// 查看团购管理
        /// </summary>
        SP1002101,
        /// <summary>
        /// 新增/编辑团购
        /// </summary>
        SP1002201,
        /// <summary>
        /// 查看促销规则
        /// </summary>
        SP1003101,
        /// <summary>
        /// 新增/编辑促销规则
        /// </summary>
        SP1003201,
        /// <summary>
        /// 查看优惠券管理
        /// </summary>
        SP1004101,
        /// <summary>
        /// 新增/编辑优惠券
        /// </summary>
        SP1004201,

        /// <summary>
        /// 查看优惠券卡号管理
        /// </summary>
        SP1006101,

        /// <summary>
        /// 修改优惠券卡号
        /// </summary>
        SP1006201,

        /// <summary>
        /// 导入优惠券卡号
        /// </summary>
        SP1006301,

        /// <summary>
        /// 绑定优惠卡
        /// </summary>
        SP1006401,

        /// <summary>
        /// 退回优惠卡
        /// </summary>
        SP1006501,

        /// <summary>
        /// 新增/编辑组合套餐
        /// </summary>
        SP1006601,
        /// <summary>
        /// 查看组合套餐
        /// </summary>
        SP1006602,
        /// <summary>
        /// 审核组合套餐
        /// </summary>
        SP1006603,
        #region 客户优惠卷绑定

        /// <summary>
        /// 绑定用户优惠券
        /// </summary>
        SP1005101,

        /// <summary>
        /// 审核/作废用户优惠券
        /// </summary>
        SP1005201,

        /// <summary>
        /// 查看用户已绑定优惠券
        /// </summary>
        SP1005301,

        /// <summary>
        /// 领取PC商城优惠券(所有、WEB、WEB_门店、WEB_APP)
        /// </summary>
        SP1005302,
        /// <summary>
        /// 领取门店优惠券(所有、门店、WEB_门店、门店_APP)
        /// </summary>
        SP1005303,
        /// <summary>
        /// 领取手机商城优惠券(所有、手机商城、WEB_APP、门店_APP)
        /// </summary>
        SP1005304,
        /// <summary>
        /// 领取物流App优惠券
        /// </summary>
        SP1005305,
        #endregion

        /// <summary>
        /// 查看优惠卡类型列表
        /// </summary>
        SP1006901,
        /// <summary>
        /// 新增/编辑优惠卡类型
        /// </summary>
        SP1006902,

        /// <summary>
        /// 查看仓库免运费
        /// </summary>
        WFF1001101,
        /// <summary>
        /// 新增/编辑仓库免运费
        /// </summary>
        WFF1001201,
        #endregion

        #region 营销管理
        /// <summary>
        /// 查看联盟网站
        /// </summary>
        UN1001101,
        /// <summary>
        /// 添加联盟网站
        /// </summary>
        UN1001201,
        /// <summary>
        /// 修改联盟网站
        /// </summary>
        UN1001301,
        /// <summary>
        /// 启用/禁用联盟网站
        /// </summary>
        UN1001701,

        /// <summary>
        /// 查看联盟广告
        /// </summary>
        UN1002101,
        /// <summary>
        /// 添加联盟广告
        /// </summary>
        UN1002201,
        /// <summary>
        /// 修改联盟广告
        /// </summary>
        UN1002301,
        /// <summary>
        /// 启用/禁用联盟广告
        /// </summary>
        UN1002701,

        /// <summary>
        /// 查看广告日志
        /// </summary>
        UN1003101,
        #region wechat huangwei
        /// <summary>
        /// 微信自动回复配置(仅只有新增,进入就新增)
        /// </summary>
        UN1004101,
        /// <summary>
        /// 微信关键词列表查看
        /// </summary>
        UN1005101,
        /// <summary>
        /// 微信关键词-新增
        /// </summary>
        //UN1005201,
        /// <summary>
        /// 微信关键词-修改
        /// </summary>
        UN1005301,
        /// <summary>
        /// 微信关键词-删除
        /// </summary>
        UN1005401,
        /// <summary>
        /// 微信关键词内容查看
        /// </summary>
        UN1006101,
        /// <summary>
        /// 微信关键词内容-新增
        /// </summary>
        //UN1006201,
        /// <summary>
        /// 微信关键词内容-修改
        /// </summary>
        UN1006301,
        /// <summary>
        /// 微信关键词内容-删除
        /// </summary>
        UN1006401,

        /// <summary>
        /// 微信自动回复配置查看列表
        /// </summary>
        UN1008101,
        /// <summary>
        /// 微信自动回复配置新增
        /// </summary>
        UN1008102,
        /// <summary>
        /// 微信自动回复配置修改
        /// </summary>
        UN1008103,
        #endregion

        #region 微信咨询客服回复 2013-11-08 郑荣华
        /// <summary>
        /// 微信咨询列表查看
        /// </summary>
        UN1007101,

        /// <summary>
        /// 微信咨询客服回复
        /// </summary>
        UN1007201,
        #endregion

        #endregion

        #region 品胜云
        /// <summary>
        /// 品胜云路由器导航页编辑
        /// </summary>
        CL1001001,
        #endregion

        #region 门店仓库百度显示
        /// <summary>
        /// 门店仓库百度显示
        /// </summary>
        LG30001,
        PD1006502,
        #endregion

        #region 分销管理
        /// <summary>
        /// 分销等级
        /// </summary>
        SB1001,
        /// <summary>
        /// 返利记录
        /// </summary>
        SB1002,
        /// <summary>
        /// 分销商关系图
        /// </summary>
        SB1003,
        /// <summary>
        /// 会员提现
        /// </summary>
        SB1004,
        /// <summary>
        /// pos机管理创建编码
        /// </summary>
        POS100101,
        /// <summary>
        /// 销售单管理
        /// </summary>
        POS200101,
        /// <summary>
        /// 退货单管理
        /// </summary>
        POS300101,
        /// <summary>
        /// 盘点单管理
        /// </summary>
        POS400101,
        /// <summary>
        /// 会员卡等级管理
        /// </summary>
        POS500101,
        /// <summary>
        /// 积分转金额管理
        /// </summary>
        POS500201,
        /// <summary>
        /// 会员卡管理
        /// </summary>
        POS500301,
        /// <summary>
        /// 门店对账
        /// </summary>
        POS500601,
        #endregion

        #region 代理商管理
        /// <summary>
        /// 代理商管理页
        /// </summary>
        AG1001,
        /// <summary>
        /// 添加代理商
        /// </summary>
        AG1002,
        /// <summary>
        /// 修改代理商
        /// </summary>
        AG1003,
        /// <summary>
        /// 查看代理商
        /// </summary>
        AG1004,
        /// <summary>
        /// 启用/禁用代理商
        /// </summary>
        AG1005,
        /// <summary>
        /// 查看代理商资金明细
        /// </summary>
        AG2001,
        /// <summary>
        /// 代理商资金提现
        /// </summary>
        AG2002,
        /// <summary>
        /// 查看代理商预存款来往明细
        /// </summary>
        AG2003,
        #endregion

        #region 商检
        /// <summary>
        /// 商品商检管理
        /// </summary>
        ICPGoods10001,
        /// <summary>
        /// 商品商检新增
        /// </summary>
        ICPGoods10002,
        /// <summary>
        /// 订单商检管理
        /// </summary>
        ICPOrder10001,
        /// <summary>
        /// 商品备案明细
        /// </summary>
        ICPGoodsItem10001,
        #endregion

        #region  分销商申请

        /// <summary>
        /// 查看分销商申请
        /// </summary>
        DSA1002101,

        /// <summary>
        /// 分销商申请审核
        /// </summary>
        DSA1002201,

        /// <summary>
        /// 分销商申请作废
        /// </summary>
        DSA1002301,
        #endregion

        #region 供应链管理
        /// <summary>
        /// 查看供应链商品列表
        /// </summary>
        SUPPLY10001,
        /// <summary>
        /// 获取供应链商品
        /// </summary>
        SUPPLY10002,
        /// <summary>
        /// 供应链商品入库
        /// </summary>
        SUPPLY10003,
        /// <summary>
        /// 供应链订单推送
        /// </summary>
        SUPPLY10004,
        /// <summary>
        /// 供应链订单查询
        /// </summary>
        SUPPLY20001,
        #endregion

        /// <summary>
        /// 加盟申请
        /// </summary>
        DSA1002401,

        /// <summary>
        /// 分销商详活动商品领取
        /// </summary>
        DSA1002402,

        #region 经销商城快递代码管理

        /// <summary>
        /// 查看经销商城快递代码
        /// </summary>
        DS1012101,

        /// <summary>
        /// 添加经销商城快递代码
        /// </summary>
        DS1012201,

        /// <summary>
        /// 修改经销商城快递代码
        /// </summary>
        DS1012301,

        /// <summary>
        /// 删除经销商城快递代码
        /// </summary>
        DS1012401,

        #endregion
        RT102001,

        /// <summary>
        /// 意见反馈
        /// </summary>
        FB102001,
        FB102002,

        #region 保证金订单
        /// <summary>
        /// 保证金订单查询
        /// </summary>
        ST100101,

        /// <summary>
        /// 保证金订单审核
        /// </summary>
        ST100102,

        #endregion

        #region 转运系统权限
        /// <summary>
        /// 物流单号操作
        /// </summary>
        TS1009001,
        /// <summary>
        /// 出库单运费管理
        /// </summary>
        TS1008003,
        /// <summary>
        /// 出库单操作
        /// </summary>
        TS1008002,
        /// <summary>
        /// 货物出口列表
        /// </summary>
        TS1008001,
        /// <summary>
        /// 创建运单
        /// </summary>
        TS1007002,
        /// <summary>
        /// 运单列表
        /// </summary>
        TS1007001,
        /// <summary>
        /// 货物打包操作
        /// </summary>
        TS1006002,
        /// <summary>
        /// 货物打包列表
        /// </summary>
        TS1006001,
        /// <summary>
        /// 扫描打印
        /// </summary>
        TS1005001,
        /// <summary>
        /// 入库称重打印
        /// </summary>
        TS1004001,
        /// <summary>
        /// 导入包裹货物列表
        /// </summary>
        TS1003001,

        /// <summary>
        /// 导入包裹数据
        /// </summary>
        TS1003002,
        /// <summary>
        /// 修改物流信息
        /// </summary>
        TS1003004,
        /// <summary>
        /// 经销商商品列表
        /// </summary>
        TS1002001,
        /// <summary>
        /// 客户列表
        /// </summary>
        TS1001002,
        /// <summary>
        /// 添加修改客户信息
        /// </summary>
        TS1001001,
        /// <summary>
        /// 包裹查询
        /// </summary>
        TS1010001,

        /// <summary>
        /// 批次订单列表
        /// </summary>
        TS1011001,
        /// <summary>
        /// 身份证列表
        /// </summary>
        TS1012001,
        /// <summary>
        /// 单件录入
        /// </summary>
        TS1003003,
        #endregion

        #region 采购购单权限编码

        /// <summary>
        /// 创建采购基础定义权限
        /// </summary>
        CG000101,

        /// <summary>
        /// 创建采购列表
        /// </summary>
        CG100101,
        /// <summary>
        /// 创建采购单
        /// </summary>
        CG100102,
        /// <summary>
        /// 审核采购单
        /// </summary>
        CG100103,

        /// <summary>
        /// 创建采购退货列表
        /// </summary>
        CGR100101,
        /// <summary>
        /// 创建采购退货单
        /// </summary>
        CGR100102,
        /// <summary>
        /// 审核采购退货单
        /// </summary>
        CGR100103,

        /// <summary>
        /// 采购分货单列表
        /// </summary>
        CG100201,
        /// <summary>
        /// 创建采购分货单 
        /// </summary>
        CG100202,
     

        /// <summary>
        /// 采购分货单审核
        /// </summary>
        CG100203,

        /// <summary>
        /// 打包分货单列表
        /// </summary>
        CG100302,
        /// <summary>
        /// 保存物流信息
        /// </summary>
        CG100304,

        /// <summary>
        /// 配送单确认收货权限
        /// </summary>
        CG100305,
        /// <summary>
        /// 采购仓库库存
        /// </summary>
        CG100401,
        /// <summary>
        /// 配送回库
        /// </summary>
        CG100402,
        /// <summary>
        /// 财务统计列表
        /// </summary>
        FN5001001,
        /// <summary>
        /// 财务统计登记表
        /// </summary>
        FN5001002,
        /// <summary>
        /// 精准营销统计报表
        /// </summary>
        RT109001,
        
        /// <summary>
        /// 创建采购付款单
        /// </summary>
        FNCG01001,
        /// <summary>
        /// 月份仓库商品数量统计
        /// </summary>
        WHKC00001,
        /// <summary>
        /// 年度仓库库存数量统计
        /// </summary>
        WHKC00002,
        /// <summary>
        /// 销售单付款列表
        /// </summary>
        FNP000001,
        /// <summary>
        /// 定义报税商品购买定义
        /// </summary>
        SY1020001,
        /// <summary>
        /// 公司下单
        /// </summary>
        GSXD000001,
        GSXD000002,

        #endregion

        #region 库存预警
        /// <summary>
        /// 库存预警管理_查看
        /// </summary>
        SAL1001101,
        #endregion

        #region 商品备案管理
        /// <summary>
        /// 商品备案管理_查看
        /// </summary>
        PR1001001,
        #endregion

        #region 网站购物状态管理
        /// <summary>
        /// 网站购物下单状态
        /// </summary>
        WEB100001,
        /// <summary>
        /// 网站购物下单状态类型
        /// </summary>
        WEB100002,
        #endregion

        /// <summary>
        /// 快递导入功能
        /// </summary>
        TS1003012,
        TS1003013,

        /// <summary>
        /// O2O加盟申请列表/审核
        /// </summary>
        WeApply1001,
    }
}
