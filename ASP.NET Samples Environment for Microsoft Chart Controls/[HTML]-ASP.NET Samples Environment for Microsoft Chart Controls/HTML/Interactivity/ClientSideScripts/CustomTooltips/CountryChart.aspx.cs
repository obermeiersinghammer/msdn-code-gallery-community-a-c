using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace System.Web.UI.DataVisualization.Charting.Samples
{
	/// <summary>
	/// Summary description for CountryChart.
	/// </summary>
	public partial class CountryChart : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(this.Page.Request["country"] != null)
			{
				string country = (string)this.Page.Request["country"];
				Chart1.Titles[0].Text = country + " - Statistics";
			}

			if(this.Page.Request["gold"] != null)
			{
				Chart1.Series["Default"].Points.AddY(int.Parse(this.Page.Request["gold"]));
			}
			if(this.Page.Request["silver"] != null)
			{
				Chart1.Series["Default"].Points.AddY(int.Parse(this.Page.Request["silver"]));
			}
			if(this.Page.Request["bronze"] != null)
			{
				Chart1.Series["Default"].Points.AddY(int.Parse(this.Page.Request["bronze"]));
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
