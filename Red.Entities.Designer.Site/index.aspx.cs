using System;
using System.Web;
using System.Web.UI;

namespace Red.Entities.Designer.Site
{
    public partial class index : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            repeater.DataSource = Global.Current.Database.Articles.Search("ab bc");
            repeater.DataBind();
        }
    }
}
