﻿    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WinJiaoJing
{
    public partial class FrmAnQingXiangQingList : Form
    {
        public string sID = "";
        public FrmAnQingXiangQingList()
        {
            InitializeComponent();
        }
        public FrmAnQingXiangQingList(string _ID)
        {
            sID = _ID;
            InitializeComponent();
        }

      
       

        private void gv_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //int hand = e.RowHandle;
            //if (hand < 0) return;
            //DataRow dr = this.gv.GetDataRow(hand);
            //if (dr == null) return;
            //switch (dr["State"].ToString().Trim())
            //{
            //    case "已报":
            //        e.Appearance.ForeColor = Color.Yellow;// 改变行背景颜色                
            //        break;
            //    case "不合格":
            //        e.Appearance.ForeColor = Color.Red;// 改变行背景颜色
            //        break;
            //    case "报废中":
            //        e.Appearance.ForeColor = Color.Yellow;// 改变行背景颜色
            //        break;
            //    case "报废完结":
            //        e.Appearance.ForeColor = Color.Green;// 改变行背景颜色
            //        break;
            //}
        }

        private void FrmAnQingXiangQingList_Load(object sender, EventArgs e)
        {

            string sError = "";
            StringBuilder strSql1 = new StringBuilder();



            SqlDataReader redupupasc = SqlHelper.ExecuteReader(CommandType.Text, $"select x.*,a.GongSiA,a.GongSiB,a.GongSiD from T_AnQingXiang x join T_AnQing a on x.AnQingId=a.AnQingNo  where x.AnQingId={sID} ; ",null,out sError);

            //查看详情时 从新赋值公司
            while (redupupasc.Read())
            {

                if (Convert.ToInt32(redupupasc["BaoType_Id"]) == 1)
                {
                    SqlHelper.ExecuteNonQuery(CommandType.Text, $"update T_AnQingXiang set GongSiID={redupupasc["GongSiA"]} where AnQingId={sID} and AnQingXiang_ID={redupupasc["AnQingXiang_ID"]}", null,out sError);
                }
                if (Convert.ToInt32(redupupasc["BaoType_Id"]) == 2)
                {
                    SqlHelper.ExecuteNonQuery(CommandType.Text, $"update T_AnQingXiang set GongSiID={redupupasc["GongSiB"]} where AnQingId={sID} and AnQingXiang_ID={redupupasc["AnQingXiang_ID"]}", null, out sError);
                }
                if (Convert.ToInt32(redupupasc["BaoType_Id"]) == 3)
                {
                    SqlHelper.ExecuteNonQuery(CommandType.Text, $"update T_AnQingXiang set GongSiID={redupupasc["GongSiD"]} where AnQingId={sID} and AnQingXiang_ID={redupupasc["AnQingXiang_ID"]}", null, out sError);
                }
            }
            redupupasc.Close();

            strSql1 = new StringBuilder();
            strSql1.Append(" select AnQingXiang_ID,AnQingId,XiangMuNo,Bao_Desc,XiangMuName,XiangBaoJia,GongSiName from T_AnQingXiang ax ");
            strSql1.Append(" join T_XiangMu xm on ax.XiangMuId=xm.XiangMuID");
            strSql1.Append(" join T_BaoType bt on ax.BaoType_Id=bt.Bao_TypeId");
            strSql1.Append(" join T_GongSi gs on ax.GongSiID=gs.GongSiId");
            strSql1.Append(" where AnQingId=" + sID + " order by xm.XiangMuID");


           
            DataTable dt = SqlHelper.RunQuery(CommandType.Text, strSql1.ToString(), null, out sError);
            this.grd.DataSource = dt;

            strSql1 = new StringBuilder();
            strSql1.Append(" select BaoSum from T_AnQing");
            strSql1.Append(" where AnQingNo=" + sID);
            SqlDataReader red = SqlHelper.ExecuteReader(CommandType.Text, strSql1.ToString(), null, out sError);

          
            while (red.Read())
            {
                this.label2.Text = red[0].ToString(); 
            }
            red.Close();
                
            grd.RefreshDataSource();
        }
    }
}
