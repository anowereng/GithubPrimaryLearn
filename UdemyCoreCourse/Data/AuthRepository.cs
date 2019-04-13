using System;
using System.Threading.Tasks;
using Sampan;
using UdemyCoreCourse.Models;
using System.Data;
using System.Collections.Generic;
using System.Collections;
namespace UdemyCoreCourse.Data
{
    public class AuthRepository
    {
        public User Login(string username, string password)
        {
            // var user = a
            var user=  GetUser(username);
            if(user ==null){
               return null;     
            }

            if (!VerifyPasswordHash(password,user.PasswordHash,user.PasswordHash))
            return null;
            return user;

        }

        private bool VerifyPasswordHash(string password, byte[]passwordHash,byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
               }
            }
            return true;
        }

        public User Register(User user, string password)
        {
            byte[] passwordHash, passwordsalt;
            CreatePasswordHash(password,out passwordHash, out passwordsalt);
            user.PasswordHash=passwordHash;
            user.PasswordSalt=passwordsalt;
            prcRegisterUser(user);
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordsalt)
        {
           using(var hmac = new System.Security.Cryptography.HMACSHA512()){
               passwordsalt=hmac.Key;
               passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           };
        }

        public bool UserExists(string username)
        {
            CoreSQLConnection CoreSQL = new CoreSQLConnection();
            ArrayList arrayList = new ArrayList();
            var Query = "SELECT  cast(Isnull(MAX(CatId),0) AS float)  AS CatId FROM tbl_Student where username='"+username+"'";
            var variable = CoreSQL.CoreSQL_GetDoubleData(Query);
            try
            {
                if(variable>0) return false; else return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
            }
        }

        /* =================== SQL Area  ======================== */


        public string prcRegisterUser(User model)
        {
            CoreSQLConnection CoreSQL = new CoreSQLConnection();
            ArrayList arrayList = new ArrayList();
            var Query = "SELECT  cast(Isnull(MAX(Id),0) + 1 AS float)  AS Id FROM tbl_Student";
            var variable = CoreSQL.CoreSQL_GetDoubleData(Query);
            try
            {
                var sqlQuery = "Insert Into tbl_loginUsers (Id, UserName, PasswordHash, PasswordSalt)" +
                               " Values ('" + variable + "','" + model.UserName + "','" + model.PasswordHash + "'," +
                               "'" + model.PasswordSalt + "')";
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
        public User GetUser(string username)
        {
            try
            {
                 string Query = "";
              CoreSQLConnection CoreSQL = new CoreSQLConnection();User aCat = new User();
                    Query = "SELECT * FROM tbl_loginUsers where username='"+username+"'";
                    IDataReader reader = CoreSQL.CoreSQL_GetReader(Query);
                    while (reader.Read())
                    {
                        aCat.UserName = reader["UserName"].ToString();
                        aCat.PasswordHash =(byte[])(reader["PasswordHash"]);
                        aCat.PasswordSalt = (byte[])reader["PasswordSalt"];
                        break;
                    }
                return aCat;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

            }
            
        }
    }
    
}