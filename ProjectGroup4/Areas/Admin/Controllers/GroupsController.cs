using Project.BAL.Repositories;
using Project.Models.DataMapper;
using ProjectGroup4.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Areas.Admin.Controllers
{
    public class GroupsController : BaseController
    {
        Repository<Group> _group;
        Repository<Business> _business;
        Repository<Role> _role;
        Repository<GroupRoles> _groupRole;
        public GroupsController()
        {
            _group = new Repository<Group>();
            _business = new Repository<Business>();
            _role = new Repository<Role>();
            _groupRole = new Repository<GroupRoles>();
        }
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            ViewBag.businesses = _business.GetAll();
            ViewBag.roles = _role.GetAll();
            return View(_group.GetAll());
        }
       
        [HttpPost]
        public ActionResult Grant(GroupRoles gr)
        {
            string msg = "";
            // 1. Gán
            // 2. Hủy
            // Kiểm tra quyền đã có hay chưa
            if (_groupRole.GetAll().Any(x => x.GroupId == gr.GroupId && x.BusinessId == gr.BusinessId && x.RoleId == gr.RoleId))
            {
                // Quyền đã tồn tại => Hủy
                var _gr = _groupRole.SingleBy(x => x.GroupId == gr.GroupId && x.BusinessId == gr.BusinessId && x.RoleId == gr.RoleId);
                // Hủy quyền (xóa)
                _groupRole.Remove(_gr);
                msg = "Hủy quyền thành công!";
            }
            else
            {
                // Quyền chưa tồn tại => Gán quyền
                _groupRole.Add(gr);
                msg = "Gán quyền thành công!";
            }
            return Json(new
            {
                StatusCode = 200,
                Message = msg


            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRoles(int id)
        {
            var data = _groupRole.GetBy(x => x.GroupId == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [CustomAuth(Roles = "VIEW")]
        public ActionResult GetGroup()
        {
            return View(_group.GetAll());
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateGroup(Group g)
        {
            if (ModelState.IsValid)
            {
                if (_group.Add(g))
                {
                    return RedirectToAction("GetGroup");
                }
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditGroup(int id)
        {
            return View(_group.Get(id));
        }
        [HttpPost]
        public ActionResult EditGroup(int id, Group g)
        {
            if (ModelState.IsValid)
            {
                if (_group.Edit(g))
                {
                    return RedirectToAction("GetGroup");
                }
                return View();
            }
            return View();
        }
        [CustomAuth(Roles = "DELETE")]
        public ActionResult DeleteGroup(int id, Group g)
        {
            _group.Remove(id);
            return View();
        }
    }
}
