using Microsoft.Ajax.Utilities;
using SimpleProject20200708.Models;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleProject20200708.Controllers
{
    public class HomeController : Controller
    {


  
        //[注意] table in DataBase is class , column on table would be properties for class
        [HttpGet]
        public ActionResult Index()
        {
            //take form view model and display the model ,but how to set the default?


            /* 
                 SimpleProjectStudentEntities content = new SimpleProjectStudentEntities();
                 List<Student> StuList1 = content.Student.OrderBy(m => m.fStudentID).ToList();
                 List<Student> StuList2 = content.Student.OrderByDescending(m => m.fStudentID).ToList();
                 FromDBViewModel vmViewModelFromDB = new FromDBViewModel();
                 FromDBvmViewModel.FirstOrder = StuList1;
                 FromDBvmViewModel.SecondOrder = StuList2;
                 return View(StuList1);                
            */
            using (SimpleProjectStudentEntities content = new SimpleProjectStudentEntities()) //為何要用using
            {
                List<Student> vStuList = content.Student.OrderBy(m => m.fStudentID).ToList();
                return View(vStuList);                                                        //reutrn view 放在using裡面才對
            }
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)//回傳到view model
        {
            using(SimpleProjectStudentEntities content = new SimpleProjectStudentEntities())
            {
            
            List<Student> StuList1 = content.Student.OrderBy(m => m.fStudentID).ToList();
            List<Student> StuList2 = content.Student.OrderByDescending(m => m.fStudentID).ToList();
            List<Student> StuList3 = content.Student.OrderBy(m => m.fScore).ToList();
            List<Student> StuList4 = content.Student.OrderByDescending(m => m.fScore).ToList();
            if (form["IdOrder"] == "1")
                {
                return View(StuList1);
                    return RedirectToAction("Index", "Home", "IdOrder=1");
                }
            if(form["IdOrder"] == "2")
                {
                return View(StuList2);
                    return RedirectToAction("Index", "Home", "IdOrder=2");
                }
            if (form["ScoreOrder"] == "1")
                {
                return View(StuList3);
                   
                }
            if (form["ScoreOrder"] == "2")
                {
                return View(StuList4);
                    return RedirectToAction("Index", "Home", "ScoreOrder=2");
                }
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student StuList)
        {

            using (SimpleProjectStudentEntities content = new SimpleProjectStudentEntities())
            {
                if (ModelState.IsValid)
                {                   
                   content.Student.Add(StuList);
                   content.SaveChanges();
                   return RedirectToAction("Index");//這個加在using內或外有差嗎<=========                    
                }                
                                                    //return RedirectToAction("Index"); 放在這邊會導致下面的return View(Stulist)出現警告 CS0162 偵測到不會執行的程式碼 
                                                    //但是VIEW Home//Index一樣可以顯示，資料庫有寫入
            }
            return View(StuList);//為什麼一定要加這個，這個加在using內或外有差嗎=>一定要加，加在using沒有用
                                 //沒有加的話會出現CS0161 不是所有路徑都有回傳值，實際上StuList不可能全部都有數值，當這個時候沒有辦法回傳
                                 //所以要加上return View 強迫StudList有沒有值都要回傳
        }

        public ActionResult Edit(int id)
        {
            using (SimpleProjectStudentEntities content = new SimpleProjectStudentEntities())
            {
                Student StuList = content.Student.FirstOrDefault(m => m.fStudentID == id);
                return View(StuList);
            }

        }
        [HttpPost]
        public ActionResult Edit(Student StuList)
        {
            var urlid = Url.RequestContext.RouteData.Values["id"].ToString();//取出URL上的id
            int id = Int32.Parse(urlid);

            using (SimpleProjectStudentEntities content = new SimpleProjectStudentEntities())
            {
                if (ModelState.IsValid)
                {
                    Student stuRemoveList = content.Student.FirstOrDefault(m => m.fStudentID == id);
                    content.Student.Remove(stuRemoveList); //這樣只有remove掉Dbset的資料，可以看成移動掉Entity Framework的資料
                    content.SaveChanges();           //要加上savechanges讓Dbset被更改的資料可以寫入SQL server這樣才有更新資料庫，view就會顯示被更改的資料庫                                                 
                    content.Student.Add(StuList);
                    content.SaveChanges();
                }
            }
            return View("Edit",StuList);

        }
        public ActionResult Delete(int iddelete)
        {
            using (SimpleProjectStudentEntities content = new SimpleProjectStudentEntities())
            {               
                Student StuList = content.Student.Where(m => m.fStudentID == iddelete).FirstOrDefault();                
                content.Student.Remove(StuList);
                content.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();


        }

    }
}