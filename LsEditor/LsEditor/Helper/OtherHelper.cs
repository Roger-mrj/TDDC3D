using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.Output;
namespace RCIS.Helper
{
   public class OtherHelper
    {
       
       public static void ReleaseComObject(object o)
       {
           GC.Collect();
           GC.WaitForPendingFinalizers();
           ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(o);
           System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
       }
       public static string strPreZDH;
       /// <summary>
       /// 根据表名获得字段名
       /// </summary>
       /// <param name="sTableName"></param>
       /// <returns></returns>
       public static ArrayList GetFieldName(string sTableName)
       {
           if (sTableName == "") return null;

           ArrayList sList = new ArrayList();

           if(sList.Count!=0) 
               sList.Clear();
           string sql = "select * from " + sTableName;

           DataSet ds = Helper.DataOperateHelper.getDataSet(sql, sTableName);

           int sColNum = ds.Tables[sTableName].Columns.Count;

           string sFieldName = "";
           for (int i = 0; i < sColNum; i++)
           {
               sFieldName = GetFieldDMMC(sTableName, ds.Tables[0].Columns[i].ToString());
               if (sFieldName != "")
               {
                   sList.Add(sFieldName);
               }
           }
           return sList;
       }
       #region 转换字段
       public static string GetFieldDMMC(string sTableName, string sFieldName)
       {
           if (sTableName == "") return "";
           if (sFieldName == "") return "";
           string ss = "";
            #region 权利人
           if (sTableName == "ZD_QLR")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "QSDWDM":
                       ss = "QSDWDM|权属单位代码";
                       break;
                   case "QSRMC":
                       ss = "QSRMC|权属人名称";
                       break;
                   case "DLRXM":
                       ss = "DLRXM|代理人姓名";
                       break;
                   case "DLRZJLX":
                       ss = "DLRZJLX|代理人证件类型";
                       break;
                   case "DLRZJH":
                       ss = "DLRZJH|代理人证件号";
                       break;
                   case "DLRSFZMS":
                       ss = "DLRSFZMS|代理人身份证明书";
                       break;
                   case "DLRDHHM":
                       ss = "DLRDHHM|代理人电话号码";
                       break;
                   case "TDZH":
                       ss = "TDZH|土地证号";
                       break;
                   case "QLRZJLX":
                       ss = "QLRZJLX|权利人证件类型";
                       break;
                   case "QLRZJH":
                       ss = "QLRZJH|权利人证件号";
                       break;
                   case "FRDBXM":
                       ss = "FRDBXM|法人代表姓名";
                       break;
                   case "FRDBZJLX":
                       ss = "FRDBZJLX|法人代表证件类型";
                       break;
                   case "FRDBZJH":
                       ss = "FRDBZJH|法人代表证件号";
                       break;
                   case "FRDBSFZMS":
                       ss = "FRDBSFZMS|法人代表身份证明书";
                       break;
                   case "FRDBDHHM":
                       ss = "FRDBDHHM|法人代表电话号码";
                       break;
                   case "QLRBM":
                       ss = "QLRBM|权利人编码";
                       break;
                   case "DWXZ":
                       ss = "DWXZ|单位性质";
                       break;
                   case "YTDZH":
                       ss = "YTDZH|原土地证号";
                       break;
                   case "YQLRMC":
                       ss = "YQLRMC|原权利人名称";
                       break;
                   case "YQLRZJLX":
                       ss = "YQLRZJLX|原权利人证件类型";
                       break;
                   case "YQLRZJH":
                       ss = "YQLRZJH|原权利人证件号";
                       break;
                   case "YQLRDHHM":
                       ss = "YQLRDHHM|原权利人电话号码";
                       break;
                   case "YTXDZ":
                       ss = "YTXDZ|原通讯地址";
                       break;
                   case "QLRZJ":
                       ss = "QLRZJ|权利人证件";
                       break;
                   case "SJZGBM":
                       ss = "SJZGBM|上级主管部门";
                       break;
                   default:
                       ss = "";
                       break;
               }
           }
#endregion
            #region 宗地
           else if (sTableName == "ZD_ATT")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "YSDM":
                       ss = "YSDM|要素代码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "ZDSZ":
                       ss = "ZDSZ|宗地四至";
                       break;
                   case "QSDWDM":
                       ss = "QSDWDM|权属单位代码";
                       break;
                   case "ZLDWDM":
                       ss = "ZLDWDM|坐落单位代码";
                       break;
                   case "QSXZ":
                       ss = "QSXZ|权属性质";
                       break;
                   case "SYQLX":
                       ss = "SYQLX|使用权类型";
                       break;
                   case "YT":
                       ss = "YT|土地用途";
                       break;
                   case "SCMJ":
                       ss = "SCMJ|实测面积";
                       break;
                   case "FZMJ":
                       ss = "FZMJ|发证面积";
                       break;
                   case "JZRJL":
                       ss = "JZRJL|建筑容积率";
                       break;
                   case "JZMD":
                       ss = "JZMD|建筑密度";
                       break;
                   case "TDJB":
                       ss = "TDJB|土地级别";
                       break;
                   case "SBDJ":
                       ss = "SBDJ|申报地价";
                       break;
                   case "QDJG":
                       ss = "QDJG|取得价格";
                       break;
                   case "TXDZ":
                       ss = "TXDZ|通讯地址";
                       break;
                   case "TDZL":
                       ss = "TDZL|土地座落";
                       break;
                   case "TDXZ":
                       ss = "TDXZ|土地性质";
                       break;
                   case "ZZRQ":
                       ss = "ZZRQ|终止日期";
                       break;
                   case "SYQX":
                       ss = "SYQX|使用期限";
                       break;
                   case "GYSYQQK":
                       ss = "GYSYQQK|共有所有权情况";
                       break;
                   case "PZYT":
                       ss = "PZYT|批准用途";
                       break;
                   case "PZLB":
                       ss = "PZLB|批准类别";
                       break;
                   case "DSWQS":
                       ss = "DSWQS|地上物权属";
                       break;
                   case "GYSYQK":
                       ss = "GYSYQK|共有使用权情况";
                       break;
                   case "TDDJTGDJQ":
                       ss = "TDDJTGDJQ|土地登记通告登记区";
                       break;
                   case "TDDJTG":
                       ss = "TDDJTG|土地登记通告";
                       break;
                   default:
                       ss = "";
                       break;
               }

           }
