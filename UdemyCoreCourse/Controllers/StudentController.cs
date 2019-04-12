using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using UdemyCoreCourse.Models;
using Sampan;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace UdemyCoreCourse.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        [HttpGet("[action]")]
        public JsonResult GetStudentList()
        {
            try
            {
                var msg = ""; string Query = "";
                CoreSQLConnection CoreSQL = new CoreSQLConnection();
                List<Student> CatList = new List<Student>();
                var StudentList = CatList;
                try
                {
                    Query = "SELECT * FROM tbl_Student";
                    IDataReader reader = CoreSQL.CoreSQL_GetReader(Query);
                    while (reader.Read())
                    {
                        Student aCat = new Student();
                        aCat.StudentId = Convert.ToInt32(reader["StudentId"].ToString());
                        aCat.StudentName = reader["StudentName"].ToString();
                        aCat.DateOfBirth = reader["DateOfBirth"].ToString();
                        aCat.EmailId = reader["EmailId"].ToString();
                        aCat.Address = reader["Address"].ToString();
                        aCat.Gender = reader["Gender"].ToString();
                        CatList.Add(aCat);
                    }
                    return Json(StudentList);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    return Json(msg);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("[action]")]
        public string GetName()
        {
            string name = "Anower Ullah";
            return name;
        }
        [HttpGet]
        [Route("GetStudentsById/{StudentId}")]
        public IActionResult GetEmaployeeById(int StudentId)
        {
            Student objEmp = new Student();
            int ID = Convert.ToInt32(StudentId);
            try
            {
                try
                {
                    var msg = ""; string Query = "";
                    CoreSQLConnection CoreSQL = new CoreSQLConnection();
                    List<Student> CatList = new List<Student>();
                    var StudentList = CatList; Student aCat = new Student();
                    try
                    {
                        Query = "SELECT * FROM tbl_Student";
                        IDataReader reader = CoreSQL.CoreSQL_GetReader(Query);
                        while (reader.Read())
                        {
                             aCat = new Student();
                            aCat.StudentId = Convert.ToInt32(reader["StudentId"].ToString());
                            aCat.StudentName = reader["StudentName"].ToString();
                            aCat.DateOfBirth = reader["DateOfBirth"].ToString();
                            aCat.EmailId = reader["EmailId"].ToString();
                            aCat.Address = reader["Address"].ToString();
                            aCat.Gender = reader["Gender"].ToString();
                            CatList.Add(aCat);
                        }
                        aCat = CatList.Where(a => a.StudentId == StudentId).FirstOrDefault();
                        return Ok(aCat);
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        return Json(msg);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(objEmp);
        }

        [HttpPost]
        public IActionResult Student(Student data)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var msg = "";
            try
            {
                msg = prcSaveDataSampan(data);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(msg);
        }
        public string prcSaveDataSampan(Student model)
        {
            CoreSQLConnection CoreSQL = new CoreSQLConnection();
            ArrayList arrayList = new ArrayList();
            var Query = "SELECT  cast(Isnull(MAX(CatId),0) + 1 AS float)  AS CatId FROM tbl_Student";
            var variable = CoreSQL.CoreSQL_GetDoubleData(Query);
            try
            {
                var sqlQuery = "Insert Into tbl_Student (StudentId, StudentName, Gender, DateOfBirth, EmailId, Address)" +
                               " Values ('" + variable + "','" + model.StudentName + "','" + model.Gender + "'," +
                               "'" + model.DateOfBirth + "','" + model.EmailId + "','" + model.Address + "')";
                arrayList.Add(sqlQuery);
                CoreSQL.CoreSQL_SaveDataUseSQLCommand(arrayList);
                return "Successfully Save.";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

            }
        }
        //[HttpPut]
        //[Route("UpdateStudents")]
        //public IActionResult PutEmaployeeMaster(Student Student)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        Student objEmp = new Student();
        //        objEmp = objEntity.Students.Find(Student.EmpId);
        //        if (objEmp != null)
        //        {
        //            objEmp.EmpName = Student.EmpName;
        //            objEmp.Address = Student.Address;
        //            objEmp.EmailId = Student.EmailId;
        //            objEmp.DateOfBirth = Student.DateOfBirth;

        //        }
        //        int i = this.objEntity.SaveChanges();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Ok(Student);
        //}
        //[HttpDelete]
        //[Route("DeleteStudents")]
        //public IActionResult DeleteEmaployeeDelete(int id)
        //{
        //    //int empId = Convert.ToInt32(id);
        //    Student emaployee = objEntity.Students.Find(id);
        //    if (emaployee == null)
        //    {
        //        return NotFound();
        //    }
        //    objEntity.Students.Remove(emaployee);
        //    objEntity.SaveChanges();

        //    return Ok(emaployee);
        //}
    }
}


