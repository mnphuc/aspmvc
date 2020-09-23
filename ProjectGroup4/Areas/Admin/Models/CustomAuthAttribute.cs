using Project.BAL.Repositories;
using Project.Models.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Areas.Admin.Models
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Kiểm tra đã đăng nhập hay chưa
            if (HttpContext.Current.Session["user"] == null)
            {
                return false;
            }


            // Nếu đã đăng nhập -->  tiếp tục kiểm tra
            // Kiểm tra action hiện tại có yêu cầu quyền
            if (String.IsNullOrEmpty(this.Roles))
            {
                // Nếu ko có yêu cầu quyền thì tiếp tục truy cập bt
                return true;
            }
            // Nếu có yêu cầu quyền
            // Lấy tên controller hiện tại
            var controller = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");

            // Lấy các quyền của người dùng
            Repository<GroupRoles> _groupRole = new Repository<GroupRoles>();
            var _user = HttpContext.Current.Session["user"] as Users;
            var _groupRoles = _groupRole.GetBy(x => x.GroupId == _user.GroupId);

            // Check xem trong đó có quyền yêu cầu hay
            if (!_groupRoles.Any(x => x.BusinessId == controller && x.RoleId == this.Roles))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Hàm sẽ gọi khi mà xác thực không thành công
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult()
            {
                ViewName = "Unauthorized"
            };
        }
    }
}