#endregion
            #region 权属审批
           else if (sTableName == "ZD_QSSP")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "SYQMJ":
                       ss = "SYQMJ|使用权面积";
                       break;
                   case "DYMJ":
                       ss = "DYMJ|独用面积";
                       break;
                   case "GYMJ":
                       ss = "GYMJ|共用面积";
                       break;
                   case "FTMJ":
                       ss = "FTMJ|分摊面积";
                       break;
                   case "SPBH":
                       ss = "SPBH|审批表号";
                       break;
                   case "CSYJ":
                       ss = "CSYJ|初审意见";
                       break;
                   case "SCR":
                       ss = "SCR|审查人";
                       break;
                   case "SCRQ":
                       ss = "SCRQ|审查日期";
                       break;
                   case "SHYJ":
                       ss = "SHYJ|审核意见";
                       break;
                   case "SHR":
                       ss = "SHR|审核人";
                       break;
                   case "SHRQ":
                       ss = "SHRQ|审核日期";
                       break;
                   case "GGRQ":
                       ss = "GGRQ|公告日期";
                       break;
                   case "GGJG":
                       ss = "GGJG|公告结果";
                       break;
                   case "PZYJ":
                       ss = "PZYJ|批准意见";
                       break;
                   case "SPR":
                       ss = "SPR|审批人";
                       break;
                   case "PZRQ":
                       ss = "PZRQ|批准日期";
                       break;
                   case "LDPS":
                       ss = "LDPS|领导批示";
                       break;
                   case "CFQK":
                       ss = "CFQK|查封情况";
                       break;
                   case "QLRBM":
                       ss = "QLRBM|权利人编码";
                       break;
                   case "TDCRH":
                       ss = "TDCRH|土地出让金交费情况";
                       break;
                   case "LJR":
                       ss = "LJR|立卷人";
                       break;
                   case "HJR":
                       ss = "HJR|核卷人";
                       break;
                   case "GDRQ":
                       ss = "GDRQ|归档日期";
                       break;
                   case "BKB":
                       ss = "BKB|备考表";
                       break;
                   default:
                       ss = "";
                       break;
               }

           }
