using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBackX
{
    public class CallBack
    {
        delegate int AsyncSum(int a, int b);

        private static void PostAsync(int a, int b)
        {
            AsyncSum caller = MySum;
            //FooCallBack,caller为额外的参数
            caller.BeginInvoke(a, b, FooCallBack, caller);   //第三个参数为回调  

        }

        private static void FooCallBack(IAsyncResult ar)
        {
            AsyncSum caller = (AsyncSum)ar.AsyncState; ;//获得BeginInvoke第4个参数
            int number = caller.EndInvoke(ar);//获取运算的结果
        }

        private static int MySum(int a, int b)
        {
            int c = a + b;
            return c;

        }
    }
}
