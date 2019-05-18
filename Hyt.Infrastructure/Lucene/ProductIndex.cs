using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.PanGu;
using Hyt.Model;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using System.Text.RegularExpressions;
using Hyt.Model.SystemPredefined;
using System.Collections;
using Hyt.Model.Transfer;
using System.Configuration;
using System.IO;
using PanGu;
using System.Data;

namespace Hyt.Infrastructure.Lucene
{
    /// <summary>
    /// 索引业务操作
    /// </summary>
    /// <remarks>2013-3-8 杨浩 创建</remarks>
    public class ProductIndex
    {


        /// <summary>
        /// 写索引对象
        /// </summary>
        private static IndexWriter _writer = null;

        /// <summary>
        /// 字段
        /// </summary>
        private static List<string> _fields = new List<string>();

        /// <summary>
        /// 修改索引对象
        /// </summary>
        private static IndexModifier _modifier = null;

        /// <summary>
        /// 关键字
        /// </summary>
        private static string _keyword = string.Empty;

        /// <summary>
        /// 索引存放路径
        /// </summary>
        public static string IndexStorePath = ConfigurationManager.AppSettings["LuceneIndexPath"]; // @"E:\Pisen\Hyt\Lucene";

        /// <summary>
        /// 索引实例
        /// </summary>
        private static ProductIndex _instance = null;

        /// <summary>
        /// 索引业务操作
        /// </summary>
        public static ProductIndex Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductIndex();

                    //初始化监听
                    if (_watcher == null)
                    {
                        _watcher = new FileSystemWatcher();
                        _watcher.Path = IndexStorePath;
                        _watcher.Filter = "*.*";
                        _watcher.Changed += new FileSystemEventHandler(OnProcess);
                        _watcher.Created += new FileSystemEventHandler(OnProcess);
                        _watcher.Deleted += new FileSystemEventHandler(OnProcess);
                        _watcher.EnableRaisingEvents = true;
                    }