#endregion
            #region 权属调查
           else if (sTableName == "ZD_QSDC")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "QSDWDM":
                       ss = "QSDWDM|权属单位代码";
                       break;
                   case "DCBH":
                       ss = "DCBH|调查表号";
                       break;
                   case "ZJWTS":
                       ss = "ZJWTS|指界委托书";
                       break;
                   case "YBDJH":
                       ss = "YBDJH|预编地籍号";
                       break;
                  
                   case "SM":
                       ss = "SM|说明";
                       break;
                   case "QSDCJS":
                       ss = "QSDCJS|权属调查记事";
                       break;
                   case "DCY":
                       ss = "DCY|调查员";
                       break;
                   case "DCRQ":
                       ss = "DCRQ|调查日期";
                       break;
                   case "ZDCT":
                       ss = "ZDCT|宗地草图";
                       break;
                   case "JZBS":
                       ss = "JZBS|界址标识";
                       break;
                   case "DJKZJS":
                       ss = "DJKZJS|地籍勘丈记事";
                       break;
                   case "KZY":
                       ss = "KZY|勘丈员";
                       break;
                   case "KZRQ":
                       ss = "KZRQ|勘丈日期";
                       break;
                   case "DCSHYJ":
                       ss = "DCSHYJ|调查审核意见";
                       break;
                   case "DCSHR":
                       ss = "DCSHR|调查审核人";
                       break;
                   case "DCSHRQ":
                       ss = "DCSHRQ|调查审核日期";
                       break;
                   case "ZJTZS":
                       ss = "ZJTZS|指界通知书";
                       break;
                   case "JZB":
                       ss = "JZB|界址表";
                       break;
                   case "WYDJTZS":
                       ss = "WYDJTZS|违约定界通知书";
                       break;
                   default:
                       ss = "";
                       break;
               }
           }
#endregion
            #region 权属来源证明
           else if (sTableName == "ZD_QSLYZM")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "QSDWDM":
                       ss = "QSDWDM|权属单位代码";
                       break;
                   case "QSZMWJLX":
                       ss = "QSZMWJLX|权属来源证明文件类型";
                       break;
                   case "QSZMWJBH":
                       ss = "QSZMWJBH|权属来源证明文件编号";
                       break;
                   case "QSLYZM":
                       ss = "QSLYZM|权属来源证明";
                       break;
                   case "TDZH":
                       ss = "TDZH|土地证号";
                       break;
                   case "QSZMWJRQ":
                       ss = "QSZMWJRQ|权属来源证明文件日期";
                       break;
                   default:
                       ss = "";
                       break;
               }
           }
#endregion
            #region 申请登记
           else if (sTableName == "ZD_SQDJ")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "TDZH":
                       ss = "TDZH|土地证号";
                       break;
                   case "SQSBH":
                       ss = "SQSBH|申请书编号";
                       break;
                   case "SQS":
                       ss = "SQS|申请书";
                       break;
                   case "SJR":
                       ss = "SJR|收件人";
                       break;
                   case "SJRQ":
                       ss = "SJRQ|收件日期";
                       break;
                   case "SJD":
                       ss = "SJD|收件单";
                       break;
                   case "QLRBM":
                       ss = "QLRBM|权利人编码";
                       break;
                   case "YQLRYJ":
                       ss = "YQLRYJ|原权利人及所有权意见";
                       break;
                   case "YJQRRQ":
                       ss = "YJQRRQ|意见确认日期";
                       break;
                   case "SQZCN":
                       ss = "SQZCN|申请者承诺";
                       break;
                   case "SQRQ":
                       ss = "SQRQ|申请日期";
                       break;
                   default:
                       ss = "";
                       break;
               }
           }
