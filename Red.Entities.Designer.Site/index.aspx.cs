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

            var db = Global.Current.Database;

            var request = db.Articles.CreateFetchRequest();
            var predicate = request.CreatePredicate();

            predicate
                .WhereStringFieldContains("Code", "ABC")
                .WhereDecimalFieldBetween("UnitPrice", 1.0M, 100.0M);

            //var entities = db.Fetch(request);

            repeater.DataSource = db.Fetch(request);
            repeater.DataBind();

        }
    }
}
