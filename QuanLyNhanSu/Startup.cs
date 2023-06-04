using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLyNhanSu.Startup))]
namespace QuanLyNhanSu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            // Configure the application here
        }
    }
}