#endregion
            #region 注册登记
           else if (sTableName == "ZD_ZCDJ")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "QSDWDM":
                       ss = "QSDWDM|权属单位代码";
                       break;
                   case "DJKBH":
                       ss = "DJKBH|登记卡编号";
                       break;
                   case "DJRQ":
                       ss = "DJRQ|登记日期";
                       break;
                   case "DJJS":
                       ss = "DJJS|登记记事";
                       break;
                   case "DJKJBR":
                       ss = "DJKJBR|登记卡经办人";
                       break;
                   case "DJKSHR":
                       ss = "DJKSHR|登记卡审核人";
                       break;
                   case "TDZH":
                       ss = "TDZH|土地证号";
                       break;
                   case "GHKH":
                       ss = "GHKH|归户卡号";
                       break;
                   case "QLRBM":
                       ss = "QLRBM|权利人编码";
                       break;
                   case "DJKXB":
                       ss = "DJKXB|登记卡续表";
                       break;
                   case "JKR":
                       ss = "JKR|缴款人";
                       break;
                   case "DJKZF":
                       ss = "DJKZF|登记勘丈费";
                       break;
                   case "GBF":
                       ss = "GBF|工本费";
                       break;
                   case "HJ":
                       ss = "HJ|合计";
                       break;
                   case "ZSBH":
                       ss = "ZSBH|证书编号";
                       break;
                   case "SZR":
                       ss = "SZR|缮证人";
                       break;
                   case "HZR":
                       ss = "HZR|核证人";
                       break;
                   case "FZR":
                       ss = "FZR|发证人";
                       break;
                   case "LZR":
                       ss = "LZR|领证人";
                       break;
                   case "LZRQ":
                       ss = "LZRQ|领证日期";
                       break;
                   default:
                       ss = "";
                       break;
               }
           }
#endregion
            #region 他项
           else if (sTableName == "ZD_TXQLDJ")
           {
               switch (sFieldName.ToUpper())
               {
                   case "OBJECTID":
                       ss = "OBJECTID|ID";
                       break;
                   case "BSM":
                       ss = "BSM|标识码";
                       break;
                   case "DJH":
                       ss = "DJH|地籍号";
                       break;
                   case "QSDWDM":
                       ss = "QSDWDM|权利单位代码";
                       break;
                   case "TXQLR":
                       ss = "TXQLR|他项权利人";
                       break;
                   case "TXQLRSFZJLX":
                       ss = "TXQLRSFZJLX|他项权利人身份证件类型";
                       break;
                   case "YWR":
                       ss = "YWR|义务人";
                       break;
                   case "YWRSFZJH":
                       ss = "YWRSFZJH|义务人身份证号";
                       break;
                   case "TXQLRSFZJH":
                       ss = "TXQLRSFZJH|他项权利人身份证件号";
                       break;
                   case "YWRSFZJLX":
                       ss = "YWRSFZJLX|义务人身份证件类型";
                       break;
                   case "TXQLZL":
                       ss = "TXQLZL|他项权利种类";
                       break;
                   case "TXQLFW":
                       ss = "TXQLFW|他项权利范围";
                       break;
                   case "SDRQ":
                       ss = "SDRQ|设定日期";
                       break;
                   case "QLSX":
                       ss = "QLSX|权利顺序";
                       break;
                   case "XCQX":
                       ss = "XCQX|续存期限";
                       break;
                   case "DJKBH":
                       ss = "DJKBH|登记卡编号";
                       break;
                   case "TXQLZH":
                       ss = "TXQLZH|他项权利证号";
                       break;
                   case "TDZH":
                       ss = "TDZH|土地证号";
                       break;
                   case "SQSBH":
                       ss = "SQSBH|申请书编号";
                       break;
                   case "SQS":
                       ss = "SQS|申请书";
                       break;
                   case "SJR":
                       ss = "SJR|收件人";
                       break;
                   case "SJRQ":
                       ss = "SJRQ|收件日期";
                       break;
                   case "SJD":
                       ss = "SJD|收件单";
                       break;
                   case "SPBH":
                       ss = "SPBH|审批表号";
                       break;
                   case "CSYJ":
                       ss = "CSYJ|初审意见";
                       break;
                   case "SCR":
                       ss = "SCR|审查人";
                       break;
                   case "SCRQ":
                       ss = "SCRQ|审查日期";
                       break;
                   case "SHYJ":
                       ss = "SHYJ|审核意见";
                       break;
                   case "SHR":
                       ss = "SHR|审核人";
                       break;
                   case "SHRQ":
                       ss = "SHRQ|审核日期";
                       break;
                   case "SPR":
                       ss = "SPR|审批人";
                       break;
                   case "SPRQ":
                       ss = "SPRQ|审批日期";
                       break;
                   case "DJRQ":
                       ss = "DJRQ|登记日期";
                       break;
                   case "DJJS":
                       ss = "DJJS|登记记事";
                       break;
                   case "DJKJBR":
                       ss = "DJKJBR|登记卡经办人";
                       break;
                   case "DJKSHR":
                       ss = "DJKSHR|登记卡审核人";
                       break;

                   default:
                       ss = "";
                       break;
               }
           }
