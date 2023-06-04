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
    public class ChamCongHubs : Hub
    {
        public ChamCongHubs()
        {
            var tableDependency = new SqlTableDependency<tblChamCongs>(ConfigurationManager.ConnectionStrings["DBQLNVContext"].ConnectionString, tableName: "tblChamCongs", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<tblChamCongs> e)
        {
            Show();
            SetLocalStoage();
        }

        public static void SetLocalStoage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChamCongHubs>();
            context.Clients.All.setLocalStoageChamCong();
        }

        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChamCongHubs>();
            context.Clients.All.displayMessageChamCong();
        }
    }
}