                    return _instance;
                }
                return _instance;
            }
        }

        /// <summary>
        /// 搜索实例
        /// 2014-7-23 林涛 创建
        /// </summary>
        private static IndexSearcher _indexSercher = null;
        public static IndexSearcher Searcher
        {
            get
            {
                if (_indexSercher != null)
                {
                    return _indexSercher;
                }

                _indexSercher = new IndexSearcher(IndexStorePath, true);

                return _indexSercher;

            }
        }

        #region 监听索引文件修改 重新写入
        /// <summary>
        /// 监听索引实例
        /// 2014-7-24 林涛 创建
        /// </summary>
        private static FileSystemWatcher _watcher = null;


        /// <summary>
        /// 写入监听日志
        /// </summary>
        //private static FileStream _file = new FileStream(IndexStorePath+@"..\..\LuceneChange.txt", FileMode.Append, FileAccess.Write);

        #region 索引文件被监听事件
        /// <summary>
        /// 监听事件处理
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="e">消息</param>
        /// 2014-7-24 林涛 创建
        private static void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);

            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                OnChanged(source, e);

            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                OnDeleted(source, e);

            }
        }
        /// <summary>
        /// 索引文件被创建方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// 2014-7-24 林涛 创建
        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            _indexSercher = null;

            //string mesage = "索引文件被创建"+e.Name+",时间：" + DateTime.Now;
            //byte[] byteArray = System.Text.Encoding.Default.GetBytes(mesage);
            //_file.Write(byteArray, 0, byteArray.Length);

        }
        /// <summary>
        /// 索引文件被修改方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// 2014-7-24 林涛 创建
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            _indexSercher = null;

            //string mesage = "索引文件被修改" + e.Name + ",时间：" + DateTime.Now;
            //byte[] byteArray = System.Text.Encoding.Default.GetBytes(mesage);
            //_file.Write(byteArray, 0, byteArray.Length);

        }

        /// <summary>
        /// 索引文件被删除方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// 2014-7-24 林涛 创建
        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            _indexSercher = null;

            //string mesage = "索引文件被删除" + e.Name + ",时间：" + DateTime.Now;
            //byte[] byteArray = System.Text.Encoding.Default.GetBytes(mesage);
            //_file.Write(byteArray, 0, byteArray.Length);

        }

        #endregion


        #endregion

        /// <summary>
        /// 获取搜索方法
        /// </summary>
        /// <returns>索引对象</returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public IndexSearcher GetIndexSearcher()
        {
            return new IndexSearcher(IndexStorePath, true);
        }

        #region 全文索引管理 朱成果 补充方法
        /// <summary>
        /// 文档数量
        /// </summary>
        /// <returns>文档数量</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks>
        public static int? GetDocCount()
        {

            if (_writer != null)
                return _writer.NumDocs();
            else
                return null;

        }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        /// <returns>最近更新时间</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks>
        public static DateTime? GetLastUpdateTime()
        {
            if (IndexReader.IndexExists(IndexStorePath))
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(IndexReader.LastModified(IndexStorePath)).ToLocalTime();//
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Fields
        /// </summary>
        /// <returns>Fields</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks>
        public static List<string> GetFields()
        {

            if (_fields.Count < 1 && _writer != null)
            {
                _fields = _writer.GetReader().GetFieldNames(IndexReader.FieldOption.ALL).ToList();
            }
            return _fields;

        }
        #region 搜索关键词分词
        /// <summary>
        /// 搜索关键词分词
        /// </summary>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>分词结果</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        private string GetKeyWordsSplitBySpace(string keywords)
        {
            StringBuilder result = new StringBuilder();

            ICollection<WordInfo> words = new PanGuTokenizer().SegmentToWordInfos(keywords);

            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }

                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }

            return result.ToString().Trim();
        }
        #endregion
        /// <summary>
        /// 查询索引
        /// </summary>
        /// <param name="fieldName">FieldName</param>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalRecord">总的记录</param>
        /// <returns>索引列表</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks> 
        public List<CBPdProductIndex> QueryDoc(string fieldName, string keywords, int pageIndex, int pageSize, out int totalRecord)
        {
            var search = new IndexSearcher(IndexStorePath);
            Query searchQuery;
            if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(keywords))
            {
                #region [关键字查询]
                var query = new BooleanQuery();
                BooleanQuery childQuery;
                BooleanQuery esenQuery;
                if (fieldName == "ProductName")
                {
                    #region 2016-4-6 杨浩 新增模糊搜索
                    childQuery = new BooleanQuery();
                    esenQuery = new BooleanQuery();
                    //模糊搜索
                    //esenQuery.Add(new FuzzyQuery(new Term("ProductName", Regex.Replace(keywords, @"\s", ""))), BooleanClause.Occur.SHOULD);

                    //esenQuery.SetBoost(4.0F);


                    //分词 盘古分词
                    var keyWordsSplitBySpace = GetKeyWordsSplitBySpace(keywords);

                  
                    //string keyWordsSplitBySpace = string.Format("{0}^{1}.0", keywords, (int)Math.Pow(3, 5));
                    //不启用分词，直接用模糊搜索
                    QueryParser productNameQueryParser = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_29, "ProductName", new PanGuAnalyzer(true));
                    Query productNameQuery = productNameQueryParser.Parse(keyWordsSplitBySpace);
                    childQuery.Add(productNameQuery, BooleanClause.Occur.SHOULD);

                    //以什么开头，输入“ja”就可以搜到包含java和javascript两项结果了
                    Query prefixQuery_productName = new PrefixQuery(new Term("ProductName", keywords.Trim()));

                    //直接模糊匹配,假设你想搜索跟‘wuzza’相似的词语,你可能得到‘fuzzy’和‘wuzzy’。
                    Query fuzzyQuery_productName = new FuzzyQuery(new Term("ProductName", keywords.Trim()));
                    //通配符搜索
                    Query wildcardQuery_productName = new WildcardQuery(new Term("ProductName", string.Format("{0}", keywords.Trim())));

                    childQuery.Add(prefixQuery_productName, BooleanClause.Occur.SHOULD);
                    childQuery.Add(fuzzyQuery_productName, BooleanClause.Occur.SHOULD);
                    childQuery.Add(wildcardQuery_productName, BooleanClause.Occur.SHOULD);
                    childQuery.SetBoost(4.0F);


                    //childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);
                    query.Add(childQuery, BooleanClause.Occur.MUST);
                    #endregion

                    //childQuery = new BooleanQuery();
                    //esenQuery = new BooleanQuery();
                    ////全词去空格
                    //esenQuery.Add(new TermQuery(new Term("ProductName", Regex.Replace(keywords, @"\s", ""))),
                    //        BooleanClause.Occur.SHOULD);
                    //esenQuery.SetBoost(3.0F);
                    //childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                    //esenQuery = new BooleanQuery();
                    ////分词 盘古分词
                    //esenQuery.Add(new QueryParser("ProductName", new PanGuAnalyzer(true)).Parse(keywords),
                    //    BooleanClause.Occur.SHOULD);

                    ////分词  按空格
                    //var keyColl = keywords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (var item in keyColl)
                    //{
                    //    esenQuery.Add(new TermQuery(new Term("ProductName", item)),
                    //        BooleanClause.Occur.SHOULD);
                    //}
                    //esenQuery.SetBoost(2.9F);
                    //childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);
                    //query.Add(childQuery, BooleanClause.Occur.MUST);
                }

                else if (fieldName == "Category")
                {
                    childQuery = new BooleanQuery();
                    esenQuery = new BooleanQuery();
                    esenQuery.Add(new TermQuery(new Term("Category", keywords)),
                               BooleanClause.Occur.SHOULD);
                    esenQuery.SetBoost(3.0F);
                    childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                    esenQuery = new BooleanQuery();
                    esenQuery.Add(new WildcardQuery(new Term("AssociationCategory", string.Format("*,{0},*", keywords))),
                            BooleanClause.Occur.SHOULD);
                    esenQuery.SetBoost(2.8F);
                    childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }

                else if (fieldName == "BrandSysNo")
                {
                    childQuery = new BooleanQuery();
                    childQuery.Add(new TermQuery(new Term("BrandSysNo", keywords)),
                               BooleanClause.Occur.SHOULD);
                    childQuery.SetBoost(3.0F);
                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }
                else if (fieldName == "DealerSysNos")
                {
                    childQuery = new BooleanQuery();
                    childQuery.Add(new WildcardQuery(new Term("DealerSysNos", string.Format("*,{0},*", keywords))),
                          BooleanClause.Occur.SHOULD);
                    childQuery.SetBoost(2.8F);
                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }
                else if (fieldName == "ProductGroupCode")
                {
                    childQuery = new BooleanQuery();
                    childQuery.Add(new WildcardQuery(new Term("ProductGroupCode", string.Format("*,{0},*", keywords))),
                          BooleanClause.Occur.SHOULD);
                    childQuery.SetBoost(2.8F);
                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }
                else
                {
                    query.Add(new TermQuery(new Term(fieldName, keywords)),
                              BooleanClause.Occur.SHOULD);
                }
                #endregion
                searchQuery = query;
            }
            else
            {
                searchQuery = new WildcardQuery(new Term("ProductName", "*雪花秀*"));
            }
            //排序方式
            var sort = new Sort();
            //搜索
            Hits hits = search.Search(searchQuery, sort);

            totalRecord = hits.Length();//总的记录
            int startIndex = (pageIndex - 1) * pageSize;
            if (startIndex < 0) startIndex = 0;
            int endIndex = startIndex + pageSize;
            if (endIndex > totalRecord - 1) endIndex = totalRecord - 1;
            List<CBPdProductIndex> lst = new List<CBPdProductIndex>();
            for (int i = startIndex; i <= endIndex; i++)
            {
                var doc = hits.Doc(i);
                lst.Add(
                    new CBPdProductIndex
                    {
                        DocID = hits.Id(i),
                        Score = hits.Score(i),
                        AssociationCategory = doc.Get("AssociationCategory"),
                        Attributes = doc.Get("Attributes"),
                        Barcode = doc.Get("Barcode"),
                        BrandSysNo = Convert.ToInt32(doc.Get("BrandSysNo")),
                        Category = Convert.ToInt32(doc.Get("Category")),
                        DisplayOrder = Convert.ToInt32(doc.Get("DisplayOrder")),
                        NameAcronymy = doc.Get("NameAcronymy"),
                        Prices = doc.Get("Prices"),
                        ProductImage = doc.Get("ProductImage"),
                        ProductName = doc.Get("ProductName"),
                        QRCode = doc.Get("QRCode"),
                        Status = Convert.ToInt32(doc.Get("Status")),
                        SysNo = Convert.ToInt32(doc.Get("SysNo")),
                        BasicPrice = Convert.ToDecimal(doc.Get("BasicPrice")),
                        Price = Convert.ToDecimal(doc.Get("Price")),
                        DispalySymbol = 0,
                        RankPrice = 0.00M,
                        ProductGroupCode = Convert.ToString(doc.Get("ProductGroupCode")),
                        DealerSysNos = doc.Get("DealerSysNos"),
                        WarehouseSysNos = doc.Get("WarehouseSysNos")
                    });
            }
            search.Close();
            return lst;
        }
        #endregion

        /// <summary>
        /// 最大合并因子
        /// </summary>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public int MaxMergeFactor
        {
            get
            {
                if (_writer != null)
                    return _writer.GetMergeFactor();
                else
                    return 0;
            }
            set
            {
                if (_writer != null)
                    _writer.SetMergeFactor(value);
            }
        }

        /// <summary>
        /// 最大合并文档数
        /// </summary>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public int MaxMergeDocs
        {
            get
            {
                if (_writer != null)
                    return _writer.GetMaxMergeDocs();
                else
                    return 0;
            }
            set
            {
                if (_writer != null)
                    _writer.SetMaxMergeDocs(value);
            }
        }

        /// <summary>
        /// 最大缓存文档数
        /// </summary>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public int MaxBufferedDocs
        {
            get
            {
                if (_writer != null)
                    return _writer.GetMaxBufferedDocs();
                else
                    return int.MaxValue;
            }
            set
            {
                if (_writer != null)
                    _writer.SetMaxBufferedDocs(value);
            }
        }

        /// <summary>
        /// 获取或设置索引文件夹路径
        /// </summary>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public string Path
        {
            get { return IndexStorePath; }
            set { IndexStorePath = value; }
        }

        /// <summary>
        /// 初始化索引路径
        /// </summary>
        /// <param name="isCreate">是否创建</param>
        /// <returns></returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public void CreateIndex(bool isCreate)
        {
            _writer = new IndexWriter(IndexStorePath, new PanGuAnalyzer(), isCreate);
        }

        /// <summary>
        /// 关闭索引(并优化)
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public void Close()
        {
            _writer.Optimize();
            _writer.Close();
        }

        /// <summary>
        /// 关闭索引(无优化)
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public void CloseWithoutOptimize()
        {
            _writer.Close();
        }

        /// <summary>
        /// 盘古分词
        /// </summary>
        /// <param name="keywords">分词关键字</param>
        /// <returns>分词后的字符</returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public static string Participle(string keywords)
        {
            StringBuilder result = new StringBuilder();
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            ICollection<PanGu.WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            if (words.Count != 0)
                _keyword = words.Max().Word;//记录用户查询关键字
            foreach (var word in words.Where(word => word != null))
            {
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }

        /// <summary>
        /// 分析并获取用户输入的关键字
        /// </summary>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public string GetKeyWord
        {
            get { return _keyword; }
        }

        /// <summary>
        /// 创建文件索引
        /// </summary>
        /// <param name="model">索引实体</param>
        /// <returns>创建的索引文件序号</returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public int IndexString(Model.PdProductIndex model)
        {
            _writer.AddDocument(ModelToDocument(model));
            int num = _writer.MaxDoc();

            return num;
        }

        /// <summary>
        /// 删除索引
        ///</summary>
        ///<param name="model">索引实体</param>
        ///<returns></returns>
        ///<remarks>2013-3-8 杨浩 创建</remarks>
        public void DeleteIndex(Hyt.Model.PdProductIndex model)
        {
            try
            {
                _modifier = new IndexModifier(IndexStorePath, new PanGuAnalyzer(), false);
                Term term = new Term("SysNo", model.SysNo.ToString());
                _modifier.DeleteDocuments(term);//删除 
            }
            catch
            {
                //TODO:此处实现日志异常记录
            }
            finally
            {
                _modifier.Close();
            }
        }

        /// <summary>
        /// 删除索引
        ///</summary>
        ///<param name="productSysNo">产品系统编号</param>
        ///<returns></returns>
        ///<remarks>2013-3-8 杨浩 创建</remarks>
        public void DeleteIndex(int productSysNo)
        {
            try
            {
                _modifier = new IndexModifier(IndexStorePath, new PanGuAnalyzer(), false);
                Term term = new Term("SysNo", productSysNo.ToString());
                _modifier.DeleteDocuments(term);//删除 
            }
            catch
            {
                //TODO:此处实现日志异常记录
            }
            finally
            {
                _modifier.Close();
            }
        }

        /// <summary>
        /// 增加索引
        ///</summary>
        ///<param name="model">索引实体</param>
        ///<returns></returns>
        ///<remarks>2013-3-8 杨浩 创建</remarks>
        public void AddIndex(Hyt.Model.PdProductIndex model)
        {
            try
            {
                _modifier = new IndexModifier(IndexStorePath, new PanGuAnalyzer(), false);
                _modifier.AddDocument(ModelToDocument(model));
            }
            catch
            {
                //TODO:此处实现日志异常记录
            }
            finally
            {
                _modifier.Flush();
                _modifier.Close();
            }
        }

        /// <summary>
        /// 修改索引
        ///</summary>
        ///<param name="model">索引实体</param>
        ///<returns></returns>
        ///<remarks>2013-3-8 杨浩 创建</remarks>
        public void UpdateIndex(Hyt.Model.PdProductIndex model)
        {
            var term = new Term("SysNo", model.SysNo.ToString());
            CreateIndex(false);
            _writer.UpdateDocument(term, ModelToDocument(model));
            _writer.Optimize();//Optimize通常需要执行一下，否则索引文件中会有两个相同id的索引
            _writer.Close();
        }

        /// <summary>
        /// 是否已经创建索引文件
        /// </summary>
        /// <returns>true:已创建 false:未创建</returns>
        /// <remarks>2013-3-8 杨浩 创建</remarks>
        public static bool IsEnableCreated()
        {
            if (global::Lucene.Net.Index.IndexReader.IndexExists(IndexStorePath))
                return true;
            else
                return false;
        }

        #region 文档实体转换

        /// <summary>
        /// 将DataRow转换为实体
        /// </summary>
        /// <param name="doc">文档</param>
        /// <returns>实体</returns>
        /// <remarks>2013-08-08 杨浩创建</remarks>
        public PdProductIndex DataRowToModel(DataRow row)
        {    
            DateTime createDate=DateTime.Now;
            DateTime.TryParse(row["CreatedDate"].ToString(),out createDate);
            decimal basicPrice = 0;
            decimal.TryParse(row["BasicPrice"].ToString(), out basicPrice);
            decimal price = 0;
            decimal.TryParse(row["Price"].ToString(), out price);
            decimal tax = 0;
            decimal.TryParse(row["Tax"].ToString(), out tax);
            return new PdProductIndex
            {
                AssociationCategory = row["AssociationCategory"].ToString(),
                Attributes = row["Attributes"].ToString(),
                Barcode = row["Barcode"].ToString(),
                BrandSysNo = Convert.ToInt32(row["BrandSysNo"].ToString()),
                Category = Convert.ToInt32(row["Category"].ToString()),
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"].ToString()),
                NameAcronymy = row["NameAcronymy"].ToString(),
                Prices = row["Prices"].ToString(),
                ProductImage = row["ProductImage"].ToString(),
                ProductName = row["ProductName"].ToString(),
                ProductSubName = row["ProductSubName"].ToString(),
                QRCode = row["QRCode"].ToString(),
                Status = Convert.ToInt32(row["Status"].ToString()),
                SysNo = Convert.ToInt32(row["SysNo"].ToString()),
                BasicPrice = basicPrice,
                Price = price,
                CommentCount = row["CommentCount"].ToString() == "" ? 0 : Convert.ToInt32(row["CommentCount"].ToString()),
                SalesCount = row["SalesCount"].ToString() == "" ? 0 : Convert.ToInt32(row["SalesCount"].ToString()),
                DispalySymbol = 0,
                RankPrice = 0.00M,
                AverageScore = row["AverageScore"].ToString() == "" ? 0 : Convert.ToDouble(row["AverageScore"].ToString()),
                TotalScore = row["TotalScore"].ToString() == "" ? 0 : Convert.ToDouble(row["TotalScore"].ToString()),
                Shares = row["Shares"].ToString() == "" ? 0 : Convert.ToInt32(row["Shares"].ToString()),
                Question = row["Question"].ToString() == "" ? 0 : Convert.ToInt32(row["Question"].ToString()),
                Liking = row["Liking"].ToString() == "" ? 0 : Convert.ToInt32(row["Liking"].ToString()),
                Favorites = row["Favorites"].ToString() == "" ? 0 : Convert.ToInt32(row["Favorites"].ToString()),
                Comments = row["Favorites"].ToString() == "" ? 0 : Convert.ToInt32(row["CommentCount"].ToString()),
                CreatedDate = createDate,
                CanFrontEndOrder =Convert.ToInt32(row["CanFrontEndOrder"].ToString()),
                ErpCode = row["ErpCode"].ToString(),
                ProductType =row["ProductType"].ToString()==""?10:int.Parse(row["ProductType"].ToString()),
                OriginSysNo =row["OriginSysNo"].ToString()==""?0:int.Parse(row["OriginSysNo"].ToString()),
                Tax = tax,
            };
        }
        /// <summary>
        /// 将索引文档转换为实体
        /// </summary>
        /// <param name="doc">文档</param>
        /// <returns>实体</returns>
        /// <remarks>2013-08-08 黄波 创建</remarks>
        public PdProductIndex DocumentToModel(Document doc)
        {
            return new PdProductIndex
            {
                AssociationCategory = doc.Get("AssociationCategory"),
                Attributes = doc.Get("Attributes"),
                Barcode = doc.Get("Barcode"),
                BrandSysNo = Convert.ToInt32(doc.Get("BrandSysNo")),
                Category = Convert.ToInt32(doc.Get("Category")),
                DisplayOrder = Convert.ToInt32(doc.Get("DisplayOrder")),
                NameAcronymy = doc.Get("NameAcronymy"),
                Prices = doc.Get("Prices"),
                ProductImage = doc.Get("ProductImage"),
                ProductName = doc.Get("ProductName"),
                ProductSubName = doc.Get("ProductSubName"),
                QRCode = doc.Get("QRCode"),
                Status = Convert.ToInt32(doc.Get("Status")),
                SysNo = Convert.ToInt32(doc.Get("SysNo")),
                BasicPrice = Convert.ToDecimal(doc.Get("BasicPrice")),
                CommentCount = Convert.ToInt32(doc.Get("CommentCount")),
                SalesCount = Convert.ToInt32(doc.Get("SalesCount")),
                DispalySymbol = 0,
                RankPrice = 0.00M,
                AverageScore = Convert.ToDouble(doc.Get("AverageScore")),
                TotalScore = Convert.ToDouble(doc.Get("TotalScore")),
                Shares = Convert.ToInt32(doc.Get("Shares")),
                Question = Convert.ToInt32(doc.Get("Question")),
                Liking = Convert.ToInt32(doc.Get("Liking")),
                Favorites = Convert.ToInt32(doc.Get("Favorites")),
                Comments = Convert.ToInt32(doc.Get("Comments")),
                CreatedDate = string.IsNullOrEmpty(doc.Get("CreatedDate")) ? new DateTime() : DateTime.Parse(doc.Get("CreatedDate")),
                CanFrontEndOrder = Convert.ToInt32(doc.Get("CanFrontEndOrder")),
                ErpCode = doc.Get("ErpCode"),
                DealerSysNos = doc.Get("DealerSysNos"),
                WarehouseSysNos = doc.Get("WarehouseSysNos"),
                Price = Convert.ToDecimal(doc.Get("Price")),
                ProductType = Convert.ToInt32(doc.Get("ProductType")),
                Tax = Convert.ToDecimal(doc.Get("Tax")),
            };
        }

        /// <summary>
        /// 构造索引文档对象
        /// </summary>
        /// <param name="model">商品索引实体</param>
        /// <returns>索引文档</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        /// <remarks>2013-12-23 邵斌 添加前台是否下单字段</remarks>
        public Document ModelToDocument(Model.PdProductIndex model)
        {
            var doc = new Document();
            Field field;

            field = new Field("SysNo", model.SysNo.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(2.0F);
            doc.Add(field);

            field = new Field("Barcode", model.Barcode ?? "", Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(2.0F);
            doc.Add(field);

            field = new Field("ProductName", model.ProductName ?? "", Field.Store.YES, Field.Index.ANALYZED);
            //field = new Field("ProductName", model.ProductName ?? "", Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(1.8F);
            doc.Add(field);

            field = new Field("ProductSubName", model.ProductSubName ?? "", Field.Store.YES, Field.Index.ANALYZED);
            field.SetBoost(1.8F);
            doc.Add(field);

            field = new Field("Category", model.Category.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(1.8F);
            doc.Add(field);

            field = new Field("BrandSysNo", model.BrandSysNo.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(0.9F);
            doc.Add(field);

            field = new Field("AssociationCategory", (model.AssociationCategory == null ? "," + model.Category.ToString() + "," : model.AssociationCategory).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(1.0F);
            doc.Add(field);

            field = new Field("Attributes", model.Attributes==null?"":model.Attributes, Field.Store.YES, Field.Index.NOT_ANALYZED);
            field.SetBoost(1.0F);
            doc.Add(field);

            field = new Field("DisplayOrder", model.DisplayOrder.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("ProductImage", model.ProductImage ?? "", Field.Store.YES, Field.Index.NO);
            doc.Add(field);

            field = new Field("NameAcronymy", model.NameAcronymy ?? "", Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Prices", model.Prices ?? "", Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);


            field = new Field("QRCode", model.QRCode ?? "", Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("BasicPrice", model.BasicPrice.ToString(Constant.DecimalFormatWithGroup), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Status", model.Status.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("CommentCount", model.CommentCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("SalesCount", model.SalesCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("AverageScore", model.AverageScore.ToString(Constant.DecimalFormatWithGroup), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Favorites", model.Favorites.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Liking", model.Liking.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Question", model.Question.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Shares", model.Shares.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("TotalScore",  model.TotalScore.ToString(Constant.DecimalFormatWithGroup), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("CreatedDate", model.CreatedDate.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            //2013-12-23 邵斌 添加前台是否下单字段
            field = new Field("CanFrontEndOrder", model.CanFrontEndOrder.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("ErpCode", model.ErpCode, Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("ProductGroupCode", model.ProductGroupCode, Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("OriginSysNo", model.OriginSysNo.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("IsFrontDisplay", model.IsFrontDisplay.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("DealerSysNos", string.IsNullOrEmpty(model.DealerSysNos) ? "" : model.DealerSysNos, Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            //2016-7-8 罗远康 添加仓库字段
            field = new Field("WarehouseSysNos", string.IsNullOrEmpty(model.WarehouseSysNos) ? "" : model.WarehouseSysNos, Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("Price", model.Price.ToString(Constant.DecimalFormatWithGroup), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            field = new Field("ProductType", model.ProductType.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);

            return doc;
        }
        #endregion
    }
}