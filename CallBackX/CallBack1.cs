using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBackX
{
    public class CallBack1
    {
        Func<int, int, int> caller;

        public void PostAsync(int a, int b)
        {
            caller = MySum;
            //FooCallBack,caller为额外的参数
            caller.BeginInvoke(a, b, FooCallBack, caller);   //第三个参数为回调  

        }

        private void FooCallBack(IAsyncResult ar)
        {
            caller = (Func<int, int, int>)ar.AsyncState; ;//获得BeginInvoke第4个参数
            int number = caller.EndInvoke(ar);//获取运算的结果
        }

        private int MySum(int a, int b)
        {
            int c = a + b;
            return c;

        }
    }
}