#endregion
           return ss;
       }
       #endregion
       /// <summary>
       /// 获得  DM|MC  左边
       /// </summary>
       /// <param name="sName"></param>
       /// <returns></returns>
       public static string GetLeftName(string sName)
       {
           string ss;
           if (sName == "")
           {
               return "";
           }
           if (sName.Contains("|"))
           {
               ss = sName.Substring(0, sName.IndexOf("|")).Trim();
           }
           else
           {
               return sName;
           }
           return ss;
       }
       /// <summary>
       /// 获得  DM|MC  左边
       /// </summary>
       /// <param name="sName"></param>
       /// <returns></returns>
       public static string GetLeftName(string sName,string sflag)
       {
           try
           {
               string ss;
               if (sName == "")
               {
                   return "";
               }
               if (sName.Contains(sflag))
               {
                   ss = sName.Substring(0, sName.IndexOf(sflag)).Trim();
               }
               else
               {
                   return sName;
               }
               return ss;
           }
           catch { return ""; }
       }
       /// <summary>
       /// 获得  DM|MC  右边
       /// </summary>
       /// <param name="sName"></param>
       /// <returns></returns>
       public static string GetRightName(string sName)
       {
           string ss;
           if (sName == "")
           {
               return "";
           }
           if (sName.Contains("|"))
           {
               ss = sName.Substring(sName.IndexOf("|") + 1, sName.Length - sName.IndexOf("|") - 1).Trim();

           }
           else { return sName; }
           return ss;
       }
       /// <summary>
       /// 获得  DM|MC  右边
       /// </summary>
       /// <param name="sName"></param>
       /// <returns></returns>
       public static string GetRightName(string sName,string sflag)
       {
           try
           {
               string ss;
               if (sName == "")
               {
                   return "";
               }
               if (sName.Contains(sflag))
               {
                   ss = sName.Substring(sName.IndexOf(sflag) + 1, sName.Length - sName.IndexOf(sflag) - 1).Trim();
               }
               else
               {
                   return sName;
               }
               return ss;
           }
           catch { return ""; }
       }
       #region//转换空值为0
       public static int ChangeNullToZero(string str)
       {

           if ((str == "")||(str ==null))
           {
               return 0;
           }
           else
           {
               return Convert.ToInt32(str);
           }
       }
       public static int ChangeNullToZero(object str)
       {

           if ((str == "") || (str == null))
           {
               return 0;
           }
           else
           {
               return Convert.ToInt32(str);
           }
       }
       #endregion
       #region//转换空值为0.0
       public static double ChangeNullToDoubleZero(string str)
       {
           try
           {
               if ((str == "") || (str == null))
               {
                   return 0.0;
               }
               else
               {
                   return Convert.ToDouble(str);
               }
           }
           catch { return 0.0; }
       }
       public static double ChangeNullToDoubleZero(object str)
       {
           try
           {
               if ((str == "") || (str == null))
               {
                   return 0.0;
               }
               else
               {
                   return Convert.ToDouble(str);
               }
           }
           catch (Exception ex)
           { return 0.0; }
       }
       #endregion
       #region//转换空值为""
       public static string  ChangeNullToString(object str)
       {

           if (str == null)
           {
               return "";
           }
           else
           {
               return Convert.ToString(str);
           }

       }
        #endregion

    

     

       public static string ChangeTJJZD(string sJBLX)
       {
           switch (sJBLX)
           {
               case "1":
                   return "A";
               case "2":
                   return "B";
               case "3":
                   return "C";
               case "4":
                   return "D";
               case "5":
                   return "G";
               case "6":
                   return "E";
               case "9":
                   return "F";
               default:
                   return "";
           }
       }

       public static void ExportActiveView(IActiveView pActiveView, string strImagePath)
       {
           IExporter pExporter;
           IEnvelope pEnv;
           tagRECT rectExpFrame;
           int hdc;
           short dpi;
             
               pExporter = new JpegExporterClass();
               

               pEnv = new EnvelopeClass();

               
               rectExpFrame = pActiveView.ExportFrame;

               pEnv.PutCoords(rectExpFrame.left, rectExpFrame.top, rectExpFrame.right, rectExpFrame.bottom);

               dpi = 96;

               pExporter.PixelBounds = pEnv;

               pExporter.ExportFileName = strImagePath;

               pExporter.Resolution = dpi;

               hdc = pExporter.StartExporting();

               pActiveView.Output(hdc, dpi, ref rectExpFrame, null, null);

               pExporter.FinishExporting();
           }

       public static  void LoadField2ComboBox(IWorkspace _Workspace,
           string sLayerName, DevExpress.XtraEditors.ComboBoxEdit cboField)
       {
           if (_Workspace == null) return;
           if (sLayerName == "") return;
           cboField.Properties.Items.Clear();
           cboField.Text = "";
           try
           {
               IFeatureWorkspace sFWS = (IFeatureWorkspace)_Workspace;

               IFeatureClass pFC = sFWS.OpenFeatureClass(sLayerName);

               for (int i = 0; i < pFC.Fields.FieldCount; i++)
               {

                   IField aSrcFld = pFC.Fields.get_Field(i);

                   if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                       || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                       || aSrcFld == pFC.LengthField
                       || aSrcFld == pFC.AreaField)
                   {
                       continue;
                   }
                   cboField.Properties.Items.AddRange(new object[]{
															  aSrcFld.AliasName+"|"+aSrcFld.Name
														  });
                   cboField.Tag = aSrcFld;

               }
           }
           catch { }
       }
       public static void LoadField2ComboBox(IWorkspace _Workspace,
          string sLayerName, ComboBox cboField)
       {
           if (_Workspace == null) return;
           if (sLayerName == "") return;
           cboField.Items.Clear();
           cboField.Text = "";
           try
           {
               IFeatureWorkspace sFWS = (IFeatureWorkspace)_Workspace;

               IFeatureClass pFC = sFWS.OpenFeatureClass(sLayerName);

               for (int i = 0; i < pFC.Fields.FieldCount; i++)
               {

                   IField aSrcFld = pFC.Fields.get_Field(i);

                   if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                       || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                       || aSrcFld == pFC.LengthField
                       || aSrcFld == pFC.AreaField)
                   {
                       continue;
                   }
                   cboField.Items.AddRange(new object[]{
															  aSrcFld.AliasName+"|"+aSrcFld.Name
														  });
                   cboField.Tag = aSrcFld;

               }
           }
           catch { }
       }

       public static void ShowMessageBox(string sInfo)
       {
           MessageBox.Show(sInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
       }

       #region 代码转名称
       public static string ChangeDMToMC(string str, string sTable)
       {
           try
           {
               string ss = null;

               if (str == "")
               {
                   return "";
               }
               string sql = "select * from " + sTable + " where DM='" + str + "'";

               DataSet ds = Helper.DataOperateHelper.getDataSet(sql, sTable);

               if (ds.Tables[sTable].Rows.Count == 0)
               {
                   return "";
               }
               else
               {
                   DataRow dr = ds.Tables[sTable].Rows[0];

                   ss = dr["MC"].ToString();

               }

               return ss;

           }
           catch (Exception ee)
           {
              
               return "";
           }

       }
       #endregion

       public static void ZDTExportActiveView(IActiveView pActiveView, string strImagePath)
       {
           IExporter pExporter;
           IEnvelope pEnv;
           tagRECT rectExpFrame;
           int hdc;
           short dpi;

           pExporter = new JpegExporterClass();

           pEnv = new EnvelopeClass();

           //Setup the exporter
           rectExpFrame = pActiveView.ExportFrame;

           pEnv.PutCoords(rectExpFrame.left, rectExpFrame.top, rectExpFrame.right, rectExpFrame.bottom);

           dpi = 96;

           pExporter.PixelBounds = pEnv;

           pExporter.ExportFileName = strImagePath;

           pExporter.Resolution = dpi;

           hdc = pExporter.StartExporting();

           pActiveView.Output(hdc, dpi, ref rectExpFrame, null, null);

           pExporter.FinishExporting();




       }

       public static string RemoveZero(string s)
       {
           if (s == "") return "";
           string sResult = "";
           try
           {
               if (Convert.ToInt64(s) % 10000000000 == 0)//省级
               {
                   sResult = s.Substring(0, 2);
               }
               //市级
               else if ((Convert.ToInt64(s) % 10000000000 != 0) && (Convert.ToInt64(s) % 100000000 == 0))
               {
                   sResult = s.Substring(0, 4);
               }
               //县级
               else if ((Convert.ToInt64(s) % 10000000000 != 0) && (Convert.ToInt64(s) % 100000000 != 0) && (Convert.ToInt64(s) % 1000000 == 0))
               {
                   sResult = s.Substring(0, 6);
               }
               //街道
               else if ((Convert.ToInt64(s) % 10000000000 != 0) && (Convert.ToInt64(s) % 100000000 != 0) && (Convert.ToInt64(s) % 1000000 != 0) && (Convert.ToInt64(s) % 1000 == 0))
               {
                   sResult = s.Substring(0, 9);
               }
               //街坊
               else
               {
                   sResult = s.Substring(0, 12);
               }
               return sResult;
           }
           catch { return ""; }
       }

       
       /// <summary>
       /// 判断数据集是否注册版本
       /// </summary>
       /// <param name="workspace"></param>
       /// <returns></returns>
       public static bool CheckIsRegister(IWorkspace workspace)
       {
           if (workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
           {
               string userName = workspace.ConnectionProperties.GetProperty("user").ToString();
               userName = userName + ".";
               userName = userName.ToUpper();
               if (workspace != null)
               {
                   IEnumDatasetName dsNames = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
                   IDatasetName dsName = dsNames.Next();

                   while (dsName != null)
                   {
                       string dsNameStr = dsName.Name.ToUpper();
                       if (dsNameStr.StartsWith(userName))
                       {
                           try
                           {
                               IVersionedObject vo = (dsName as ESRI.ArcGIS.esriSystem.IName).Open() as IVersionedObject;
                               if (vo != null)
                               {

                                   if (!vo.IsRegisteredAsVersioned)
                                   {

                                       return false;
                                   }
                                   else
                                   {
                                       return true;
                                   }
                               }
                               return false;
                           }
                           catch (Exception ex)
                           {
                               return false;
                           }
                       }
                       dsName = dsNames.Next();
                   }
                   return false;
               }
               return false;
           }
           return false;
       }

     
       public static double GetArea(IGeometry pGeo,int flag)
       {
           IPolygon pPoly = pGeo as IPolygon;
           return GetArea(pPoly, flag);
       }

       public static  void Kill(string s)
       {
           try
           {
               //释放COM组件，其实就是将其引用计数减1   
               foreach (System.Diagnostics.Process theProc in System.Diagnostics.Process.GetProcessesByName(s))
               {
                   //先关闭图形窗口。如果关闭失败...有的时候在状态里看不到图形窗口的excel了，   
                   //但是在进程里仍然有EXCEL.EXE的进程存在，那么就需要杀掉它:p   
                   if (theProc.CloseMainWindow() == true)
                   {
                       theProc.Kill();
                   }
                   else
                   {
                       theProc.Kill();
                   }
               }

           }
           catch (Exception ex)
           { }
       }

    }
}
