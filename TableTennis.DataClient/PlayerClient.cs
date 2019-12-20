using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPUtility;
using TableTennis.BusinessLogic;
using TableTennis.DataModels;

namespace TableTennis.DataClient
{
    public class PlayerClient: IDisposable
    {
        public void Dispose()
        {
        }

        public Result Add(ref PLAYER dataObj)
        {
            Result r = new Result();
            PlayerLogic logic = new PlayerLogic();
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

        public Result Get(out PLAYER dataObj,int? id)
        {
            dataObj = null;
            Result r = new Result();
            PlayerLogic logic = new PlayerLogic();
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

        public Result GridResult(out List<PLAYER> dataObj, ParamPlayer param)
        {
            dataObj = null;
            Result r = new Result();
            PlayerLogic logic = new PlayerLogic();
            try
            {
                dataObj = logic.GridResult(param);
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
