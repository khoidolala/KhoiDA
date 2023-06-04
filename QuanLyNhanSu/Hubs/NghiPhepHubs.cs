using Microsoft.AspNet.SignalR;
using QuanLyNhanSu.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace QuanLyNhanSu.Hubs
{
    public class NghiPhepHubs : Hub
    {
        public NghiPhepHubs()
        {
            var tableDependency = new SqlTableDependency<tblNghiPhep>(ConfigurationManager.ConnectionStrings["DBQLNVContext"].ConnectionString, tableName: "tblNghiPhep", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<tblNghiPhep> e)
        {
            Show();
            SetLocalStoage();
        }

        public static void SetLocalStoage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NghiPhepHubs>();
            context.Clients.All.setLocalStoageNghiPhep();
        }

        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NghiPhepHubs>();
            context.Clients.All.displayMessageNghiPhep();
        }
    }
}