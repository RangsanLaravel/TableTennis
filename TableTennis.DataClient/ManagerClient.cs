using SPUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTennis.BusinessLogic;
using TableTennis.DataModels;

namespace TableTennis.DataClient
{
    public class ManagerClient:IDisposable
    {

        public void Dispose()
        {
        }

        public Result Add(ref MANAGER dataObj)
        {
            Result r = new Result();
            ManagerLogic logic = new ManagerLogic();
            try
            {
                logic.Add(ref dataObj);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result Get(out MANAGER dataObj, long? id)
        {
            dataObj = null;
            Result r = new Result();
            ManagerLogic logic = new ManagerLogic();
            try
            {
                dataObj = logic.Get(id);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result SignIn(out MANAGER dataObj, string email, int? id)
        {
            dataObj = null;
            Result r = new Result();
            ManagerLogic logic = new ManagerLogic();
            try
            {
                dataObj = logic.SignIn(email, id);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GridManager(out List<MANAGER> dataObj, ParamManager param)
        {
            dataObj = null;
            Result r = new Result();
            ManagerLogic logic = new ManagerLogic();
            try
            {
                dataObj = logic.GridManager(param);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result Update(ref MANAGER dataObj)
        {
            Result r = new Result();
            ManagerLogic logic = new ManagerLogic();
            try
            {
                logic.Update(ref dataObj);
                r.Successed = true; 
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result SendRecover(out MANAGER dataObj, string email)
        {
            dataObj = null;
            Result r = new Result();
            ManagerLogic logic = new ManagerLogic();
            try
            {
                dataObj = logic.SendRecover(email);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
    }
}
